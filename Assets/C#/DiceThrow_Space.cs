using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class DiceThrow_Space : MonoBehaviour
{
    [Header("Throw Settings")]
    public float thrustPower = 7f;
    public Vector2 thrustDirection = Vector2.up;
    public float torqueRange = 15f;

    [Header("Stop Detection")]
    public float stopVelocityThreshold = 0.05f;
    public float stopAngularThreshold = 5f;

    [Header("UI (Optional)")]
    public Text resultText;

    [Header("Dice Sprites (1~6)")]
    public Sprite[] diceFaces; // 在 Inspector 拖入六張骰子圖片

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isRolling = false;
    private bool resultShown = false;
    private int currentRandom = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (resultText != null) resultText.text = "";
    }

    void Update()
    {
        // 空白鍵投擲
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ThrowDice();
        }

        // 投擲中 → 顯示隨機數字閃爍
        if (isRolling && !resultShown)
        {
            currentRandom = Random.Range(1, 7);

            // UI 顯示閃爍數字
            if (resultText != null)
                resultText.text = currentRandom.ToString();

            // Sprite 也跟著閃爍
            if (diceFaces != null && diceFaces.Length >= 6)
                sr.sprite = diceFaces[currentRandom - 1];

            // 檢查是否停止
            if (rb.linearVelocity.magnitude < stopVelocityThreshold &&
                Mathf.Abs(rb.angularVelocity) < stopAngularThreshold)
            {
                ShowResult();
            }
        }

        // R 重置
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetDice();
        }
    }

    void ThrowDice()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.AddForce(thrustDirection.normalized * thrustPower, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-torqueRange, torqueRange), ForceMode2D.Impulse);

        isRolling = true;
        resultShown = false;
        Debug.Log("Dice thrown!");
    }

    void ShowResult()
    {
        int result = Random.Range(1, 7);
        resultShown = true;
        isRolling = false;

        // 換 Sprite
        if (diceFaces != null && diceFaces.Length >= 6)
            sr.sprite = diceFaces[result - 1];

        if (resultText != null)
            resultText.text = "🎲 " + result;

        Debug.Log("骰子點數：" + result);

        // 比對目標
        TargetNumberDisplay target = FindObjectOfType<TargetNumberDisplay>();
        if (target != null)
        {
            if (result == target.targetNumber)
            {
                if (resultText != null)
                    resultText.text += "\n✅ 恭喜！投中目標";
                Debug.Log("✅ 恭喜！投中目標");
            }
            else
            {
                if (resultText != null)
                    resultText.text += "\n❌ 沒有投中，目標是 " + target.targetNumber;
                Debug.Log("❌ 沒有投中，目標是 " + target.targetNumber);
            }
        }
    }

    void ResetDice()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = Vector3.zero;
        isRolling = false;
        resultShown = false;
        if (resultText != null) resultText.text = "";
        Debug.Log("Dice reset.");
    }
}
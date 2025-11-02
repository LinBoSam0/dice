using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
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

    private Rigidbody2D rb;
    private bool isRolling = false;
    private bool resultShown = false;
    private int currentRandom = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            if (resultText != null)
                resultText.text = currentRandom.ToString();

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
        int result = Random.Range(1, 7); // 最終結果
        resultShown = true;
        isRolling = false;

        if (resultText != null)
            resultText.text = "🎲骰子點數 " + result;

        Debug.Log("骰子點數：" + result);
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
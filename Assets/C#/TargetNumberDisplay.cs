using UnityEngine;
using UnityEngine.UI;

public class TargetNumberDisplay : MonoBehaviour
{
    [Header("UI")]
    public Text targetText;

    [Header("Target Number")]
    [Range(1, 6)]
    public int targetNumber = 1;

    void Start()
    {
        UpdateTargetText();
    }

    // 隨機產生一個目標點數
    public void GenerateNewTarget()
    {
        targetNumber = Random.Range(1, 7);
        UpdateTargetText();
    }

    void UpdateTargetText()
    {
        if (targetText != null)
            targetText.text = "🎯 目標點數：" + targetNumber;
    }
}
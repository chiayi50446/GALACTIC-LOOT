using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Image barImage;  // 進度條（Bar）的 Image 組件
    [SerializeField] private float maxValue = 100f;  // 最大值
    [SerializeField] private float initValue = 100f;
    private float currentValue;  // 當前數值

    void Start()
    {
        currentValue = initValue;  // 初始化數值
        UpdateBar();  // 更新 Bar
    }

    public void SetMaxAndInitValue(float max)
    {
        maxValue = max;
        initValue = max;
    }

    public float GetValue()
    {
        return currentValue;
    }

    public void SetValue(float value)
    {
        currentValue = Mathf.Clamp(value, 0, maxValue); // 限制範圍
        StartCoroutine(SmoothUpdateBar(currentValue / maxValue));
    }

    private void UpdateBar()
    {
        if (barImage != null)
        {
            barImage.fillAmount = currentValue / maxValue; // 設定填充比例 (0~1)
        }
    }
    public IEnumerator SmoothUpdateBar(float targetValue)
    {
        float startValue = barImage.fillAmount;
        float time = 0f;
        while (time < 0.5f) // 0.5 秒內變化
        {
            time += Time.unscaledDeltaTime;
            barImage.fillAmount = Mathf.Lerp(startValue, targetValue, time / 0.5f);
            yield return null;
        }
    }
}

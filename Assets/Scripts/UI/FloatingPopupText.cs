using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingPopupText : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private float riseDistance = 90f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float punchScale = 1.4f;
    [SerializeField] private float punchDuration = 0.15f;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Play(string text, Color color, Vector2 anchoredPosition)
    {
        if (label != null)
        {
            label.text = text;
            label.color = color;
        }

        rectTransform.anchoredPosition = anchoredPosition;
        transform.localScale = Vector3.zero;

        StartCoroutine(PlayRoutine(anchoredPosition, color));
    }

    IEnumerator PlayRoutine(Vector2 origin, Color baseColor)
    {
        float t = 0f;
        while (t < punchDuration)
        {
            t += Time.deltaTime;
            float scale = Mathf.LerpUnclamped(0f, punchScale, EaseOutBack(t / punchDuration));
            transform.localScale = Vector3.one * scale;
            yield return null;
        }

        float settleDuration = 0.1f;
        float settleElapsed = 0f;
        while (settleElapsed < settleDuration)
        {
            settleElapsed += Time.deltaTime;
            float scale = Mathf.Lerp(punchScale, 1f, settleElapsed / settleDuration);
            transform.localScale = Vector3.one * scale;
            yield return null;
        }

        float riseDuration = Mathf.Max(lifetime - punchDuration - settleDuration, 0.1f);
        float riseElapsed = 0f;
        while (riseElapsed < riseDuration)
        {
            riseElapsed += Time.deltaTime;
            float phase = riseElapsed / riseDuration;
            rectTransform.anchoredPosition = origin + Vector2.up * (riseDistance * phase);

            if (label != null)
            {
                float alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp01((phase - 0.4f) / 0.6f));
                label.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    static float EaseOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);
    }
}

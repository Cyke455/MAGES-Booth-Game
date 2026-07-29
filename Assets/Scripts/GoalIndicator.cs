using System.Collections;
using UnityEngine;

public class GoalIndicator : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;

    [Header("Juice")]
    [SerializeField] private float popScale = 1.2f;
    [SerializeField] private float popDuration = 0.25f;

    private static readonly int IsLitHash = Animator.StringToHash("IsLit");

    private bool isLit;
    private Vector3 baseScale;
    private Coroutine popRoutine;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    void OnEnable()
    {
        SpeedBar.OnSpeedUpdate += HandleSpeedUpdate;
    }

    void OnDisable()
    {
        SpeedBar.OnSpeedUpdate -= HandleSpeedUpdate;
    }

    void HandleSpeedUpdate(float newValue, bool instantUpdate)
    {
        if (!animator.isActiveAndEnabled) return;

        if (newValue >= gameManager.winSpeed)
        {
            if (!isLit) Pop();
            isLit = true;
            animator.SetBool(IsLitHash, true);
        }
        else if (instantUpdate)
        {
            isLit = false;
            animator.SetBool(IsLitHash, false);
        }
    }

    void Pop()
    {
        if (popRoutine != null) StopCoroutine(popRoutine);
        popRoutine = StartCoroutine(PopRoutine());
    }

    IEnumerator PopRoutine()
    {
        float t = 0f;
        while (t < popDuration)
        {
            t += Time.deltaTime;
            float phase = Mathf.Sin(Mathf.Clamp01(t / popDuration) * Mathf.PI);
            transform.localScale = baseScale * (1f + (popScale - 1f) * phase);
            yield return null;
        }

        transform.localScale = baseScale;
        popRoutine = null;
    }
}

using UnityEngine;

public class GoalIndicator : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;

    private static readonly int IsLitHash = Animator.StringToHash("IsLit");

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

        if (newValue >= gameManager.winSpeed) animator.SetBool(IsLitHash, true);
        else if (instantUpdate) animator.SetBool(IsLitHash, false);
    }
}

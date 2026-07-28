using UnityEngine;

public class HermesController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speedMultiplier = 0.05f;
    [SerializeField] private float distancePerPress = 0.02f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float minAnimSpeed = 0.15f;
    [SerializeField] private float maxAnimSpeed = 1.5f;
    [SerializeField] private float animSpeedSmoothing = 0.15f;

    public bool GameActive = true;
    public float DistanceTraveled { get; private set; }

    private float currentSpeedValue;
    private Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.position;
    }

    void OnEnable()
    {
        SpeedBar.OnSpeedUpdate += HandleSpeedUpdate;
        SpeedBar.OnKeyPressed += HandleKeyPressed;
    }

    void OnDisable()
    {
        SpeedBar.OnSpeedUpdate -= HandleSpeedUpdate;
        SpeedBar.OnKeyPressed -= HandleKeyPressed;
    }

    void Update()
    {
        UpdateAnimationSpeed();

        if (!GameActive || currentSpeedValue <= 0f) return;

        Move(currentSpeedValue * speedMultiplier * Time.deltaTime);
    }

    void HandleSpeedUpdate(float newValue, bool instantUpdate)
    {
        currentSpeedValue = newValue;
    }

    void HandleKeyPressed()
    {
        if (!GameActive) return;

        Move(distancePerPress);

        SoundManager.Instance.PlaySFX(SoundType.Run);
        SoundManager.Instance.PlaySFX(SoundType.Wind);
    }

    void Move(float delta)
    {
        transform.Translate(Vector3.right * delta, Space.World);
        DistanceTraveled += delta;
    }

    void UpdateAnimationSpeed()
    {
        if (animator == null || !animator.isActiveAndEnabled) return;

        // Not GameActive (countdown/goal-reached) means presses aren't updating
        // currentSpeedValue, so force the fraction down rather than leaving Hermes
        // running in place at whatever speed he last had.
        float fraction = GameActive && gameManager != null ? Mathf.Clamp01(currentSpeedValue / gameManager.winSpeed) : 0f;
        float targetSpeed = Mathf.Lerp(minAnimSpeed, maxAnimSpeed, fraction);
        float alpha = 1f - Mathf.Exp(-Time.deltaTime / animSpeedSmoothing);
        animator.speed = Mathf.Lerp(animator.speed, targetSpeed, alpha);
    }

    public void ResetHermes()
    {
        transform.position = startPosition;
        DistanceTraveled = 0f;
        currentSpeedValue = 0f;
        if (animator != null) animator.speed = minAnimSpeed;
    }
}

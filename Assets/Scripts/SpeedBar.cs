using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpeedBar : MonoBehaviour
{
    // Events
    public static event Action<float, bool> OnSpeedUpdate;
    public static event Action OnKeyPressed;


    public PlayerInputActions InputActions;

    public float SpeedValue;
    public bool GameActive = true;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private float speedDecay = 0.35f;
    [SerializeField] private float SpeedDecayTime = 0.1f;
    [SerializeField] private float speedIncrease = 0f;

    

    public bool SpeedDecreaseToggle = true;
    private bool SpeedDecreased = false;

    void Awake()
    {
       InputActions = new PlayerInputActions();
       StartCoroutine(SpeedDecay());
    }

    void OnEnable()
    {
        InputActions.Enable();
        InputActions.Player.Interact.performed += OnInteract;
    }

    void OnDisable()
    {
        InputActions.Player.Interact.performed -= OnInteract;
        InputActions.Disable();
    }

    void Start()
    {
        OnSpeedUpdate?.Invoke(SpeedValue, false);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (!GameActive || !context.ReadValueAsButton()) return;
        SpeedIncrease();
        OnKeyPressed?.Invoke();
    }

    public void ResetSpeed()
    {
        SpeedValue = 0f;
        SpeedDecreased = false;
        OnSpeedUpdate?.Invoke(SpeedValue, true);
    }

    void SpeedDecrease()
    {
        SpeedValue = Mathf.Max(SpeedValue - speedDecay, 0);
        OnSpeedUpdate?.Invoke(SpeedValue, false);
    }

    void SpeedIncrease()
    {
        bool wasAtGoal = SpeedValue >= gameManager.winSpeed;
        SpeedValue = Mathf.Clamp(SpeedValue + speedIncrease, 0, gameManager.winSpeed);
        OnSpeedUpdate?.Invoke(SpeedValue, false);
        if (SpeedValue >= gameManager.winSpeed && !wasAtGoal)
        {
            SoundManager.Instance.PlaySFX(SoundType.GoalBonus);
            gameManager.StageClear();
        }
    }

    IEnumerator SpeedDecay()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpeedDecayTime);
            if (GameActive && SpeedDecreaseToggle && !SpeedDecreased)
            {
                speedDecay = 0.9f+(0.1f * gameManager.gameDifficulty);
                SpeedDecrease();
            }
        }
    }

    void Update()
    {

    }
}
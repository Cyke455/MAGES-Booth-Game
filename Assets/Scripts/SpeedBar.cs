using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpeedBar : MonoBehaviour
{
    public static event Action<float> OnSpeedUpdate;

    public PlayerInputActions InputActions;
    public float SpeedValue = 100;


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
        OnSpeedUpdate?.Invoke(SpeedValue);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact triggered!");
        SpeedIncrease();
    }

    void SpeedDecrease()
    {
        SpeedValue = Mathf.Max(SpeedValue - speedDecay, 0);
        OnSpeedUpdate?.Invoke(SpeedValue);
    }

    void SpeedIncrease()
    {
        SpeedValue = Mathf.Max(SpeedValue + speedIncrease, 0);
        OnSpeedUpdate?.Invoke(SpeedValue);
    }

    IEnumerator SpeedDecay()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpeedDecayTime);
            if (SpeedDecreaseToggle && !SpeedDecreased)
            {
                SpeedDecrease();
            }
        }
    }

    void Update()
    {

    }
}
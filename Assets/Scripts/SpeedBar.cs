using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class SpeedBar : MonoBehaviour
{
    public PlayerInputActions InputActions;
    public float SpeedValue = 100;
    public static event Action<float> OnSpeedUpdate;

    [SerializeField] private float speedDecay = 0.35f;
    private float SpeedDecayTime = 0.1f;
    [SerializeField] private float speedIncrease = 0f;

    public bool SpeedDecreaseToggle = true;
    private bool SpeedDecreased = false;
    private void Awake()
    {
       InputActions = new PlayerInputActions();
       StartCoroutine(SpeedDecay());
    }

    private void OnEnable()
    {
        InputActions.Enable();
        InputActions.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        InputActions.Player.Interact.performed -= OnInteract;
        InputActions.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact triggered!");
        SpeedIncrease(1f);
    }

    private void SpeedDecrease(float decrease)
    {
        SpeedValue = Mathf.Max(SpeedValue - decrease, 0);
        OnSpeedUpdate?.Invoke(SpeedValue);
    }

    private void SpeedIncrease(float increase)
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
                SpeedDecrease(speedDecay);
            }
        }
    }

    void Update()
    {

    }
}
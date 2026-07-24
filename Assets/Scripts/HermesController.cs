using UnityEngine;

public class HermesController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speedMultiplier = 0.05f;
    [SerializeField] private float distancePerPress = 0.02f;

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
        if (!GameActive || currentSpeedValue <= 0f) return;

        Move(currentSpeedValue * speedMultiplier * Time.deltaTime);
    }

    void HandleSpeedUpdate(float newValue)
    {
        currentSpeedValue = newValue;
    }

    void HandleKeyPressed()
    {
        if (!GameActive) return;

        Move(distancePerPress);
    }

    void Move(float delta)
    {
        transform.Translate(Vector3.right * delta, Space.World);
        DistanceTraveled += delta;
    }

    public void ResetHermes()
    {
        transform.position = startPosition;
        DistanceTraveled = 0f;
        currentSpeedValue = 0f;
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    [SerializeField] private GameManager gameManager;

    [Header("Speed Sync")]
    [SerializeField] private float minSpeedFactor = 0.15f;
    [SerializeField] private float maxSpeedFactor = 1.5f;
    [SerializeField] private float speedSmoothing = 0.15f;

    private float currentScroll;
    private float currentSpeedValue;
    private float speedFactor;

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
        currentSpeedValue = newValue;
        if (instantUpdate) speedFactor = Mathf.Lerp(minSpeedFactor, maxSpeedFactor, Mathf.Clamp01(newValue / gameManager.winSpeed));
    }

    // Update is called once per frame
    void Update()
    {

        float targetSpeedFactor = Mathf.Lerp(minSpeedFactor, maxSpeedFactor, Mathf.Clamp01(currentSpeedValue / gameManager.winSpeed));
        float alpha = 1f - Mathf.Exp(-Time.deltaTime / speedSmoothing);
        speedFactor = Mathf.Lerp(speedFactor, targetSpeedFactor, alpha);

        float particleSpeed = (gameManager.gameDifficulty * speedFactor * 1.5f);
        
        var mainModule = particle.main;
        

        mainModule.simulationSpeed = particleSpeed;
    }
}

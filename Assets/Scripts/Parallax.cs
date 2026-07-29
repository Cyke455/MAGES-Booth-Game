using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float speedChange;
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
        float difficultyFactor = 1+((gameManager.gameDifficulty-1)/speedChange);
    
        float targetSpeedFactor = Mathf.Lerp(minSpeedFactor, maxSpeedFactor, Mathf.Clamp01(currentSpeedValue / gameManager.winSpeed));
        float alpha = 1f - Mathf.Exp(-Time.deltaTime / speedSmoothing);
        speedFactor = Mathf.Lerp(speedFactor, targetSpeedFactor, alpha);
        
        currentScroll += scrollSpeed * difficultyFactor * speedFactor * Time.deltaTime;
        currentScroll = currentScroll%1920;
        transform.localPosition =  new Vector3(960-currentScroll, transform.localPosition.y);
    }
}

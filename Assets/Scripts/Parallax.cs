using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float scrollSpeed; 
    [SerializeField] private float speedChange; 
    [SerializeField] private GameManager gameManager;

    private float currentScroll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newSpeed = 1+((gameManager.gameDifficulty-1)/speedChange);

        currentScroll += scrollSpeed * newSpeed * Time.deltaTime;
        currentScroll = currentScroll%1920;
        transform.localPosition =  new Vector3(960-currentScroll, transform.localPosition.y);
    }
}

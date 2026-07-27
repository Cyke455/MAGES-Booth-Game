using UnityEngine;
using UnityEngine.UI;

public class CountdownDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] numbers;
    [SerializeField] private Transform run;
    [SerializeField] private Image runImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void DisplayNumber(int number)
    {
        image.enabled = true;
        runImage.color = new Color(1, 1, 1, 0);
        runImage.enabled = false;

        image.sprite = numbers[number];
        image.color = new Color(1f, 1f, 1f, 0f);
        transform.rotation = Quaternion.Euler(0, 0, 20);
        transform.localScale = new Vector3(1, 1, 1) * 0.5f;
    }

    public void DisplayStart()
    {
        image.enabled = false;
        runImage.enabled = true;
        run.localScale = new Vector3(1, 1, 1) * 0.75f;
        runImage.color = Color.white;
    }
    // Update is called once per frame
    void Update()
    {
        float colorAlpha = Mathf.Clamp(1f - Mathf.Exp(-Time.deltaTime / 0.2f), 0, 1);
        float movementAlpha = Mathf.Clamp(1f - Mathf.Exp(-Time.deltaTime / 0.3f), 0, 1);
        image.color = Color.Lerp(image.color, new Color(1, 1, 1, 1f), colorAlpha);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, movementAlpha);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), movementAlpha);

        float runColorAlpha = Mathf.Clamp(1f - Mathf.Exp(-Time.deltaTime / 0.4f), 0, 1);
        float runMovementAlpha = Mathf.Clamp(1f - Mathf.Exp(-Time.deltaTime / 0.5f), 0, 1);
        runImage.color = Color.Lerp(runImage.color, new Color(1, 1, 1, 0f), runColorAlpha);
        run.localScale = Vector3.Lerp(run.localScale, new Vector3(1, 1, 1), runMovementAlpha);
        runImage.enabled = (runImage.color.a > 0.01f);

    }
}

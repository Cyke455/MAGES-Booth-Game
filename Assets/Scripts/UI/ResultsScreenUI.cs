using TMPro;
using UnityEngine;

public class ResultsScreenUI : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private GameManager gameManager;

    void OnEnable()
    {
        finalScoreText.text = scoreManager.Score.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.ExitResultsScreen();
        }
    }
}

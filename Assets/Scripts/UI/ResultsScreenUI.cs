using TMPro;
using UnityEngine;

public class ResultsScreenUI : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TMP_Text finalScoreText;

    void OnEnable()
    {
        finalScoreText.text = scoreManager.Score.ToString();
    }
}

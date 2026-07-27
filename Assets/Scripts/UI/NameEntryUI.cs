using TMPro;
using UnityEngine;

public class NameEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameManager gameManager;

    public void Submit()
    {
        gameManager.SubmitName(nameInput.text);
        nameInput.text = string.Empty;
    }
}

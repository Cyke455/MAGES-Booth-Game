using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private FloatingPopupText popupPrefab;
    [SerializeField] private RectTransform spawnParent;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private RectTransform comboSpawnPoint;
    [SerializeField] private RectTransform bonusSpawnPoint;

    [Header("Colors")]
    [SerializeField] private Color comboColor = new Color(1f, 0.35f, 0.1f);
    [SerializeField] private Color bonusColor = new Color(1f, 0.85f, 0.2f);

    [SerializeField] private string[] comboMessages = { "COMBO!", "ON FIRE!", "BLAZING!" };

    void OnEnable()
    {
        if (scoreManager == null) return;
        scoreManager.OnComboEngaged += HandleComboEngaged;
        scoreManager.OnStageBonusAwarded += HandleStageBonusAwarded;
    }

    void OnDisable()
    {
        if (scoreManager == null) return;
        scoreManager.OnComboEngaged -= HandleComboEngaged;
        scoreManager.OnStageBonusAwarded -= HandleStageBonusAwarded;
    }

    void HandleComboEngaged()
    {
        string message = comboMessages[Random.Range(0, comboMessages.Length)];
        Spawn(message, comboColor, comboSpawnPoint);
    }

    void HandleStageBonusAwarded(int amount)
    {
        Spawn("+" + amount, bonusColor, bonusSpawnPoint);
    }

    void Spawn(string text, Color color, RectTransform spawnPoint)
    {
        if (popupPrefab == null || spawnParent == null || spawnPoint == null) return;

        FloatingPopupText popup = Instantiate(popupPrefab, spawnParent);

        Vector2 localPoint;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, spawnPoint.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(spawnParent, screenPoint, null, out localPoint);

        popup.Play(text, color, localPoint);
    }
}

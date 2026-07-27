using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton_SoundManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private Button button;

    void Awake()
    {
        button = GetComponent<UnityEngine.UI.Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.PlayUI(SoundType.ButtonHover);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.PlayUI(SoundType.ButtonClick);
    }
}

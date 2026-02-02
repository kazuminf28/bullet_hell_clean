using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverColor : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    private Text buttonText;

    public Color normalColor;
    public Color hoverColor;

    void Awake()
    {
        // 非アクティブな子も含めて検索
        buttonText = GetComponentInChildren<Text>(true);

        if (buttonText == null)
        {
            Debug.LogError(
                "Text (Legacy) が Button の子に見つかりません: "
                + gameObject.name,
                this
            );
            return;
        }

        buttonText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = normalColor;
    }
}

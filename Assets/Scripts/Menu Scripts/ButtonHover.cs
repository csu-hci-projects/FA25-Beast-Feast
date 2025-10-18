using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private TextMeshProUGUI text;
    private Vector3 originalScale;
    private Color originalColor;

    public float hoverScale = 1.2f;
    public Color hoverColor = Color.white;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        originalScale = rectTransform.localScale;
        originalColor = text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.localScale = originalScale * hoverScale;
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.localScale = originalScale;
        text.color = originalColor;
    }
}
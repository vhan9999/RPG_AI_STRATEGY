using UnityEngine;
using UnityEngine.UI;

public class SquareButton : MonoBehaviour
{
    RectTransform rectTransform;
    void Start()
    {
        Button button = GetComponent<Button>();

        rectTransform = button.GetComponent<RectTransform>();
    }
    private void Update()
    {
        float height = rectTransform.rect.height;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height);
    }
}
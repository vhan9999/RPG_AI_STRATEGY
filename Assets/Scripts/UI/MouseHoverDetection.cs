using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHoverDetection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        Debug.Log("Mouse entered UI element.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        Debug.Log("Mouse exited UI element.");
    }

    public bool IsMouseOver
    {
        get { return isMouseOver; }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PindahPopUp : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public RectTransform dragRectTransform;
    public Canvas canvas;

   
    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransform.SetAsLastSibling();
    }

}

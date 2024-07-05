using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LastPopUp : MonoBehaviour, IPointerDownHandler
{
    public RectTransform popUpWindows;

    public void OnPointerDown(PointerEventData eventData)
    {
        popUpWindows.SetAsLastSibling();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadialButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Image buttonbg;
    public Image icon;
    public string title;
    public RadialMenuScript myMenu;

    Color defaultColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        myMenu.selected = this;
        defaultColor = icon.color;
        icon.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myMenu.selected = null;
        icon.color = defaultColor;
    }
}

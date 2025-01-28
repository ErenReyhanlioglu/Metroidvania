using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CharmButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CharmPanelUI charmPanel;
    public CharmBase charmBase;

    public void OnPointerClick(PointerEventData eventData)
    {
        charmPanel.OnTabSelected(charmBase, gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        charmPanel.OnTabEnter(charmBase, gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        charmPanel.OnTabExit(charmBase, gameObject);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; 

public class TabButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TabGroup tabGroup;
    public MeshRenderer background; 
    public TextMeshPro label; 

    private void Start()
    {
        tabGroup.Subscribe(this);
        if (background == null)
            background = GetComponent<MeshRenderer>();
        if (label == null)
            label = GetComponentInChildren<TextMeshPro>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    public void SetColor(Color color)
    {
        if (background != null)
        {
            background.material.color = color;
        }
    }

    public void SetLabelColor(Color color)
    {
        if (label != null)
        {
            label.color = color;
        }
    }
}

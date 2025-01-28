using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButtonUI> tabButtons; 
    public List<GameObject> tabPanels; 

    public Color tabIdle = Color.gray; 
    public Color tabActive = Color.white; 
    public Color tabHover = Color.green; 

    public TabButtonUI tabSelected; // Aktif olan buton

    public void Subscribe(TabButtonUI tabButton)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButtonUI>();
        }

        tabButtons.Add(tabButton);
    }

    public void OnTabEnter(TabButtonUI tabButton)
    {
        ResetTabs();

        if (tabSelected == null || tabSelected != tabButton)
        {
            tabButton.SetColor(tabHover);
        }
    }

    public void OnTabExit(TabButtonUI tabButton)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtonUI tabButton)
    {
        tabSelected = tabButton;
        ResetTabs();
        tabButton.SetColor(tabActive);

        int index = tabButton.transform.GetSiblingIndex();

        for (int i = 0; i < tabPanels.Count; i++)
        {
            if (index == i)
                tabPanels[i].SetActive(true);
            else
                tabPanels[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        foreach (TabButtonUI tabButton in tabButtons)
        {
            if (tabButton == tabSelected && tabSelected != null)
                continue;

            tabButton.SetColor(tabIdle);
        }
    }
}

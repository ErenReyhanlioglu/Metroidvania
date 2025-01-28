using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class CharmPanelUI : MonoBehaviour
{
    public TextMeshProUGUI charmDescription;
    public TextMeshProUGUI charmCost;

    public Color charmIdle = Color.gray;
    public Color charmActive = Color.white;
    public Color charmHover = Color.green;

    public List<GameObject> charms = new List<GameObject>();
    public CharmBase selectedCharm;

    private void OnEnable()
    {
        CharmManager.OnCharmDiscovered += UpdateCharmPanel;
        UpdateCharmPanel();
    }

    private void OnDisable()
    {
        CharmManager.OnCharmDiscovered -= UpdateCharmPanel;
    }

    private void UpdateCharmPanel(CharmBase charm)
    {
        foreach (var _charm in charms)
        {
            if (_charm.gameObject.name == charm.charmName)
            {
                Image charmImage = _charm.GetComponent<Image>();
                if (charmImage != null)
                {
                    charmImage.color = Color.white; 
                }
            }
        }
    }

    private void UpdateCharmPanel()
    {
        foreach (var _charm in charms)
        {
            if (CharmManager.discoveredCharms.Any(x => x.charmName == _charm.name))
            {
                Image charmImage = _charm.GetComponent<Image>();
                if (charmImage != null)
                {
                    if(CharmManager.activeCharms.Any(x => x.charmName == _charm.name))
                        charmImage.color = Color.green;
                    else
                        charmImage.color = Color.white;
                }
            }
        }
    }

    public void OnTabSelected(CharmBase senderCharm, GameObject charmUI)
    {
        selectedCharm = senderCharm;

        if (selectedCharm == null) return;

        bool control = CharmManager.activeCharms.Contains(selectedCharm)
            ? CharmManager.DeactivateCharm(selectedCharm)
            : CharmManager.ActivateCharm(selectedCharm);

        if (control && charmUI != null)
        {
            Image charmImage = charmUI.GetComponent<Image>();
            if (charmImage != null)
            {
                charmImage.color = CharmManager.activeCharms.Contains(selectedCharm) ? Color.green : Color.white;
            }
        }
    }


    public void OnTabEnter(CharmBase senderCharm, GameObject charmUI)
    {
        ResetTabs();

        selectedCharm = senderCharm;

        if(selectedCharm != null && CharmManager.discoveredCharms.Contains(selectedCharm))
        {
            charmDescription.text = selectedCharm.description;
            charmCost.text = selectedCharm.cost.ToString();
        }
    }

    public void OnTabExit(CharmBase senderCharm, GameObject charmUI)
    {
        ResetTabs();
    }

    private void ResetTabs()
    {
        foreach (GameObject _charm in charms)
        {
            Image charmImage = _charm.GetComponent<Image>();
            if (charmImage != null && !CharmManager.discoveredCharms.Any(x => x.charmName == _charm.name))
            {
                charmImage.color = Color.black;
            }
        }
    }
}

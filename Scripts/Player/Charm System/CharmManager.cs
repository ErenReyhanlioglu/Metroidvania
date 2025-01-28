using System.Collections.Generic;
using UnityEngine;
using System;

public class CharmManager : MonoBehaviour
{
    [Header("CharmData")]
    public CharmDataBase allCharmDataBase;

    [Header("Player")]
    public GameObject player;

    public static List<CharmBase> allCharms; 
    public static List<CharmBase> discoveredCharms;
    public static List<CharmBase> activeCharms;

    public static event Action<CharmBase> OnCharmDiscovered;

    private void Start()
    {
        allCharms = new List<CharmBase>();
        discoveredCharms = new List<CharmBase>();
        activeCharms = new List<CharmBase>();

        FillAllCharmDatas();
        InitializeAllCharms();
    }

    private void FillAllCharmDatas()
    {
        foreach (var charm in allCharmDataBase.allCharmList)
        {
            allCharms.Add(charm);
        }
    }

    private void InitializeAllCharms()
    {
        foreach (var charm in allCharms)
        {
            charm.OnInitializeCharm(player);
            charm.isCharmActive = false;
            charm.isCharmDiscovered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Charm")
        {
            CharmObject charmObject = collision.gameObject.GetComponent<CharmObject>();

            if (charmObject != null && charmObject.charm != null)
            {
                DiscoverCharm(charmObject.charm);
                Destroy(collision.gameObject);
            }
        }
    }

    public static bool DiscoverCharm(CharmBase charm)
    {
        if (!allCharms.Contains(charm))
            return false;

        if (!charm.isCharmDiscovered)
        {
            charm.DiscoverCharm();
            discoveredCharms.Add(charm);

            OnCharmDiscovered?.Invoke(charm);
        }

        return true;
    }

    public static bool ActivateCharm(CharmBase charm)
    {
        if (!charm.isCharmDiscovered)
            return false;

        if (!charm.isCharmActive)
        {
            charm.ActivateCharm();
            activeCharms.Add(charm);
        }

        return true;
    }

    public static bool DeactivateCharm(CharmBase charm)
    {
        if (!charm.isCharmDiscovered)
            return false;

        if (charm.isCharmActive)
        {
            charm.DeactivateCharm();
            activeCharms.Remove(charm);
        }

        return true;
    }

    public void DeactivateAllCharms()
    {
        foreach (var charm in activeCharms)
        {
            charm.DeactivateCharm();
            charm.isCharmActive = false;
        }

        activeCharms.Clear();
    }
}



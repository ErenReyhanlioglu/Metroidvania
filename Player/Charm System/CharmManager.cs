using System.Collections.Generic;
using UnityEngine;

public class CharmManager : MonoBehaviour
{
    public List<CharmBase> allCharms = new List<CharmBase>();
    public List<CharmBase> discoveredCharms = new List<CharmBase>(); 
    public List<CharmBase> activeCharms = new List<CharmBase>();
    public GameObject Player;

    private void Start()
    {
        InitializeAllCharms();
    }

    public void InitializeAllCharms()
    {
        foreach (var charm in allCharms)
        {
            charm.OnInitializeCharm(Player);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Charm"))
        {
            CharmObject charmObject = collision.gameObject.GetComponent<CharmObject>();

            if (charmObject != null && charmObject.charm != null)
            {
                DiscoverCharm(charmObject.charm); 
                ActivateCharm(charmObject.charm); 
                Destroy(collision.gameObject); 
            }
        }
    }

    public void DiscoverCharm(CharmBase charm)
    {
        if (!allCharms.Contains(charm))
            return;

        if (IsCharmDiscovered(charm))
        {
            discoveredCharms.Add(charm);
            charm.isCharmDiscovered = true;
        }
    }

    public void ActivateCharm(CharmBase charm)
    {
        if (IsCharmDiscovered(charm))
        {
            return;
        }

        if (!activeCharms.Contains(charm))
        {
            charm.ActivateCharm();
            charm.isCharmActive = true;
            activeCharms.Add(charm);
        }
    }

    public void DeactivateCharm(CharmBase charm)
    {
        if (IsCharmActive(charm))
        {
            charm.DeactivateCharm();
            charm.isCharmActive = false;
            activeCharms.Remove(charm);
        }
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

    public bool IsCharmDiscovered(CharmBase charm)
    {
        return discoveredCharms.Contains(charm);
    }

    public bool IsCharmActive(CharmBase charm)
    {
        return activeCharms.Contains(charm);
    }
}



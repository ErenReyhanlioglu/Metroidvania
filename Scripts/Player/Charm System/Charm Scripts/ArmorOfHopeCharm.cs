using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorOfHopeCharm", menuName = "Charms/Armor of Hope")]
public class ArmorOfHopeCharm : CharmBase
{
    public override bool OnInitializeCharm(GameObject _player)
    {
        return true;
    }

    public override bool DiscoverCharm()
    {
        isCharmDiscovered = true;
        return true;
    }

    public override bool ActivateCharm()
    {
        return true;
    }

    public override bool DeactivateCharm()
    {
        return true;
    }
}

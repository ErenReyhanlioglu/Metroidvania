using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuidingLightCharm", menuName = "Charms/Guiding Light")]
public class GuidingLightCharm : CharmBase
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

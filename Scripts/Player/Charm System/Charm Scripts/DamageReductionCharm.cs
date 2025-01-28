using UnityEngine;

[CreateAssetMenu(fileName = "DamageReductionCharm", menuName = "Charms/DamageReductionCharm")]
public class DamageReductionCharm : CharmBase
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

using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoostCharm", menuName = "Charms/SpeedBoostCharm")]
public class SpeedBoostCharm : CharmBase
{
    PlayerMovement playerMovement;
    public float speedIncrease; 

    public override void DiscoverCharm()
    {
        isCharmDiscovered = true;
    }

    public override void OnInitializeCharm(GameObject Player)
    {
        playerMovement = Player.GetComponent<PlayerMovement>();
    }

    public override void ActivateCharm()
    {
        isCharmActive = true;

        playerMovement.moveSpeed += speedIncrease;
    }

    public override void DeactivateCharm()
    {
        isCharmActive = false;

        playerMovement.moveSpeed -= speedIncrease;
    }
}

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoostCharm", menuName = "Charms/Speed Boost")]
public class SpeedBoostCharm : CharmBase
{
    PlayerMovement playerMovement;
    public float speedIncrease; 

    public override bool DiscoverCharm()
    {
        isCharmDiscovered = true;
        return true;
    }

    public override bool OnInitializeCharm(GameObject _player)
    {
        try
        {
            playerMovement = _player.GetComponent<PlayerMovement>();
            return playerMovement != null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in OnInitializeCharm: {ex.Message}");
            return false;
        }
    }

    public override bool ActivateCharm()
    {
        isCharmActive = true;

        playerMovement.moveSpeed += speedIncrease;

        return playerMovement != null;
    }

    public override bool DeactivateCharm()
    {
        isCharmActive = false;

        playerMovement.moveSpeed -= speedIncrease;

        return playerMovement != null;
    }
}

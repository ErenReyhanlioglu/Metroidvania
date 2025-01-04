using UnityEngine;

[CreateAssetMenu(fileName = "DamageReductionCharm", menuName = "Charms/DamageReductionCharm")]
public class DamageReductionCharm : CharmBase
{
    public float damageReductionPercentage; 

    private void OnEnable()
    {
        triggerType = TriggerType.OnUpdate; 
    }

    public override void OnInitializeCharm(GameObject Player)
    {
        
    }

    public override void DiscoverCharm()
    {
        isCharmDiscovered = true;
    }

    public override void ActivateCharm()
    {
        //player.OnTakeDamage += ReduceDamage; // Hasar olay�na ba�lanma
    }

    public override void DeactivateCharm()
    {
        //player.OnTakeDamage -= ReduceDamage; // Hasar olay�ndan ��kma
    }

    private void ReduceDamage(ref float damage)
    {
        damage -= damage * damageReductionPercentage; 
    }
}

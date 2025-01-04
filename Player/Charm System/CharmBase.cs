using UnityEngine;

public abstract class CharmBase : ScriptableObject
{
    public string charmName; 
    public string description;
    public bool isCharmDiscovered = false;
    public bool isCharmActive = false;
    public int cost; 
    public Sprite icon;
    public TriggerType triggerType { get; protected set; }

    public abstract void DiscoverCharm();

    public abstract void ActivateCharm();

    public abstract void DeactivateCharm();

    public abstract void OnInitializeCharm(GameObject Player);
}

public enum TriggerType
{
    OnJump,
    OnUpdate,
    Every10Seconds,
    // ...
}
using UnityEngine;

public abstract class CharmBase : ScriptableObject
{
    public string charmName; 
    public string description;
    public bool isCharmDiscovered;
    public bool isCharmActive;
    public int cost; 
    public Sprite icon;
    public TriggerType triggerType;

    public abstract bool DiscoverCharm(); 

    public abstract bool ActivateCharm();

    public abstract bool DeactivateCharm();

    public abstract bool OnInitializeCharm(GameObject _player);
}

public enum TriggerType
{
    OnJump,
    OnUpdate,
    Every10Seconds,
    // ...
}
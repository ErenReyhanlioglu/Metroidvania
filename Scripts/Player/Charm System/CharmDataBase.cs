using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharmDataBase", menuName = "Charms/Charm Data Base")]
public class CharmDataBase : ScriptableObject
{
    public List<CharmBase> allCharmList = new List<CharmBase>();
}

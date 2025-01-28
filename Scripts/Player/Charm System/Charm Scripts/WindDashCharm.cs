using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WindDashCharm", menuName = "Charms/Wind Dash")]
public class WindDashCharm : CharmBase
{
    [Range(0f, 1f)]
    [Header("Dash cool down reduce amount(%)")]
    public float coolDownReduce;

    public List<CharacterEmotion> characterEmotions;

    public override bool OnInitializeCharm(GameObject _player)
    {
        return characterEmotions.Count == 3;
    }

    public override bool DiscoverCharm()
    {
        if (characterEmotions.Count != 3)
            return false;

        isCharmDiscovered = true;
        return true;
    }

    public override bool ActivateCharm()
    {
        if (characterEmotions.Count != 3)
            return false;

        foreach (var emotion in characterEmotions)
        {
            emotion.customInsObjEmotions.dashCoolDown *= (1 - coolDownReduce); // duygular scriptable object oldugu i�in oyun charm aktifken kapat�ld�g�nda cooldown o de�erde(eksik) kal�yor. 
        }

        isCharmActive = true;
        return true;
    }

    public override bool DeactivateCharm()
    {
        if (characterEmotions.Count != 3)
            return false;

        foreach (var emotion in characterEmotions)
        {
            emotion.customInsObjEmotions.dashCoolDown /= (1 - coolDownReduce);
        }

        isCharmActive = false;
        return true;
    }
}

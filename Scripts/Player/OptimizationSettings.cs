using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizationSettings : MonoBehaviour
{
    int targetFrameRate = 144;
    
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject escapePanel; 

    private bool isPanelActive = false; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePanel();
        }
    }

    private void TogglePanel()
    {
        isPanelActive = !isPanelActive;
        
        if (escapePanel != null)
        {
            escapePanel.SetActive(isPanelActive);
        }
    }
}

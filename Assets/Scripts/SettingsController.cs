using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public GameObject settingsPanel;
    public TextMeshProUGUI settingsText;
    private int buttonClicks;
    public void ToggleSettings()
    {
        buttonClicks++;
        if (buttonClicks % 2 == 1)
        {
            settingsPanel.SetActive(true);
            settingsText.SetText("HIDE");
        }
        else
        {
            settingsPanel.SetActive(false);
            settingsText.SetText("SETTINGS");
        }
    }
}

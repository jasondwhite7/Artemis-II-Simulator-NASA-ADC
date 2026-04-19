using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MarioMode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject background;
    private int buttonClicks;
    public Image checkmark;
    public TextMeshProUGUI marioTitle;
    public TextMeshProUGUI marioDescription;
    public Image marioPicture;
    public GameObject marioToggle;

    private Color green = new Color(0.04463333f, 0.5566038f, 0.04463333f);
    
    void Start()
    {
        background.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        background.SetActive(false);
    }
    public bool ToggleMarioMode()
    {
        buttonClicks++;
        if (buttonClicks % 2 == 1)
        {   
            checkmark.color = green;
            return true;
        }    
        else
        {
            checkmark.color = Color.black;
            return false;
        }
    } 
    public void DiscoverMarioMode()
    {
        marioTitle.SetText("Mario Mode");
        marioDescription.SetText("When selected, if the resultant velocity " + 
        "mode is on and simulation speed is set to max, Orion will turn into Bullet Bill " +
        "traveling along Rainbow Road!");
        Color color = marioPicture.color;
        color.a = 1;
        marioPicture.color = color;
        marioToggle.SetActive(true);
    }
}


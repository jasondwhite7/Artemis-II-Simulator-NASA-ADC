using System;
using System.Collections;
using System.Collections.Generic;
using CsvHelper;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ColorKey : MonoBehaviour
{
    //all toggles, color keys, and lines for each setting
    [Header("Resultant Velocity")]
    public GameObject resultantVelocityToggle;
    public GameObject resultantVelocityColorKey;
    public GameObject velocityLines;
    public GameObject bigVelocityLines;
    [Header("Mission Phase")]
    public GameObject missionPhaseToggle;
    public GameObject missionPhaseColorKey;
    public GameObject missionLines;
    public GameObject bigMissionLines;
    [Header("Nominal Antenna")]
    public GameObject antennaAvailibityColorKey;
    public GameObject antennaLines;
    public GameObject bigAntennaLines;
    public GameObject nominalToggle;

    [Header("Off Nominal Antenna")]
    public GameObject offNominalToggle;
    public GameObject offNominalLines;
    public GameObject bigOffNominalLines;
    [Header("Dynamic")]
    public GameObject dynamicToggle;
    public GameObject dynamicColorKey;
    public GameObject dynamicLines;
    public GameObject bigDynamicLines;
    [Header("Trail")]
    public GameObject trailToggle;
    public GameObject trailColorKey;
    public GameObject trailLines;
    public GameObject bigTrailLines; 
    [Header("Other Buttons")]
    public GameObject collapseToggle;
    public GameObject pageToggle;
    //game objects which contain all info on first or second page
    public GameObject firstPage;
    public GameObject secondPage;
    //arrow images
    public GameObject nextArrow;
    public GameObject backArrow;
    //collapse button text to change it when it gets collapsed
    public TextMeshProUGUI buttonText;
    //rect transforms of the panel and collapse button
    private RectTransform panelTransform;  
    private RectTransform collapseTransform;
    //int to know whether to collapse or expand
    private int collapseClicks = 0;
    private int pageClicks = 0;
    //initial position and scale of the panel and collapse button
    Vector2 normalPanel;
    Vector2 normalCollapsePosition;
    Vector2 normalCollapseSize;
    //delgate to know which function was last ran so when they open it up again the correct color key is shown
    private delegate void FunctionDelegate();
    private FunctionDelegate LastFunctionRan; 
    public void Start()
    {
      //grab the rect transforms of the panel and the collapse button
      panelTransform = GetComponent<RectTransform>();
      collapseTransform = collapseToggle.GetComponent<RectTransform>();
      //grab the initial position and scaling and such to be used later
      normalPanel = new Vector2(panelTransform.sizeDelta.x, panelTransform.sizeDelta.y);
      normalCollapsePosition = new Vector2(collapseTransform.anchoredPosition.x, collapseTransform.anchoredPosition.y);
      normalCollapseSize = new Vector2(collapseTransform.sizeDelta.x, collapseTransform.sizeDelta.y);
    }
    //Functions that sets other color keys and toggles to false so only the wanted one is 
    //showing and when the desired color key toggle is clicked it appears
    public void ShowResultantVelocityColorKey()
    {
      //i believe this is pretty self explanatory, but they all turn off everything then turn on what they need
      TurnOffColorKeys();
      TurnOffLines();
      resultantVelocityColorKey.SetActive(true);    
      velocityLines.SetActive(true);
      bigVelocityLines.SetActive(true);
      LastFunctionRan = ShowResultantVelocityColorKey;
    }
    public void ShowMissionPhaseColorKey()
    {
      //turn on normal toggles so when back button is hit the correct toggles show
      TurnOffColorKeys();
      TurnOffLines();
      missionPhaseColorKey.SetActive(true); 
      missionLines.SetActive(true);
      bigMissionLines.SetActive(true);
      LastFunctionRan = ShowMissionPhaseColorKey;
    }
    public void ShowAntennaColorKey()
    {
      TurnOffColorKeys();
      TurnOffLines();
      antennaAvailibityColorKey.SetActive(true);
      antennaLines.SetActive(true);
      bigAntennaLines.SetActive(true);
      LastFunctionRan = ShowAntennaColorKey;
    }
    public void ShowOffNominal()
    {
      TurnOffColorKeys();
      TurnOffLines();
      antennaAvailibityColorKey.SetActive(true);
      offNominalLines.SetActive(true);
      bigOffNominalLines.SetActive(true);
      LastFunctionRan = ShowOffNominal;
    }
    public void ShowDynamic()
    {
      TurnOffColorKeys();
      TurnOffLines();
      dynamicColorKey.SetActive(true);
      dynamicLines.SetActive(true);
      bigDynamicLines.SetActive(true);
      LastFunctionRan = ShowDynamic;
    }
    public void ShowTrail()
    {
      TurnOffColorKeys();
      TurnOffLines();
      trailColorKey.SetActive(true);
      trailLines.SetActive(true);
      bigTrailLines.SetActive(true);
      LastFunctionRan = ShowTrail;
    }
    public void SwitchPage()
    {
      pageClicks++;
      if (pageClicks % 2 == 1)
      {
        secondPage.SetActive(true);
        firstPage.SetActive(false);
        TurnOffToggles();
        TurnOnSecondToggles();
        nextArrow.SetActive(false);
        backArrow.SetActive(true);
      }
      else
      {
        firstPage.SetActive(true); 
        secondPage.SetActive(false);
        TurnOffToggles();
        TurnOnFirstToggles();
        nextArrow.SetActive(true);
        backArrow.SetActive(false);
      }
    }
    public void Collaspe()
    {
      collapseClicks++;
      //if there is a remainder, close the panel since they have hit it 1, 3, etc times
      if (collapseClicks % 2 == 1)
      {
        //turn off everything
        TurnOffColorKeys();
        TurnOffToggles();
        //set the sizes of the panel and button to these numbers i messed around with
        panelTransform.sizeDelta = new Vector2(145, 60);
        collapseTransform.anchoredPosition = new Vector2(-2.9f, 0);
        collapseTransform.sizeDelta = new Vector2(130, 46.368f);
        //set text to color keys so when its hidden it doesnt say hide
        buttonText.text = "COLOR KEYS";
        }
      //if no remainder they want to open it back up since they have hit it 2, 4, etc times 
        else 
        {
        //run the last function called, should pull up the correct lines and color keys
        if (LastFunctionRan == null)
        {
          ShowMissionPhaseColorKey();
          TurnOnFirstToggles();
        }
        else
        {
          LastFunctionRan.Invoke();
          if (pageClicks % 2 == 1)
          {
            TurnOnSecondToggles();
          }
          else
          {
            TurnOnFirstToggles();
          }
        }     
        //set the size of panel and button back to normal
        panelTransform.sizeDelta = normalPanel;
        collapseTransform.anchoredPosition = normalCollapsePosition;
        collapseTransform.sizeDelta = normalCollapseSize;
        //set the size of panel and button back to normal
        buttonText.text = "HIDE";
        }
    }
    //i hope all of these functions are self explanatory
    public void TurnOffColorKeys()
    {
      antennaAvailibityColorKey.SetActive(false);
      missionPhaseColorKey.SetActive(false);
      resultantVelocityColorKey.SetActive(false);
      dynamicColorKey.SetActive(false);
      trailColorKey.SetActive(false);
    }
    public void TurnOffLines()
    {
      missionLines.SetActive(false);
      antennaLines.SetActive(false);
      velocityLines.SetActive(false);
      offNominalLines.SetActive(false);
      dynamicLines.SetActive(false);
      trailLines.SetActive(false);
      bigMissionLines.SetActive(false);
      bigAntennaLines.SetActive(false);
      bigVelocityLines.SetActive(false);
      bigOffNominalLines.SetActive(false);
      bigDynamicLines.SetActive(false);
      bigTrailLines.SetActive(false);
    }

    public void TurnOffToggles()
    {
      missionPhaseToggle.SetActive(false);
      resultantVelocityToggle.SetActive(false);
      dynamicToggle.SetActive(false);
      trailToggle.SetActive(false);
      nominalToggle.SetActive(false);
      offNominalToggle.SetActive(false);
    }
    public void TurnOnFirstToggles()
    {
      resultantVelocityToggle.SetActive(true);
      missionPhaseToggle.SetActive(true);
      dynamicToggle.SetActive(true);
    }
    public void TurnOnSecondToggles()
    {
      nominalToggle.SetActive(true);
      offNominalToggle.SetActive(true);
      trailToggle.SetActive(true);
    }
    public bool IsMarioMode()
    {
      if (LastFunctionRan == ShowResultantVelocityColorKey) 
      {
        return true;
      }
      else
      {
        return false;
      }
    }
}


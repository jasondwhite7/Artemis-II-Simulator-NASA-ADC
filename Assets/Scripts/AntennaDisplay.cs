using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AntennaDisplay : MonoBehaviour
{
    // game objects for the percentage bars
    [Header("Battery Bar Images")]
    public Image WPSABar;
    public Image DS24Bar;
    public Image DS34Bar;
    public Image DS54Bar;
    [Header("Antenna Game Objects")]
    // game objects for the antenna sprites
    public GameObject WPSAAntenna;
    public GameObject DS24Antenna;
    public GameObject DS34Antenna;
    public GameObject DS54Antenna;
    [Header("Link Budget and Time Texts")]
    // text meshes for the link budgets and availability times
    public TextMeshProUGUI WPSAValue;
    public TextMeshProUGUI DS24Value;
    public TextMeshProUGUI DS34Value;
    public TextMeshProUGUI DS54Value;
    public TextMeshProUGUI WPSATime;
    public TextMeshProUGUI DS24Time;
    public TextMeshProUGUI DS34Time;
    public TextMeshProUGUI DS54Time;
    [Header("Green Antenna Images")]
    //green antenna images
    public Image WPSAGreenAntenna;
    public Image DS54GreenAntenna;
    public Image DS24GreenAntenna;
    public Image DS34GreenAntenna;
    //rect transforms for each bar
    private RectTransform WPSARect;
    private RectTransform DS54Rect;
    private RectTransform DS24Rect;
    private RectTransform DS34Rect;
    [Header("Green Line Connected Game Objects")]
    //connected green line gameobjects
    public GameObject WPSAConnect;
    public GameObject DS54Connect;
    public GameObject DS24Connect;
    public GameObject DS34Connect;
    [Header("Toggles")]
    //toggles
    public GameObject SwitchesToggle;
    public GameObject LinkBudgetToggle;
    public GameObject BalanceToggle;
    //height of the rectangles
    private float height = 5.074f;
    string connectedAntenna;
    string[] antennas = {
        "WPSA Antenna",
        "DS54 Antenna",
        "DS24 Antenna",
        "DS34 Antenna"
    };
    // initialise all the game objects and load sprites
    void Start()
    {
        // set the names because it's weird I dunno
        WPSAAntenna.name = "WPSA Antenna";
        DS24Antenna.name = "DS24 Antenna";
        DS34Antenna.name = "DS34 Antenna";
        DS54Antenna.name = "DS54 Antenna";
        //grab the rect transforms of each bar
        WPSARect = WPSABar.GetComponent<RectTransform>();
        DS54Rect = DS54Bar.GetComponent<RectTransform>();
        DS24Rect = DS24Bar.GetComponent<RectTransform>();
        DS34Rect = DS34Bar.GetComponent<RectTransform>();
    }

    public void ChangeBar(RectTransform bar, GameObject antenna, float linkBudget, Image GreenAntenna) {
        //Set the transparency of the green antenna to 0-1 based on link budget 
        Color color = GreenAntenna.color;
        color.a = linkBudget / 10000;
        GreenAntenna.color = color;
        // checks if link budget is not 0, aka antenna is visible
        if (linkBudget != 0) {
            //Set the width to a number 0-10 since that is the width of the rectangles based on link budget
            bar.sizeDelta = new Vector2(linkBudget / 1000, height);

            if (antenna.name == connectedAntenna)
            {
                //Set the green lines on whichever antenna is the connected one
                if (antenna.name == "WPSA Antenna")
                    WPSAConnect.SetActive(true);
                else if (antenna.name == "DS54 Antenna")
                    DS54Connect.SetActive(true);
                else if (antenna.name == "DS24 Antenna")
                    DS24Connect.SetActive(true);
                else if (antenna.name == "DS34 Antenna")
                    DS34Connect.SetActive(true);
            }
        } else {
            // if link budget is 0 set width to 0
            bar.sizeDelta = new Vector2(0, 5.074f);
        }
    }
    // message receiver functions
    public void WPSADisplay(float linkBudget) {
        //Turn them all the green connected lines off first since this one runs first
        TurnOffConnectedDisplays();
        ChangeBar(WPSARect, WPSAAntenna, linkBudget, WPSAGreenAntenna);
        WPSAValue.text = $"WPSA\n{(linkBudget < 10000 ? linkBudget : 10000)}";
    }
    public void DS24Display(float linkBudget) {
        ChangeBar(DS24Rect, DS24Antenna, linkBudget, DS24GreenAntenna);
        DS24Value.text = $"DS24\n{(linkBudget < 10000 ? linkBudget : 10000)}";
    }
    public void DS34Display(float linkBudget) {
        ChangeBar(DS34Rect, DS34Antenna, linkBudget, DS34GreenAntenna);
        DS34Value.text = $"DS34\n{(linkBudget < 10000 ? linkBudget : 10000)}";
    }
    public void DS54Display(float linkBudget) {
        ChangeBar(DS54Rect, DS54Antenna, linkBudget, DS54GreenAntenna);
        DS54Value.text = $"DS54\n{(linkBudget < 10000 ? linkBudget : 10000)}";
    }

    // funciton for telling the display which antenna is picked, using antennas array
    public void AntennaSwitch(int pick) {
        connectedAntenna = antennas[pick];
    }

    public void AvailabilityTimes(List<float> times) {
        WPSATime.text = $"{times[0]}\nMINUTES REMAINING";
        DS54Time.text = $"{times[1]}\nMINUTES REMAINING";
        DS24Time.text = $"{times[2]}\nMINUTES REMAINING";
        DS34Time.text = $"{times[3]}\nMINUTES REMAINING";
    }
    public void TurnOffConnectedDisplays()
    {
        //turn all the green lines off!
        WPSAConnect.SetActive(false);
        DS54Connect.SetActive(false);
        DS24Connect.SetActive(false);
        DS34Connect.SetActive(false);
    }
}

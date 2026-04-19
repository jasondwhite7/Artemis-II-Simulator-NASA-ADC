using System;
using UnityEngine;
using TMPro;
using System.Linq.Expressions; //added for TextMeshProUGUI

public class DisplayVelocityText : MonoBehaviour
{
  //text for each velocity
   public TextMeshProUGUI vX;
   public TextMeshProUGUI vY;
   public TextMeshProUGUI vZ;
   public TextMeshProUGUI vR;
   //hide button
   public GameObject collapseToggle;
   //everything but the button, will explain later
   public GameObject everything;
   //int to know to hide or expand
   private int collapseClicks;
   //rect transforms for panel and button
   private RectTransform panelTransform;  
   private RectTransform collapseTransform;
   //text to change based on if it is hidden or not
   public TextMeshProUGUI buttonText;
   //initial position and scale of the panel and collapse button
   Vector2 normalPanelSize;
   Vector2 normalPanelPosition;
   Vector2 normalCollapse;
   //velocities on bro
   private float Vx;
   private float Vy;
   private float Vz;
   private float Vr;
    public void Start()
    {
      //grab the rect transforms
      panelTransform = GetComponent<RectTransform>();
      collapseTransform = collapseToggle.GetComponent<RectTransform>();
      //grab the initial posotions and sizes of them
      normalPanelSize = new Vector2(panelTransform.sizeDelta.x, panelTransform.sizeDelta.y);
      normalPanelPosition = new Vector2(panelTransform.localPosition.x, panelTransform.localPosition.y);
      normalCollapse = new Vector2(collapseTransform.anchoredPosition.x, collapseTransform.anchoredPosition.y);
    }

    public void DisplayVelocity(TimeSplice timeSplice)
    {
      //give values to each velocity
      Vx = timeSplice.RoundedVx;
      Vy = timeSplice.RoundedVy;
      Vz = timeSplice.RoundedVz;
      Vr = timeSplice.RoundedResultantVelocity;
      //display them boys
      vX.text = $"X: {Convert.ToString(Vx)}";
      vY.text = $"Y: {Convert.ToString(Vy)}";
      vZ.text = $"Z: {Convert.ToString(Vz)}";
      vR.text = $"RESULTANT: {Convert.ToString(Vr)}";
    }
    public void VelocityCollaspe()
    {
    collapseClicks++;
    //if there is a remainder, close the panel since they have hit it 1, 3, etc times
    if (collapseClicks % 2 == 1)
    {
      //turn everything off but the button and panel!
      everything.SetActive(false);
      //set the sizes of the panel and button to these numbers i messed around with
      panelTransform.sizeDelta = new Vector2(413.51f, 200);
      panelTransform.localPosition = new Vector2(-890, -243.8f);
      collapseTransform.anchoredPosition = new Vector2(-2.9f, 0);
      //set text to velocity display so when its hidden it doesnt say hide
      buttonText.text = "VELOCITY DISPLAY";
    }
    else
    {
      //turn it all back on
      everything.SetActive(true);
      //set the size of panel and button back to normal
      panelTransform.sizeDelta = normalPanelSize;
      panelTransform.localPosition = normalPanelPosition;
      collapseTransform.anchoredPosition = normalCollapse;
      //set the size of panel and button back to normal
      buttonText.text = "HIDE";
    }
    }
}



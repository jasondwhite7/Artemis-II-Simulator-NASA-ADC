using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class TotalDistance : MonoBehaviour
{
   //total distance text
   public TextMeshProUGUI totalDistance;
   public void DisplayTotalDistance(float distance)
   {
      //DISPLAY IT!!!!!
      totalDistance.text = $"DISTANCE TRAVELED: {Convert.ToString(distance)} KM";
   }
}

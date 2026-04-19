using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class MoonPhase : MonoBehaviour
{
    //the light!
    public Light moonlight;
    //both of these to be recieved by the timer
    private float SimSpeed = 1;
    private bool timerIsActive;
    public void RecieveSimSpeed(float speed)
    {
        //recieve the sim speed when it gets changed
        SimSpeed = speed;
    }
    private void Update()
    {
        if (timerIsActive)
        {
            //rotate the light, that number is in degrees / second
            moonlight.transform.Rotate(0, 0.0001411f * SimSpeed * Time.deltaTime, 0);
        }    
    }  
    public void StopTimer(bool timerStatus)
    {
        //recieve the timer status from the timer
        timerIsActive = timerStatus;
    }
    public void UpdateMoonPhase(float time)
    {
        //hard lock the rotation in case the update function sucks based on the current time, the number is degrees / minute
        //254.7 is the initial rotation to get the correct starting moon phase
        moonlight.transform.rotation = Quaternion.Euler(0, 254.7f + 0.008466f * time, 0);
    }
}

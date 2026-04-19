using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//update cares about sim speed not function and needs time.DeltaTime
//function - rotate along y axis, multiplied by the time in seconds
//capsule - 
public class EarthController : MonoBehaviour
{
    //both of these to be recieved by the timer
    private float SimSpeed = 1;
    private bool timerIsActive;
    private bool wasEarthMoved;
    public void RecieveSimSpeed(float speed)
    {
        //recieve the sim speed when it gets changed
        SimSpeed = speed;
    }
    private void Update()
    { 
        if (timerIsActive)
        {
            if (!wasEarthMoved)
            {
                //rotate the earth, that number is in degrees / second
            transform.Rotate(0, -0.004166667f * SimSpeed * Time.deltaTime, 0, Space.Self);
            }
            else
            {
                wasEarthMoved = false;
            }
            
        } 
    }
    public void StopTimer(bool timerStatus)
    {
        //recieve the timer status from the timer
        timerIsActive = timerStatus;
    }
    public void RotateEarth(float time)
    {
        //hard lock the rotation in case the update function sucks based on the current time, 0.25 is degrees / minute
        transform.rotation = Quaternion.Euler(90, 0, 0) * Quaternion.Euler(0, -0.25f * time - 108.051f, 0);
        wasEarthMoved = true;
    }
}

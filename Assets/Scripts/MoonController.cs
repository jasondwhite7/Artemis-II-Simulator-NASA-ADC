using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    //all of these to be recieved by the timer
    private float SimSpeed = 1;
    private bool timerIsActive;
    private Vector3 velocityVector;
    //earth rotation since i rotated everything for dr harmon
    private Quaternion earthTiltRotation = Quaternion.Euler(90, 0, 23.5f);
    private bool wasMoonMoved;
    public void RecieveSimSpeed(float speed)
    {
        //recieve the sim speed when it gets changed
        SimSpeed = speed;
    }
    public void Update()
    {
        if (timerIsActive)
        {
            if (!wasMoonMoved)
            {
                //move the moon in the direction of its velocity if sim speed is slow enough
            Vector3 movement = SimSpeed * Time.deltaTime * velocityVector;
            transform.Translate(movement, Space.World);
            }
            else 
            {
                wasMoonMoved = false;
            }

        }
    }
    public void RecieveVelocityVector(Vector3 Vector)
    {
        //recieve the current velocity from the timer and rotate it to be used above
        Vector3 rotatedVelocityVector = earthTiltRotation * Vector;
        velocityVector = rotatedVelocityVector;
    }
    public void StopTimer(bool timerStatus)
    {
        //recieve the timer status from the timer
        timerIsActive = timerStatus;
    }
    public void MoveMoon(Vector3 positionVector)
    {
        //set the position of the moon to the rotated position
        Vector3 rotatedVector = earthTiltRotation * positionVector;
        transform.position = rotatedVector;
        wasMoonMoved = true;
    }
}

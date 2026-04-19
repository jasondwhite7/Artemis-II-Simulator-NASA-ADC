using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CapsuleController : MonoBehaviour
{
    //lines affected before and after capsule
    // private float linesAffected = 200f;
    //both of these to be recieved from the timer
    private float SimSpeed;
    private bool timerIsActive;
    //for capsules internal update function
    private Vector3 velocityVector;
    private Vector3 rotatedLookAtVector;
    private Vector3 rotatedTargetVector;
    //these 4 are for changing the opacity of each line
    // private Transform Line;
    // private LineRenderer lr;
    // private Color color;
    // private Material material;
    //these are to know which line is turned on
    // public GameObject parent;
    // private GameObject[] parents;
    // //how many total lines there are
    // private int lineCount;
    //earth rotation since i rotated everything for dr harmon
    private Quaternion earthTiltRotation = Quaternion.Euler(90, 0, 23.5f);
    //
    private bool wasCapsuleMoved;
    // public void Start()
    // {
    //     //initiate the parents
    //     parents = new GameObject[]
    //     {
    //         parent.transform.Find("Mission").gameObject,
    //         parent.transform.Find("Antenna").gameObject,
    //         parent.transform.Find("Velocity").gameObject,
    //         parent.transform.Find("OffNominal").gameObject
    //     };
    //     //set lineCount
    //     lineCount = parents[0].transform.childCount;
    // }
    public void RecieveSimSpeed(float speed)
    {
        //recieve sim speed from timer when updated
        SimSpeed = speed;
    }
    public void RecieveVelocityVector(Vector3 vector)
    {     
        //recieve the velocity vector to be used for update function, and rotate it
        Vector3 rotatedCurrentVector = earthTiltRotation * vector;
        velocityVector = rotatedCurrentVector;
    }
    public void Update()
    {
        if (timerIsActive)
        {
            if (!wasCapsuleMoved)
            {
                // transform.LookAt(rotatedLookAtVector);
                // transform.Rotate(180, 0, 0);
                //translate the rocket to move to the next position vector in the correct amount of time if sim speed is low enough
                Vector3 movement = SimSpeed * Time.deltaTime * velocityVector;
                transform.Translate(movement, Space.World); 
            }
            else 
            {
                wasCapsuleMoved = false;
            }
        }
    }
    public void StopTimer(bool timerStatus)
    {
        //recieve timer status and stop it if its done so the update function stops going
        timerIsActive = timerStatus;
    }
    public void MoveCapsule(Vector3 targetVector, Vector3 lookAtVector)
    {
        //move the capsule to the next position vector and look at the one after that
        //rotate it for earths rotation, and 180 to make the capsule look forward not backwards
        rotatedTargetVector = earthTiltRotation * targetVector;
        rotatedLookAtVector = earthTiltRotation * lookAtVector;
        transform.position = rotatedTargetVector;  
        transform.LookAt(rotatedLookAtVector);
        transform.Rotate(180, 0, 0);
        wasCapsuleMoved = true;
    }
    // public void ChangeOpacities(int i)
    // {
    //     i = (i == 12977) ? 12976 : i;
    //     foreach (GameObject lineParent in parents)
    //     {
    //         if (lineParent.activeInHierarchy)
    //         {
    //             parent = lineParent;
    //         }
    //     }
    //     if (i < linesAffected)
    //     {
    //         int j = i;
    //         while (j >= 0)
    //         {
    //             Line = parent.transform.Find(parent.name + " Line " + j);
    //             lr = Line.GetComponent<LineRenderer>();
    //             material = lr.material;
    //             color = material.color;
    //             color.a = (float)(i - j) / i;
    //             material.color = color;
    //             j--;
    //         }
    //         for (int k = 0; k < linesAffected; k++)
    //         {
    //             Line = parent.transform.Find(parent.name + " Line " + (i + k));
    //             lr = Line.GetComponent<LineRenderer>();
    //             material = lr.material;
    //             color = material.color;
    //             color.a = k / linesAffected;
    //             material.color = color;
    //         }
    //     }
    //     else if (i < lineCount - linesAffected)
    //     {
    //         for (int j = 0; j < linesAffected + 1; j++)
    //         {
    //             Line = parent.transform.Find(parent.name + " Line " + (i + j));
    //             lr = Line.GetComponent<LineRenderer>();
    //             material = lr.material;
    //             color = material.color;
    //             color.a = j / linesAffected;
    //             material.color = color;

    //             Line = parent.transform.Find(parent.name + " Line " + (i - j));
    //             lr = Line.GetComponent<LineRenderer>();
    //             material = lr.material;
    //             color = material.color;
    //             color.a = j / linesAffected;
    //             material.color = color;
    //         }
    //     }
    //     else
    //     {
    //         for (int j = 0; j < linesAffected + 1; j++)
    //         {
    //             Line = parent.transform.Find(parent.name + " Line " + (i - j));
    //             lr = Line.GetComponent<LineRenderer>();
    //             material = lr.material;
    //             color = material.color ;
    //             color.a = j / linesAffected;
    //             material.color = color;
    //         }
    //         int k = 0;
    //         while (i + k < lineCount)
    //         {
    //             Line = parent.transform.Find(parent.name + " Line " + (i + k));
    //             lr = Line.GetComponent<LineRenderer>();
    //             material = lr.material;
    //             color = material.color;
    //             color.a = k / linesAffected;
    //             material.color = color;
    //             k++;
    //         }
    //     }
    // }
}
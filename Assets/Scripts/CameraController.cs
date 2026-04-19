using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Cinemachine Brain
    public Camera mainCamera;
    //Cinemachime virtual cameras
    public CinemachineVirtualCamera thirdPerson;
    public CinemachineVirtualCamera topDown;
    public CinemachineFreeLook freeLook;
    public CinemachineVirtualCamera frontEnd;
    public CinemachineVirtualCamera birdsEye;
    //Which camera is connected text
    public TextMeshProUGUI cameraText;
    //Culling mask of the camera
    private int cullingMask;
    //The layer that the big lines and red dot are on
    private int bigLinesLayer = 7;
    //Integer for how many times tab is clicked
    private int cameraClicks;
    //The lil remainder
    private int remainder;
    //Integer for how many times space is clicked
    private int lockClicks;
    private void Start()
    {
        //yay get the culling mask of the camera!
        cullingMask = mainCamera.cullingMask;
    }
    private void Update()
    {
        //if tab is clicked
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //add 1 and divide by 5 and find the remainder
            cameraClicks++;
            remainder = cameraClicks % 5;
            {
                //if clicks is 5, 10, etc
                if (remainder == 0)
                {   
                    //set the priority of each camera, third persom being the highest
                    thirdPerson.m_Priority = 4;
                    //set the text to Third Person cause its the name
                    cameraText.text = thirdPerson.name;
                    topDown.m_Priority = 3;
                    freeLook.m_Priority = 2;
                    frontEnd.m_Priority = 1;
                    birdsEye.m_Priority = 0;
                    //Ok this means call this function after 0.7 seconds
                    Invoke("ChangeCullingMask", 0.7f);
                }
                if (remainder == 1)
                {   
                    //Same thing on the next 4 of these but a different camera is at 4
                    thirdPerson.m_Priority = 0;
                    topDown.m_Priority = 4;
                    cameraText.text = topDown.name;
                    freeLook.m_Priority = 3;
                    frontEnd.m_Priority = 2;
                    birdsEye.m_Priority = 1;
                }
                if (remainder == 2)
                {   
                    thirdPerson.m_Priority = 1;
                    topDown.m_Priority = 0;
                    freeLook.m_Priority = 4;
                    cameraText.text = freeLook.name;
                    frontEnd.m_Priority = 3;
                    birdsEye.m_Priority = 2;
                }
                if (remainder == 3)
                {   
                    thirdPerson.m_Priority = 2;
                    topDown.m_Priority = 1;
                    freeLook.m_Priority = 0;
                    frontEnd.m_Priority = 4;
                    birdsEye.m_Priority = 3;
                    cameraText.text = frontEnd.name;
                }
                if (remainder == 4)
                {   
                    thirdPerson.m_Priority = 3;
                    topDown.m_Priority = 2;
                    freeLook.m_Priority = 1;
                    frontEnd.m_Priority = 0;
                    birdsEye.m_Priority = 4;
                    cameraText.text = birdsEye.name;
                    //Same thing as when remainder was 0
                    Invoke("ChangeCullingMask", 0.7f);
                }
            }
        }
        //If free look is the main camera
        if (remainder == 2)  
        {
            //If space is clicked
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Add 1 this thang
                lockClicks++;
                //If it needs to be locked, 1, 3, etc
                if (lockClicks % 2 == 1)
                {
                    //Set the speed camera movement 0 so the camera cannot move
                    freeLook.m_YAxis.m_MaxSpeed = 0;
                    freeLook.m_XAxis.m_MaxSpeed = 0;
                }
                //If it needs to be unlocked, 2, 4, etc
                if (lockClicks % 2 == 0)
                {
                    //Set the speed back to the old ones
                    freeLook.m_YAxis.m_MaxSpeed = 2;
                    freeLook.m_XAxis.m_MaxSpeed = 300;
                }
            } 
        }       
    }  
    private void ChangeCullingMask()
    {
        //If we just moved off of the birds eye
        if (remainder == 0)
        {
            //Get rid of the layer with big lines and red dot
            cullingMask &= ~(1 << bigLinesLayer);
        }
        //If we just went onto the birds eye camera
        else
        {
            //Add the layer with big lines and red dot
            cullingMask |= 1 << bigLinesLayer;
        }
        //Update the camera's culling maask
        mainCamera.cullingMask = cullingMask;
    }
}

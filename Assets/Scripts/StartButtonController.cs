using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonController : MonoBehaviour
{
    //el starto buttono
    public Button startButton;

    public void ChangeScene()
    {
        //LOAD THE LOADING SCENE!!!!!!!!
        SceneManager.LoadScene(2);
    }
}



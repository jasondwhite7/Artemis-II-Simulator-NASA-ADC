using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
public void Start()
{
    Invoke("LoadMainScene", 0.01f);
}
public void LoadMainScene()
{
    SceneManager.LoadScene(1);
}
}
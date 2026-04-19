using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cinemachine;


public class PerformanceToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject background;
    public GameObject miniCamera;
    public GameObject minimapPanel;
    public Image checkmark;
    private int buttonClicks;
    public CinemachineVirtualCamera thirdPerson;
    public CinemachineVirtualCamera topDown;
    public CinemachineFreeLook freeLook;
    public CinemachineVirtualCamera frontEnd;
    private Color green = new Color(0.04463333f, 0.5566038f, 0.04463333f);

    void Start()
    {
        background.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        background.SetActive(false);
    }
    public void TogglePerformance()
    {
        buttonClicks++;
        if (buttonClicks % 2 == 1)
        {
            checkmark.color = green;
            miniCamera.SetActive(false);
            minimapPanel.SetActive(false);
            thirdPerson.m_Lens.FarClipPlane = 30000;
            topDown.m_Lens.FarClipPlane = 30000;
            freeLook.m_Lens.FarClipPlane = 30000;
            frontEnd.m_Lens.FarClipPlane = 30000;
        }
        else
        {
            checkmark.color = Color.black;
            miniCamera.SetActive(true);
            minimapPanel.SetActive(true);
            thirdPerson.m_Lens.FarClipPlane = 10000000;
            topDown.m_Lens.FarClipPlane = 10000000;
            freeLook.m_Lens.FarClipPlane = 10000000;
            frontEnd.m_Lens.FarClipPlane = 10000000;
        }
    }
}

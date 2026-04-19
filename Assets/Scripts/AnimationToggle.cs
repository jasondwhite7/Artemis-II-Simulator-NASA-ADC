using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimationToggle: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject background;
    private int buttonClicks;
    public Image checkmark;
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
    public bool ToggleAnimations()
    {
        buttonClicks++;
        if (buttonClicks % 2 == 1)
        {   
            checkmark.color = green;
            return true;
        }    
        else
        {
            checkmark.color = Color.black;
            return false;
        }
    } 
}

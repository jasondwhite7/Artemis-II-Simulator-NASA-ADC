using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    public RectTransform panel;
    private Vector2 startingPanel;
    private Vector2 endingPanel;
    public CanvasGroup canvas;
    private float slideDuration = 0.5f;
    private float fadeDuration = 1;
    void Start()
    {
        startingPanel = panel.anchoredPosition;
        endingPanel = new Vector2(-223.67f, -54f);
    }
    public void CueNotification()
    {
        StartCoroutine(SlideAndFade());
    }
    IEnumerator SlideAndFade()
    {
        yield return StartCoroutine(SlidePanel(startingPanel, endingPanel, slideDuration));
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(FadePanel(fadeDuration));
        gameObject.SetActive(false);
    }
    IEnumerator SlidePanel(Vector2 start, Vector2 end, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            panel.anchoredPosition = Vector2.Lerp(start, end, time/duration);
            yield return null;
        }
        panel.anchoredPosition = end;
    }
    IEnumerator FadePanel(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvas.alpha = 1 - time/duration;
            yield return null;
        }
        canvas.alpha = 0;
    }
}

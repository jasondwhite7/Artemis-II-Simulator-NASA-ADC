using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    public GameObject serviceModule;
    public GameObject secondStage;
    public GameObject capsule;
    private Vector3 secondStagePos;
    private Vector3 serviceModulePos;
    private Vector3 capsulePos;
    public void Start()
    {
        animator = GetComponent<Animator>();
        secondStagePos = secondStage.transform.localPosition;
        serviceModulePos = serviceModule.transform.localPosition;
        capsulePos = capsule.transform.localPosition;
    }
    public void RunSecondStage()
    {
        animator.SetTrigger("PlaySecondStage");
        StartCoroutine(WaitForSecondStage());                         
    }
    public void RunServiceModule()
    {
        animator.Play("New State");
        animator.SetTrigger("PlayServiceModule");
        StartCoroutine(WaitForServiceModule());
    }
    public void RunSplashDown()
    {
        animator.Play("New State");
        animator.SetTrigger("PlaySplashdown");
        StartCoroutine(WaitForSplashdown());
    }
    public void HideSecondStage()
    {
        secondStage.SetActive(false);
    }
    public void HideServiceModule()
    {
        serviceModule.SetActive(false);
    }

private IEnumerator WaitForSecondStage()
{
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    // Wait until the animator enters the Second Stage Separation State (Separation is Complete)
    while (!stateInfo.IsName("Second Stage Separation"))
    {
        yield return null; // Wait for the next frame
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    // Wait for the length of the animation
    yield return new WaitForSeconds(stateInfo.length);

    // Destroy the object after the animation finishes
    secondStage.SetActive(false);
}
private IEnumerator WaitForServiceModule()
{
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    // Wait until the animator enters the Service Module Separation State (Separation is Complete)
    while (!stateInfo.IsName("Service Module Separation"))
    {
        yield return null; // Wait for the next frame
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    // Wait for the length of the animation
    yield return new WaitForSeconds(stateInfo.length);

    // Destroy the object after the animation finishes
    serviceModule.SetActive(false);
}
private IEnumerator WaitForSplashdown()
{
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    // Wait until the animator enters the Service Module Separation State (Separation is Complete)
    while (!stateInfo.IsName("Splashdown"))
    {
        yield return null; // Wait for the next frame
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    // Wait for the length of the animation
    yield return new WaitForSeconds(stateInfo.length);

    // Destroy the object after the animation finishes
    capsule.SetActive(false);
}
public void RestartCapsule()
{
    secondStage.SetActive(true);
    serviceModule.SetActive(true);
    capsule.SetActive(true);

    animator.Play("New State");

    animator.ResetTrigger("PlaySecondStage");
    animator.ResetTrigger("PlayServiceModule");
    animator.ResetTrigger("PlaySplashdown");

    secondStage.transform.localPosition = secondStagePos;
    serviceModule.transform.localPosition = serviceModulePos;
    capsule.transform.localPosition = capsulePos;
}
}

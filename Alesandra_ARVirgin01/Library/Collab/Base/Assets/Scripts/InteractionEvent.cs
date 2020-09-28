using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    private bool isActivated = false;

    private void Start()
    {
        //deactivate();
    }

    private void OnMouseDown()
    {
        if(!isActivated)
        {
            activate();
        }
        else
        {
            Debug.Log("Interaction event has already been activated!");
        }
    }

    private void activate()
    {
        Debug.Log("Starting interaction event!");
        isActivated = true;

        if(animator != null)
        {
            animator.SetTrigger("startEvent");
        }
        
        if(audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void deactivate()
    {
        Debug.Log("Stopping interaction event!");
        isActivated = false;

        if (animator != null)
        {
            animator.SetTrigger("stopEvent");
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}

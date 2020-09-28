
/*

La Historia de Rosario Flores
Alesandra Miro Quesada
Interaction Event Manager

This script assigned to every Game Object controlls the activation and deactivation of the subtitles, animations, audio/video 
and interactions with the AR Image Targets. 
It also creates a string which will be used to display more information about that tracked image working alongisde 
the TrackingManager script.
Having a script like this ensure that all of your Image Targets are playing accordingly and acts as a master controll for all 
of the subcomponents of the Image Targets.

Bibliography and Links:
https://docs.unity3d.com/Manual/class-AnimatorController.html
https://docs.unity3d.com/Manual/class-Transition.html
https://answers.unity.com/questions/785273/whats-the-difference-between-activatedeactivate-en.html#:~:text=enable%20%2F%20disable%20is%20specifically%20for,you%20can%20disable%20that%20script.
https://docs.unity3d.com/Manual/class-VideoPlayer.html

*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InteractionEvent : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    [TextArea] //This TextArea Attribute allows a string to be edited in a scrollable text area
    public string trackingInfo; // string that will display the necesary information about the tracked ImageTarget
    private bool isActivated = false;
    private Subtitles subtitles;



    private void Start()
    {
        subtitles = GetComponent<Subtitles>(); //Accessing our Subtitles Script
        deactivate();
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

    public void activate()
    {
        Debug.Log("Starting interaction event!");
        isActivated = true;

        if(animator != null)
        {
            animator.SetBool("eventActive", true); //start animation
        }
        
        if(audioSource != null)
        {
            audioSource.Play(); //play sound
        }

        if(subtitles != null)
        {
            subtitles.activate(); //activate subtitles
        }

        TrackingManager.instance.setTrackedInfo(trackingInfo); //whenever something is being tracked display tracking Info string connected to the Trackign Manager
    }

    public void deactivate()
    {
        if(!isActivated) //if it stops tracking 
        {
            return;
        }

        Debug.Log("Stopping interaction event!");
        isActivated = false;

        if (animator != null)
        {
            animator.SetBool("eventActive", false);
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        if (subtitles != null)
        {
            subtitles.deactivate();
        }

        TrackingManager.instance.setTrackedInfo(null); //deactivating the tracked info so it goes back to originall message
    }
}

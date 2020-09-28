/*

La Historia de Rosario Flores
Alesandra Miro Quesada
Tracking Manager

This script is responsible for identifying when an image is being tracked and displaying the correspondant information
to that specific Image Target.
When an Image Target starts being tracked it will send over a script to this TrackingManager and on our UI it will allow us to have
that informatino there on a pop up.

Bibliography and Links:
https://developer.vuforia.com/forum/unity-extension-technical-discussion/get-tracked-image-runtime
https://stackoverflow.com/questions/12307139/error-in-if-while-condition-argument-is-of-length-zero
https://forum.unity.com/threads/check-if-entire-array-is-empty.588517/
https://docs.unity3d.com/ScriptReference/TextAreaAttribute.html
https://www.youtube.com/watch?v=mOUC91plKTg
https://answers.unity.com/questions/424874/showing-a-textarea-field-for-a-string-variable-in.html

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingManager : MonoBehaviour
{
    public static TrackingManager instance;

    [TextArea] //This TextArea Attribute allows a string to be edited in a scrollable text area
    public string notTrackingMessage = "Press this button when you're tracking something to learn more about it!"; //Every time the app is not trackign it will display this message
    public GameObject trackedImageInfo;
    public Text trackedImageInfoText; //If it is tracking it will display the text for the corresponding Image Target

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        trackedImageInfoText.text = notTrackingMessage; //starts off by not tracking
    }

    public void setTrackedInfo(string info) //If infor is equeal to null you get the no Tracking message
    {
        if(info == null || info.Length == 0)
        {
            trackedImageInfoText.text = notTrackingMessage;
        }
        else
        {
            trackedImageInfoText.text = info; //otherwise reveal the info to that specific Image Target
        }
    }

    public void toggleInfo() //then on our UI I will be able to assign specific text
    {
        trackedImageInfo.SetActive(!trackedImageInfo.activeInHierarchy);
    }
}

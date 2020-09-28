
/*

La Historia de Rosario Flores
Alesandra Miro Quesada
Subtitle Script

This script uses time stamps and strings to match subtitle text files to either audio or video files.
This was by far one of the most challenging scripts I had to write for the whole project. Even though the script is not that long 
and it may not appear to be very complex, logically it was very challenging to figure out as well as learning 
about new special corutine functions.
This script works in conjunction with the JavaSubtitleProcessor program which formatts the .txt file to be read here.

This program imports a .txt file, loads them and separates them as a sequence
1) Import .txt file
2) Split subs each line and also when "|" is detected
3) Trims current line and adds the line to a Subtitle TimeStamp which contains a time and a sub
4) Uses IENumerator to controll when the subtitles are displayed
5) Calculates the waitTime between subtitles
6) Proceedes onto the next subtitle line

Bibliography and Links:
https://forum.unity.com/threads/how-can-i-make-a-c-method-wait-a-number-of-seconds.61011/
https://stackoverflow.com/questions/12932306/how-does-startcoroutine-yield-return-pattern-really-work-in-unity
https://www.youtube.com/watch?v=UnrITNPDnas
https://answers.unity.com/questions/779761/cant-understand-how-to-use-yield.html
https://www.youtube.com/watch?v=oNC6UtxMXcA
https://hub.packtpub.com/arrays-lists-dictionaries-unity-3d-game-development/
https://www.youtube.com/watch?v=zGzdFrZRNG8
https://answers.unity.com/questions/1386291/how-to-wait-a-certain-amount-of-seconds-in-c.html
https://gamedevbeginner.com/coroutines-in-unity-when-and-how-to-use-them/#coroutine_best_practice
https://answers.unity.com/questions/1253570/creating-a-tooltip-when-hovering-over-a-ui-button.html

*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Video;

public class Subtitles : MonoBehaviour
{
    [Tooltip("Filename of the subtitle data")] //Tooltips are essencially a help/info bubble of what that gameObject is looking for
    public string subtitlesFileName;
    public Text subtitleText;
    public bool useOnlyAudio = false; //for AR Image targets that dont have videos attached to them

    private List<SubtitleTimeStamp> subtitles; 
    private int currentSubtitleIndex = 0;
    private bool isActivated = false; // it starts turned off so that it can be triggered with the corresponding vide/audio
    private VideoPlayer video; //where the corresponding video is going to go
    private AudioSource audioSource; //where the corresponding audio is going to go

    private class SubtitleTimeStamp
    {
        //creating variables to match up the correct time stamp to the correct subtitle
        public float timeStamp { get; private set; } //can only the var in a public setting, but set it in a private one!!
        public string subtitle { get; private set; }

        public SubtitleTimeStamp(float timeStamp, string subtitle)
        {
            this.timeStamp = timeStamp;
            this.subtitle = subtitle;
        }
    }

    private void Awake()
    {
        //immediatly create a list of all the subtitles .txt files that are loaded
        subtitles = new List<SubtitleTimeStamp>();
        
        //the if statement makes it possible to use either a video or an audio source
        if(useOnlyAudio)
        {
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            video = GetComponent<VideoPlayer>();
        }

        //calling the subs
        loadSubtitles();
    }

    public void activate()
    {

        //plays video or audio source when image is tracked
        isActivated = true;
        if (video != null)
        {
            video.Play();
        }

        if(audioSource != null)
        {
            audioSource.Play(); 
        }

        StartCoroutine(startSubtitleSequence()); //Corutines need to be called specifically like this otherwise errors everywhere!
    }

    public void deactivate()
    {
        //pause it if image is not being tracked anymore
        isActivated = false;
        if (video != null)
        {
            video.Pause(); //this is key to keep the subs from restarting or stopping all together
        }

        if (audioSource != null)
        {
            audioSource.Pause();
        }

        StopAllCoroutines();
    }

    private void loadSubtitles()
    {
        if(subtitlesFileName.EndsWith(".txt"))
        {
            subtitlesFileName = subtitlesFileName.Substring(0, subtitlesFileName.IndexOf(".txt"));
        }

        TextAsset text = Resources.Load<TextAsset>("Subtitle_Data/" + subtitlesFileName);
        string[] lines = text.text.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        //string[] lines = File.ReadAllLines("Assets/Resources/Subtitle_Data/" + subtitlesFileName);

        //loop that is going to process and split the subtitles  into lines
        for(int i = 0; i < lines.Length; i++)
        {
            string currentLine = lines[i];
            if(currentLine.Length > 1)
            {
                int splitIndex = currentLine.IndexOf("|");//here is where we split the subs
                float time = float.Parse(currentLine.Substring(0, splitIndex)); //converts string to float
                string subtitle = currentLine.Substring(splitIndex + 1).Trim();
                subtitles.Add(new SubtitleTimeStamp(time, subtitle));//add the subs with correspoinding time and sub information
            }
        }
    }

    // Corrutines are a special type function that allows Unity to stop the extecution of 'something' untill it meets
    // a certain condition, then it continue where it left off.
    // Requirements of corrutine function:
    // 1) Must return IEnumerator
    // 2) Must include a yield statement

    private IEnumerator startSubtitleSequence() //This is the golden nugget of the script and what essencially makes the subs run
    {
        while(isActivated)
        {
            SubtitleTimeStamp current = subtitles[currentSubtitleIndex]; //grabs the corresponding time stamp and subtitle information
            subtitleText.text = current.subtitle;

            float waitTime = 0f; 
            float currentTime = useOnlyAudio ? audioSource.time : (float)video.time; //only use audio, if thats the case grab specific time, otherwise its going to be video.time
            //if we are not at the last subtitle
            if(currentSubtitleIndex < subtitles.Count - 1)
            {
                float nextSubtitleTimestamp = subtitles[currentSubtitleIndex + 1].timeStamp;
                waitTime = nextSubtitleTimestamp - currentTime;
            }
            else
            {
                waitTime = (float)(video.length - currentTime); //simple maths that allows us to figure out how long we have to wait
            }

            yield return new WaitForSeconds(waitTime);  //returning a new instance of the WaitForSeconds class and inside we are passing the amount we want to wait

            if(currentSubtitleIndex == subtitles.Count - 1)
            {
                currentSubtitleIndex = 0;
            }
            else
            {
                currentSubtitleIndex++;
            }
        }
    }
}

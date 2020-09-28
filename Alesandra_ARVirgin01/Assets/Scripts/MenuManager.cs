/*

La Historia de Rosario Flores
Alesandra Miro Quesada
Menu Manager

This script controlls all the navigationa and activation of the app.Its responsible for triggering buttons and displaying 
the appropriate information on screen. Moreover when START EXPERIENCE is pressed it also activates the AR Image targets.
I had very limited knowledge of UI but luckly Unity makes it very easy to controll and understand as well as providing a lot 
of very usefull documentations and videos. 
This was the last script I worked on and even though now I already see things I want to change in terms of UI and UX, the core
of the button navigation manager was straight forward and clear to udnerstand.

Bibliography and Links:
https://answers.unity.com/questions/1724825/ui-menushandling-menus.html
https://medium.com/adventures-in-ar/placing-different-objects-in-ar-using-on-screen-buttons-in-unity-arcore-ac3f446c35a
https://www.youtube.com/watch?v=S1VrS05IxyQ
https://www.coursera.org/lecture/intermediate-object-oriented-programming-unity-games/adding-a-menu-manager-htRQm
https://forum.unity.com/threads/hide-show-menu-on-keydown.715601/
https://www.octomangames.com/unity-tutorials/unity-5-tutorial-simple-2d-menu/


*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static int numTrackedImages = 0;

    public GameObject trackedImages;
    public MenuData englishUI;
    public MenuData spanishUI;

    private MenuData currentUI; //represents the current UI we are using, either the english or spanish one
    private GameObject mainMenu; //represents the current UI's main menu section
    private bool isUsingEnglish = true;

    //Creatig sub classes that will store all of the Menus 
    [Serializable] //Serializable in Unity means we are allowed to see this in the inspector. Great for working with lots of UI such as englisha and spanish menus
    public class MenuData //Stored group of all of the below objects
    {
        //declaring all my menus
        public GameObject UI;
        public GameObject mainMenu;
        public GameObject HUD; //new word I leart that describes the head-up display esencially everything that appears glued onto the screen: buttons, menus, health bar etc
        public GameObject ARInstructions;
    }

    private void Start()
    {
        //start with tracked images turned off, because we always start in the main menu, and don't want to track images while in
        //the main menu
        trackedImages.SetActive(false);
        setLanguageToSpanish(false); //not implemented but working on it

    }

    // Further work will require an option to select spanish or english menu, this way the audience is free to choose
    public void setLanguageToSpanish(bool status)
    {
        if(status)
        {
            currentUI = spanishUI;
        }
        else
        {
            currentUI = englishUI;
        }
        isUsingEnglish = !status;
        mainMenu = currentUI.mainMenu;
    }

    public void activateSubMenu(GameObject submenu)
    {
        submenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    //Back button deactivates the curent maenu and shows the main menu
    public void onBackClick(GameObject currentMenu)
    {
        currentMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void onBackClickSelfOnly(GameObject currentMenu)
    {
        currentMenu.SetActive(false);
    }

    //Exits main menu and takes you to experience where the tracked AR images are activated and HUD menu are now available
    //AR experience can begin!
    public void onStartExperienceClick()
    {
        mainMenu.SetActive(false);
        currentUI.HUD.SetActive(true);
        trackedImages.SetActive(true);
    }

    public void takeScreenShot()//not implemented but working on it 
    {
        ScreenCapture.CaptureScreenshot("ARVirgin_Screenshot_" + DateTime.Now); //still a bit buggy and found out that it might only work with a plugin
    }

    //AR instructions menu
    public void showARInstructions()
    {
        currentUI.ARInstructions.SetActive(true);
    }
    
    //Back to main Menu sets everyting back to how it began.
    public void backToMainMenu()
    {
        currentUI.ARInstructions.SetActive(false);
        currentUI.HUD.SetActive(false);
        mainMenu.SetActive(true);
        trackedImages.SetActive(false);
    }


} 

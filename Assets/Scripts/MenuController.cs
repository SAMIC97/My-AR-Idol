using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Inworld.Util;

//Menu Scene - Main Menu (Groups Menu) script
public class MenuController : MonoBehaviour
{
    public GameObject panelGroupsMenu;//Groups Menu, initial menu
    public GameObject panelMembers;//Memebers Menu
    public GameObject panelComingSoon;//Memebers Menu
    public GameObject panelDA;//Disclaimer and Agreement
    public GameObject startButton;
    public ButtonPressOnTextInput buttonPressScript;

    private string nameButton;//Stores the members name that is selected

    void Start()
    {
        // Check the value of PlayerPrefs variable at the beginning
        int someValue = PlayerPrefs.GetInt("Menus");

        if (someValue == 1)
        {
            MembersMenu();
        } 
        else if (someValue == 2)
        {
            GroupsMenu();
        }

        // Deactivate the additional button initially
        startButton.SetActive(false);
    }

    //User agrees on the D&A, panel deactivate
    public void agreedDA()
    {
        panelDA.SetActive(false);
    }

    //When group icon is slected it deactivates Panel 1(Groups menu) to show Panel 2(Members Menu)
    public void ActivatePanel2(string buttonName)
    {
        if (buttonName == "EXO")
        {
            // Deactivate Panel 1(Groups menu)
            panelGroupsMenu.SetActive(false);

            // Activate Panel 2(Members Menu)
            panelMembers.SetActive(true);
        }
        else
        {
            panelComingSoon.SetActive(true);
            StartCoroutine(HidePanelAfterDelay(1.0f));
        }

        // Print which button was pressed
        //Debug.Log("Button Pressed: " + buttonName);
    }

    //When a members icon is selected Start button will show up
    public void ShowButtonGetName(Button buttonName)
    {
        startButton.SetActive(true);

        nameButton = buttonName.name;
    }

    //When Start button is selected will load the AR scene
    public void StartButtonSelected()
    {
        if (nameButton == "CHANYEOL")
        {
            LoadNewScene();
        }
        else
        {
            panelComingSoon.SetActive(true);
            StartCoroutine(HidePanelAfterDelay(1.0f));
        }
    }

    //Loads the new scene
    void LoadNewScene()
    {
        SceneManager.LoadScene("AR Scene");
    }

    //When the Back arrow button is pressed, it will show the Panel 1(Groups Menu)
    public void ShowPanel1()
    {
        // Activate Panel 1(Groups menu)
        panelGroupsMenu.SetActive(true);

        //Deactivate the start button when going back to Groups Menu
        startButton.SetActive(false);

        // Call the GoBackPanelGroups method from ButtonPressOnTextInput script
        buttonPressScript.GoBackPanelGroups();

        // Deactivate Panel 2(Members Menu)
        panelMembers.SetActive(false);
    }

    //When user selected from AR Scene to go back to the member menu
    void MembersMenu()
    {
        agreedDA();
        panelGroupsMenu.SetActive(false);
        panelMembers.SetActive(true);
        PlayerPrefs.SetInt("Menus", 0);
    }

    //When user selected Home from mini Menu
    void GroupsMenu()
    {
        agreedDA();
        panelGroupsMenu.SetActive(true);
        PlayerPrefs.SetInt("Menus", 0);
    }

    IEnumerator HidePanelAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Disable the panel GameObject after the delay
        panelComingSoon.SetActive(false);
    }

}

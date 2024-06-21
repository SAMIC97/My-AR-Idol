using System;
using UnityEngine;
using Inworld;
using System.Collections;
using Inworld.Util;

public class GameManager : MonoBehaviour
{
    public GameObject panelUp;//Panel that contains the Arrow Back Button
    public GameObject panelDown;//Panel that contains the microphone and outfit button
    public GameObject panelMiddle;//Panel that contains the instructions for AR
    public GameObject panelOutfit;//Panel that contains the arrows and Ok button to choose the outfit
    public GameObject panelMiniMenu;//Panel that contains the mini Menu to go back to main menu, member menu or cancel button
    public GameObject panelIndications;//Panel that contains the indications to talk, outfit
    public GameObject missionList;//Image with Missions List
    public GameObject buttonMissionList;//Button to access the Missions List

    public GameObject imageTarget;//Game object with the image target of Vuforia, inWorldController, and 3D models to show
    public GameObject arCamera;//Game object with Vuforia camera and player controller reference
    public GameObject outfitChoice;//Game object with the room, models outfit and camera UI

    [SerializeField] private GameData gameData;
    [SerializeField] private InworldUserSettings inWordlUserSetting;

    public bool okPressed;//Used to when user has read the instructions to use AR
    GameObject modelToShow;//Model to show according to the outfit choosen
    Transform directionalLight;//The light on GameObject ImageTarget

    private void Start()
    {
        panelDown.SetActive(false);
        panelOutfit.SetActive(true);
        imageTarget.SetActive(false);
        arCamera.SetActive(false);
        okPressed = false;
        inWordlUserSetting.Name = gameData.UserName;

        directionalLight = imageTarget.transform.Find("Directional Light");

        if (directionalLight == null)
        {
            Debug.LogWarning("Directional Light not found as a child of ImageTarget.");
        }
    }

    //When Ok button is pressed from the instructions panel
    public void instructionsOK()
    {
        //Activate the Panel with Home button
        panelUp.SetActive(true);

        //Inside Panel UP activate the mission list button
        buttonMissionList.SetActive(true);

        //Activate the panel with mic and outfit change buttons
        panelDown.SetActive(true);

        //Deactivate Instructions
        panelMiddle.SetActive(false);

        okPressed = true;

        //Start timer for indications
        showIndications();
    }

    //When Back button is pressed show Mini Menu
    public void ShowMiniMenu()
    {
        panelMiniMenu.SetActive(true);
        if(panelOutfit.activeInHierarchy)
        {
            panelOutfit.SetActive(false);
        }
        else if(panelDown.activeInHierarchy)
        {
            panelOutfit.SetActive(false);
        }

    }

    //When Vuforia camera detects image target
    //Checks whether the user has pressed ok on instructions
    public void OnTargetFound()
    {
        //Debug.Log("Target found");

        modelToShow.SetActive(true);
        instructionsOK();
        /*
        if (okPressed)
        {
            modelToShow.SetActive(true);
        }*/
    }

    //Function called when choosing an outfit and start AR experience
    //Activate ARCamera, Image target and change values in inWorldCronteller
    public void PerformGameManagerAction(String activeModelName)
    {
        //Activate Parent Game Object
        imageTarget.SetActive(true);

        // Get all Transform components of children (including the parent itself)
        Transform[] childTransforms = imageTarget.GetComponentsInChildren<Transform>(true);

        // Loop through all child Transform components (excluding the parent itself)
        foreach (Transform childTransform in childTransforms)
        {
            GameObject childGameObject = childTransform.gameObject;

            Transform modelToShowTransform = childTransform.Find(activeModelName);

            // Check for specific child GameObjects by name
            if (childGameObject.name == "Chanyeolie")
            {
                // Access the InworldCharacter script and change m_CurrentAvatar
                InworldCharacter inworldCharacterScript = childGameObject.GetComponent<InworldCharacter>();
                Animator inWorldAnimator = childGameObject.GetComponent<Animator>();
                
                // Access the GameObject component or perform actions
                modelToShow = modelToShowTransform.gameObject;

                if (inworldCharacterScript != null)
                {
                    // Modify the CurrentAvatar variable
                    inworldCharacterScript.CurrentAvatar = modelToShow;

                    //Modify the Animator variable
                    inWorldAnimator.avatar = modelToShow.GetComponent<Animator>().avatar;
                }
            }
        }

        //Activate the light for AR in imagetarget
        directionalLight.gameObject.SetActive(true);

        //Activate Vuforia camera
        arCamera.SetActive(true);

        //Deactivate panel to choose outfit
        panelOutfit.SetActive(false);

        //audioCapture.StartRecording();

        //if user already pressed the Ok instructions once, does not show again if outfit has changed
        if (!okPressed)
        {
            //Activate Instructions Panel
            panelMiddle.SetActive(true);
            panelUp.SetActive(false);
            buttonMissionList.SetActive(true);
        }
        else
        {
            instructionsOK();
        }

    }

    //Outfit button pressed and changes back to outfit view
    public void ChangeOutfit()
    {
        //Activate panel to choose outfit
        panelOutfit.SetActive(true);

        //Deactivate down buttons
        panelDown.SetActive(false);

        //Deactivate the Mission List when choosing outfit
        buttonMissionList.SetActive(false);

        //Deactivate mission panel list if active when choosing to change outfit icon
        if (missionList.activeInHierarchy)
        {
            missionList.SetActive(false);
        }

        //Deactivate the indications when choosing outfit if active
        panelIndications.SetActive(false);

        //Activate GMO
        outfitChoice.SetActive(true);

        //Deactivate model active
        modelToShow.SetActive(false);

        //Deactivate Vuforia camera
        arCamera.SetActive(false);

        //Deactivate the light from AR Camera
        directionalLight.gameObject.SetActive(false);

        //audioCapture.StopRecording();

    }

    //Show Indications to talk with model or change outfits
    void showIndications()
    {
        panelIndications.SetActive(true);
        // Start the coroutine to hide the panel after 10 seconds
        StartCoroutine(HidePanelAfterDelay(10f));
    }

    IEnumerator HidePanelAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Disable the panel GameObject after the delay
        panelIndications.SetActive(false);
    }

    //To show Mission List when pressing button
    public void showMissionList()
    {
        missionList.SetActive(true);
        buttonMissionList.SetActive(false);
    }

    //For close button inside the Mission List
    public void closeMissionList()
    {
        buttonMissionList.SetActive(true);
        missionList.SetActive(false);
    }

    //Reset everything when quitting app
    private void OnApplicationQuit()
    {
        // Reset all PlayerPrefs keys
        PlayerPrefs.DeleteAll();

        // Reset GameData
        GameData.Instance.ResetData();
    }
}

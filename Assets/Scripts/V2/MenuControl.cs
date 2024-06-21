using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Inworld.Util;

public class MenuControl : MonoBehaviour
{
    public GameObject panelWelcomeMenu;//Welcome Menu, initial menu
    public GameObject panelDA;//Disclaimer and Agreement
    public GameObject panelScan;//Scan screen
    public GameObject panelName;//Input Name screen
    public GameObject panelMyLibrary;//My Library screen
    public GameObject startButton;//Start button
    public GameObject pcImage;//Image with PC design
    public GameObject backToButton;//Back button
    public GameObject noItemsImage;//Image with No Items in Librarry

    //public InworldUserSettings userSettings;
    public ImageTargetDetector imageTargetDetector;

    [SerializeField] private GameData gameData;
    [SerializeField] private InworldUserSettings inWordlUserSetting;

    public TMP_InputField inputField;//Input Field for user's name
    public Button nextButton;//Next button

    // Start is called before the first frame update
    void Start()
    {

        if (gameData.StateValue == "Home")
        {
            //Welcome panel to be activate if home button was pressed
            panelWelcomeMenu.SetActive(true);
            gameData.StateValue = "continueSession";
        }
        else
        {
            //Disclaimer and Agreement panel to show up first
            panelDA.SetActive(true);

            //Welcome panel iniatially needs to be deactivate
            panelWelcomeMenu.SetActive(false);

            gameData.LibraryEmpty = true;
        }

        //Cleans user name for user to change or write it again
        inWordlUserSetting.Name = gameData.UserName;

        // Disable the StartButton initially
        nextButton.interactable = false;

        // Add a listener to the input field to check for changes
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // Subscribe to the onEndEdit event of the input field
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    //User agrees on the D&A, panel deactivate
    public void AgreedDA()
    {
        panelWelcomeMenu.SetActive(true);
        panelDA.SetActive(false);
    }

    //Checks if InoutField text is not empty
    void OnInputFieldValueChanged(string newValue)
    {
        // Enable the StartButton only if the input field text is not empty
        nextButton.interactable = !string.IsNullOrEmpty(newValue);
    }

    //Once the PC has been scanned, change to AR scene
    public void OnStartButtonClicked()
    {
        /*
        //Changes to AR Scene experience
        SceneManager.LoadScene("AR Scene 1");
        */
        panelName.SetActive(true);
        panelScan.SetActive(false);
    }

    //Once the name is written show next Scene (Scan)
    public void OnNextButtonClicked()
    {
        /*
        panelName.SetActive(false);
        panelScan.SetActive(true);
        */

        //Changes to AR Scene experience
        SceneManager.LoadScene("AR Scene 1");
    }

    //Personalized user name
    void OnEndEdit(string text)
    {
        // Update the user name in the user settings script
        //userSettings.Name = text;
        gameData.UserName = text;
        SaveUserName();
    }

    //When Vuforia has found target to show Start button
    public void AfterImageTargetFound(UnityEngine.UI.Image foundImage)
    {
        startButton.SetActive(true);
        pcImage.SetActive(true);

        UnityEngine.UI.Image pcImageComponent = pcImage.GetComponent<UnityEngine.UI.Image>();
        if (pcImageComponent != null)
        {
            pcImageComponent.sprite = foundImage.sprite; // Assign the sprite from foundImage to pcImageComponent
            gameData.LibraryEmpty = false;
        }
    }


    //When back is pressed to go back to name
    public void BackToButton(string actualPanel)
    {
        if(actualPanel == "Scan")
        {
            startButton.SetActive(false);
            noItemsImage.SetActive(false);
            pcImage.SetActive(false);
            panelScan.SetActive(false);

        }
        else if(actualPanel == "MyLibrary")
        {
            panelMyLibrary.SetActive(false);
        }
        else
        {
            panelName.SetActive(false);
        }

        panelWelcomeMenu.SetActive(true);
        
    }

    //When User choose New Session Button from Main Menu
    public void NewSession()
    {
        /*
        panelName.SetActive(true);
        panelWelcomeMenu.SetActive(false);
        */
        panelScan.SetActive(true);
        panelWelcomeMenu.SetActive(false);

    }

    //When User choose My Library Button from Main Menu
    public void MyLibrary()
    {
        panelMyLibrary.SetActive(true);
        panelWelcomeMenu.SetActive(false);

        if (gameData.LibraryEmpty)
        {
            noItemsImage.SetActive(true);
            StartCoroutine(HidePanelAfterDelay(1.0f));
        }
    }

    //Save Username into GameData
    void SaveUserName()
    {
        // Update the user name in the user settings script
        inWordlUserSetting.Name = gameData.UserName;
    }

    IEnumerator HidePanelAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Disable the panel GameObject after the delay
        noItemsImage.SetActive(false);
    }

    /// <summary>
    /// To reset game values in mobile
    /// </summary>

    //Reset everything when quitting app
    private void OnApplicationQuit()
    {
        // Reset all PlayerPrefs keys
        PlayerPrefs.DeleteAll();

        // Reset GameData
        GameData.Instance.ResetData();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        // Save the user name when the application pauses
        SaveUserName();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // Save the user name when the application loses focus
        if (!hasFocus)
        {
            SaveUserName();
        }
    }


}

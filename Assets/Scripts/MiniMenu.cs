using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script to handle the Mini Menu behaviour
public class MiniMenu : MonoBehaviour
{
    public GameObject panelMiniMenu;//Panel that contains the mini Menu w/buttons
    public GameObject panelChoice;//Panel that contains the arrows to change and selected model
    public GameObject panelDown;//Panel that contains the microphone and outfit icon

    GameManager gameManager;
    bool okPressedGameManager;

    [SerializeField] private GameData gameData;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        okPressedGameManager = gameManager.okPressed;
    }

    //Button home is clicked changes to Menu Scene (Groups Menu)
    public void OnHomeButtonClicked()
    {
        // Save Game Data to show Menu wo D&A
        gameData.StateValue = "Home";

        SceneManager.LoadScene("Menu Scene 1");
    }

    //Button cancel is clicked Mini Menu panel is deactivated and active Outfit Choice panel
    public void OnCancelButtonClicked()
    {
        if (panelMiniMenu != null)
        {
            panelMiniMenu.SetActive(false);

            if (okPressedGameManager)
            {
                panelDown.SetActive(true);
            }
            else
            {
                panelChoice.SetActive(true);
            }
        }
    }

    //Button members is clicked changes to Menu Scene (Groups Menu) to show Members Menu
    //Discontinued on V2
    public void OnMembersButtonClicked()
    {
        // Save PlayerPrefs to show Members Menu instead of Menu Menu
        gameData.StateValue = "Members";

        // Load main scene
        SceneManager.LoadScene("Menu Scene");
    }
}

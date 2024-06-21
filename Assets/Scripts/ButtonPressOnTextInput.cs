using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Inworld.Util;

public class ButtonPressOnTextInput : MonoBehaviour
{
    public TMP_InputField inputField; // Reference to the TMP_InputField
    public Transform panelWithButtons; // Reference to the panel containing the buttons
    public InworldUserSettings userSettings;

    private Button currentButton; // Reference to the currently selected button
    private Button[] buttons;

    void Start()
    {
        // Subscribe to input field events
        inputField.onSelect.AddListener(OnInputFieldSelected);

        // Find all Button components under the panel
        buttons = panelWithButtons.GetComponentsInChildren<Button>();
        

        // Subscribe to the onEndEdit event of the input field
        inputField.onEndEdit.AddListener(OnEndEdit);

        // Add button press effect to each button
        foreach (Button button in buttons)
        {
            //AddButtonPressEffect(button);
            button.onClick.AddListener(() => OnButtonClicked(button));
        }

        // Ensure the input field and user settings reference are set
        if (inputField == null)
        {
            Debug.LogError("InputField reference is not set in UserNameInput script.");
            return;
        }

        if (userSettings == null)
        {
            Debug.LogError("InworldUserSettings reference is not set in UserNameInput script.");
            return;
        }
    }

    void OnInputFieldSelected(string text)
    {
        // Get the currently selected button
        //currentButton = GetSelectedButton();

        // Update the sprite of the current button
        UpdateButtonSprite(currentButton);
    }

    void OnButtonClicked(Button clickedButton)
    {
        // Reset the sprite of the current button if a new button is clicked
        if (currentButton != null && clickedButton != currentButton)
        {
            ResetButtonSprite(currentButton);
        }

        // Update the sprite of the clicked button
        currentButton = clickedButton;
        UpdateButtonSprite(clickedButton);
    }

    void UpdateButtonSprite(Button button)
    {
        // Change the sprite of the button to its pressed state
        button.image.sprite = button.spriteState.pressedSprite;
    }

    void ResetButtonSprite(Button button)
    {
        // Reset the sprite of the button to its normal state
        button.image.sprite = button.spriteState.highlightedSprite;
    }

    //Personalized experience by changing user name
    void OnEndEdit(string text)
    {
        // Update the user name in the user settings script
        userSettings.Name = text;
    }

    public void GoBackPanelGroups()
    {
        if(currentButton != null)
        {
            ResetButtonSprite(currentButton);
        }
        else
        {
            Debug.Log("No current button yet");
        }
        
    }
}

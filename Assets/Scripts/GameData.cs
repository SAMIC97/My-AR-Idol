using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    [SerializeField] private bool _libraryEmpty;
    [SerializeField] private string _userName;
    [SerializeField] private int _missionsDone;
    [SerializeField] private string _stateValue;//Home, continue session, AgreedD&A

    public bool LibraryEmpty
    {
        get { return _libraryEmpty;  }
        set { _libraryEmpty = value; }
    }

    public string UserName
    {
        get { 
            if (_userName == "" || _userName == null)
            {
                _userName = "Exo-L";
            }
            return _userName; }
        set { _userName = value; }
    }

    public int MissionsDone
    {
        get { return _missionsDone; }
        set { _missionsDone = value; }
    }

    public string StateValue
    {
        get { return _stateValue; }
        set { _stateValue = value; }
    }

    /// <summary>
    /// Array to store the completion status of missions
    /// </summary>
    [SerializeField] private bool[] _missionCompleted = new bool[3]; // Assuming there are 3 missions

    public bool[] MissionCompleted
    {
        get { return _missionCompleted; }
        set { _missionCompleted = value; }
    }

    /// <summary>
    /// To save the color information of the card that was scanned and saved in My Library
    /// </summary>
    [System.Serializable]
    public struct ImageColor
    {
        public string imageName;
        public Color color;
    }

    public static GameData Instance;

    public ImageColor[] imageColors; // Array to store the colors of images

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void SetColorForImage(string imageName, Color color)
    {
        // Check if the imageColors array is initialized
        if (imageColors == null)
        {
            imageColors = new ImageColor[0];
        }

        // Check if the image name already exists in the array
        for (int i = 0; i < imageColors.Length; i++)
        {
            if (imageColors[i].imageName == imageName)
            {
                // Update the color for the existing image name
                imageColors[i].color = color;
                return;
            }
        }

        // If the image name doesn't exist, add it to the array
        ImageColor newImageColor = new ImageColor();
        newImageColor.imageName = imageName;
        newImageColor.color = color;

        // Resize the array to accommodate the new element
        System.Array.Resize(ref imageColors, imageColors.Length + 1);
        imageColors[imageColors.Length - 1] = newImageColor;
    }

    public Color GetColorForImage(string imageName)
    {
        // Check if the imageColors array is initialized
        if (imageColors == null)
        {
            return Color.gray; // Default color if array is not initialized
        }

        // Search for the color associated with the specified image name
        foreach (var imageColor in imageColors)
        {
            if (imageColor.imageName == imageName)
            {
                return imageColor.color;
            }
        }

        return Color.gray; // Default color if image color is not found
    }

    /// <summary>
    /// To check whether a card has been already scanned or if it is a new one
    /// </summary>
    public string[] scannedCards;

    public bool IsFirstScan(string cardName)
    {
        return Array.IndexOf(scannedCards, cardName) == -1;
    }

    public void SaveScannedCard(string cardName)
    {
        if (IsFirstScan(cardName))
        {
            Array.Resize(ref scannedCards, scannedCards.Length + 1);
            scannedCards[scannedCards.Length - 1] = cardName;
        }
    }

    /// <summary>
    /// Save the Panel of the group to be shown
    /// </summary>

    // Add a dictionary to store panel states
    private Dictionary<string, bool> panelStates = new Dictionary<string, bool>();

    // To get the state of a panel by its name
    public int GetPanelState(string panelName)
    {
        return LoadPanelState(panelName) ? 1 : 0;
    }

    // To save the state of a panel
    public void SavePanelState(string panelName, bool isActive)
    {
        if (panelStates.ContainsKey(panelName))
        {
            panelStates[panelName] = isActive;
        }
        else
        {
            panelStates.Add(panelName, isActive);
        }
    }

    // To load the state of a panel
    public bool LoadPanelState(string panelName)
    {
        if (panelStates.ContainsKey(panelName))
        {
            return panelStates[panelName];
        }
        return false; // Default state if panel name not found
    }

    // To reset panel states
    public void ResetPanelStates()
    {
        panelStates.Clear();
    }

    public void ResetData()
    {
        _libraryEmpty = true;
        _userName = "";
        _missionsDone = 0;
        scannedCards = new string[0];
        _stateValue = "";

        // Reset mission completion status
        Array.Clear(_missionCompleted, 0, _missionCompleted.Length);
    }
}
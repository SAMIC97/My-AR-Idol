using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ImageTargetDetector : MonoBehaviour
{
    public ObserverBehaviour nObserverBehaviour;
    public MenuControl menuControl;
    public GameObject myLibraryPanel; // Reference to the My Library panel
    public GameObject[] libraryImages; // Array of images in your library panel

    public GameObject alreadyInLibraryImage; // Image with already added text
    public GameObject newCardAddedImage; // Image with new item added to ML text

    [SerializeField] private GameData gameData;

    private void Start()
    {
        nObserverBehaviour = GetComponent<ObserverBehaviour>();

        // Load the saved states of the panels
        LoadPanelStates();
    }

    private void LoadPanelStates()
    {
        Debug.Log(gameData.StateValue);

        if (gameData.StateValue == "Home")
        {
            /*
            foreach (Transform child in myLibraryPanel.transform)
            {
                // Check if the child GameObject has "Panel" in its name
                if (child.name.Contains("Panel"))
                {
                    string panelName = child.name.Replace("Panel ", "");
                    int state = gameData.GetPanelState(panelName);
                    child.gameObject.SetActive(state == 1);
                    Debug.Log("LoadPanelStates: " + child.gameObject.name);

                    // Apply saved image colors to child images
                    ApplySavedImageColors();
                }
            }*/
            // Apply saved image colors to child images
            ApplySavedImageColors();
        }
        else
        {
            ResetPanelState();
        }
    }

    //When image has been found finds the correspondent image and panel to be added (visible)
    public void OnTrackingFound()
    {
        // Find the corresponding image in the library panel
        GameObject foundImage = FindImageByName(nObserverBehaviour.TargetName);

        if (foundImage != null)
        {
            // Scroll to the found image
            UnblockPCImage(foundImage);            
        }

        if (myLibraryPanel != null)
        {
            // Extract the first word before the first "_" character
            string[] words = nObserverBehaviour.TargetName.Split('_');
            string panelName = words[0];

            // Find the child panel with a matching name
            Transform childPanel = myLibraryPanel.transform.Find("Panel " + panelName);
            Debug.Log("OnTrackingFound: " + childPanel.name);
            if (childPanel != null)
            {
                // Activate the found child panel
                childPanel.gameObject.SetActive(true);

                // Save the state of the activated child panel
                SavePanelState(panelName, true);
            }
            else
            {
                Debug.LogWarning("Child panel not found for target name: " + panelName);
            }
        }
    }

    public void OnTrackingLost()
    {
        if(alreadyInLibraryImage.activeInHierarchy)
        {
            alreadyInLibraryImage.SetActive(false);
        }
        else if (newCardAddedImage.activeInHierarchy)
        {
            newCardAddedImage.SetActive(false);
        }
    }

    //Looks for the correspondent image on the library Image
    private GameObject FindImageByName(string targetName)
    {
        foreach (GameObject image in libraryImages)
        {
            if (image.name == targetName)
            {
                return image;
            }
        }
        return null; // Image not found
    }

    //Changes the color to white, indicating is the PC that was unlocked and scanned
    private void UnblockPCImage(GameObject image)
    {
        // Change the color of the image to white
        UnityEngine.UI.Image imageComponent = image.GetComponent<UnityEngine.UI.Image>();
        if (imageComponent != null)
        {
            imageComponent.color = Color.white;
            GameData.Instance.SetColorForImage(image.name, Color.white);
            menuControl.AfterImageTargetFound(imageComponent);

            // Call OnCardScanned with the name of the scanned card
            OnCardScanned(image.name);
        }
        else
        {
            Debug.LogWarning("Image component not found on the GameObject.");
        }
    }

    // Save the data of the Panel that was found and is active
    public void SavePanelState(string panelName, bool isActive)
    {
        gameData.SavePanelState(panelName, isActive);
    }


    // Save the data of the Panel that was found and is active
    public void ResetPanelState()
    {
        gameData.ResetPanelStates();
    }

    // Apply saved image colors to child images
    public void ApplySavedImageColors()
    {
        if (libraryImages == null || libraryImages.Length == 0)
        {
            Debug.LogWarning("Library images array is null or empty.");
            return;
        }

        foreach (GameObject image in libraryImages)
        {
            string imageName = image.name; // Assuming the image name matches the target name
            Color savedColor = GameData.Instance.GetColorForImage(imageName);

            UnityEngine.UI.Image imageComponent = image.GetComponent<UnityEngine.UI.Image>();
            if (imageComponent != null)
            {
                imageComponent.color = savedColor;
            }
            else
            {
                Debug.LogWarning("Image component not found on the GameObject: " + image.name);
            }
        }
    }

    public void OnCardScanned(string cardName)
    {
        if (gameData.IsFirstScan(cardName))
        {
            newCardAddedImage.SetActive(true);
            alreadyInLibraryImage.SetActive(false);

            // Save the name of the scanned card
            gameData.SaveScannedCard(cardName);
        }
        else
        {
            newCardAddedImage.SetActive(false);
            alreadyInLibraryImage.SetActive(true);
        }
    }
}

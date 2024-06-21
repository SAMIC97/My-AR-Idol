using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ModelAndMaterialSwitcher : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public Button okButton; // Ok button reference

    public GameObject backgroundParent; // Parent GameObject containing multiple meshes
    public GameObject newMissionsOutfit; // Image with message for New Missions and Outfit


    public GameManager gameManager; // Reference to the GameManager
    public MissionCheck missionCheck; // Reference to the GameManager


    public GameObject[] models; // An array to hold your different models
    public Material[] materials; // An array to hold your different materials

    private int currentModelIndex = 0; // Index of the current model
    private string activeModelName; // Variable to store the name of the active model

    [SerializeField] private GameData gameData;

    void Start()
    {
        // Make sure there are models and materials assigned in the Unity Editor
        if (models == null || models.Length == 0 || materials == null || materials.Length == 0)
        {
            Debug.LogError("Please assign models and materials to the ModelAndMaterialSwitcher script in the Unity Editor.");
            return;
        }

        currentModelIndex = 0;
        activeModelName = "";

        if (gameData.StateValue == "continueSession")
        {
            IsFirtModel();
        }

        // Set the initial models and material
        SetBackgroundMaterial(materials[currentModelIndex]);

        // Attach the methods to the button click events
        leftButton.onClick.AddListener(SwitchModelAndMaterialBackward);
        rightButton.onClick.AddListener(SwitchModelAndMaterialForward);

        // Attach the method to the Ok button click event
        okButton.onClick.AddListener(OnOkButtonPressed);
    }

    //Changes the model and material forward when Right Arrow (>) is pressed
    void SwitchModelAndMaterialForward()
    {
        // Verify if index is bigger than lenght in array before increment the model index
        if ((currentModelIndex + 1) > models.Length - 1)
        {
            currentModelIndex = models.Length - 1;

            if (missionCheck.missionsDone)
            {
                //Debug.Log("missionsDone");
                newMissionsOutfit.SetActive(true);
                StartCoroutine(HidePanelAfterDelay(3f));
            }
        }
        else
        {
            currentModelIndex = (currentModelIndex + 1);
        }

        // Check if there are more materials
        if (currentModelIndex < materials.Length)
        {
            // Set the new material for the background meshes
            SetBackgroundMaterial(materials[currentModelIndex]);
        }

        IsFirtModel();
        // Show new model and hide previous one
        missionCheck.CheckUnlockOutfit();
        okButton.interactable = missionCheck.missionsDone;
        models[currentModelIndex - 1].SetActive(false);
        models[currentModelIndex].SetActive(true);
    }

    //Changes the model and material backwards when Left Arrow is pressed
    void SwitchModelAndMaterialBackward()
    {
        // Decrement the model index and wrap around if needed
        currentModelIndex = (currentModelIndex - 1);

        //Set limit to 0
        if (currentModelIndex < 0)
        {
            currentModelIndex = 0;
        }

        IsFirtModel();

        // Set the new material for the background meshes
        SetBackgroundMaterial(materials[currentModelIndex]);

        // Show new model and hide next one
        missionCheck.CheckUnlockOutfit();
        okButton.interactable = missionCheck.missionsDone;
        models[currentModelIndex + 1].SetActive(false);
        models[currentModelIndex].SetActive(true);
    }

    //Changes de material of the Room gameobject and its childs
    void SetBackgroundMaterial(Material material)
    {
        // Set the new material for all child meshes of the background parent
        Renderer[] childRenderers = backgroundParent.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childRenderers)
        {
            childRenderer.material = material;
        }
    }

    //User pressed ok button when outfit has been choosen
    //Deactivate current outfit gameobject and activate thorugh GameManager the AR features
    void OnOkButtonPressed()
    {
        if (gameManager != null)
        {
            // Store the name of the currently active model
            activeModelName = models[currentModelIndex].name;
            //Debug.Log(activeModelName);

            //Send and perform gameManager function
            gameManager.PerformGameManagerAction(activeModelName);

            //Deactivate Choose gameobject
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("GameManager reference is null.");
        }
    }

    void IsFirtModel()
    {
        if (currentModelIndex == 0)
        {
            //Debug.Log("Is First model");
            missionCheck.firstModel = true;
        }
        else { missionCheck.firstModel = false; }

    }

    IEnumerator HidePanelAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Disable the panel GameObject after the delay
        newMissionsOutfit.SetActive(false);
    }
}
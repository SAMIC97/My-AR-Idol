using UnityEngine;
using UnityEngine.UI;

namespace Inworld
{
    public class RecordingButtonController : MonoBehaviour
    {
        public AudioCapture audioCapture; // Reference to the AudioCapture script
        public Button uiRecordingButton; // Reference to the UI button
        public Sprite pressedSprite; // Reference to the sprite when the button is pressed
        public MissionCheck missionCheckRef;

        private Sprite defaultSprite; // Reference to the default sprite

        void Start()
        {
            if (uiRecordingButton != null)
            {
                // Save the default sprite
                defaultSprite = uiRecordingButton.image.sprite;

                // Attach the method to the button's click event
                uiRecordingButton.onClick.AddListener(ToggleRecording);
            }
            else
            {
                Debug.LogError("RecordingButtonController script requires a Button component on the same GameObject.");
            }

            uiRecordingButton.image.sprite = pressedSprite;
        }

        void OnDestroy()
        {
            // Remove the listener to avoid leaks
            if (uiRecordingButton != null)
            {
                uiRecordingButton.onClick.RemoveListener(ToggleRecording);
            }
        }

        void ToggleRecording()
        {
            if (audioCapture != null)
            {
                if (audioCapture.IsCapturing)
                {
                    // If recording is active, stop recording and set the default sprite
                    audioCapture.StopRecording();
                    uiRecordingButton.image.sprite = defaultSprite;
                    missionCheckRef.ChangeColorMission(2);
                    missionCheckRef.CompleteMission(2);
                }
                else
                {
                    // If not recording, start recording and set the pressed sprite
                    audioCapture.StartRecording();
                    uiRecordingButton.image.sprite = pressedSprite;
                }
            }
        }
    }
}

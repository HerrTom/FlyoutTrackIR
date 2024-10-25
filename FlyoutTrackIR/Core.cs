using UnityEngine;
using UnityEngine.InputSystem;
using MelonLoader;
using NaturalPoint.TrackIR;
using System.Collections;
using Il2CppInterop.Runtime.Injection;

[assembly: MelonInfo(typeof(FlyoutTrackIR.Core), "FlyoutTrackIR", "1.0.0", "HerrTom", null)]
[assembly: MelonGame("Stonext Games", "Flyout")]

namespace FlyoutTrackIR
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            ClassInjector.RegisterTypeInIl2Cpp<TrackIRComponent>();
            MelonLogger.Msg("Initialized FlyoutTrackIR.");
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "PlanetScene2") { MelonCoroutines.Start(WaitForGameObjects()); }
            base.OnSceneWasLoaded(buildIndex, sceneName);
        }
        private IEnumerator WaitForGameObjects()
        {
            Il2Cpp.Craft craft = null;

            // Wait until the object you want is found
            while (craft == null)
            {
                craft = UnityEngine.Object.FindObjectOfType<Il2Cpp.Craft>();
                if (craft == null)
                    // Yield until next frame to check again
                    yield return null;
            }
            // Once the object is found, execute main function
            CreateTrackIRCamera();
        }
        public void CreateTrackIRCamera()
        {
            // Use Camera.main to get the active camera tagged as "MainCamera"
            GameObject mainCamera = GameObject.FindWithTag("MainCamera");

            if (mainCamera == null)
            {
                LoggerInstance.Error("Main camera not found.");
                return;
            }

            // Add TrackIRComponent to the camera
            TrackIRComponent trackIRComponent = mainCamera.gameObject.AddComponent<TrackIRComponent>();
            MelonLogger.Msg("Added TrackIRComponent to main camera!");

            // Set properties
            // trackIRComponent.AssignedApplicationId = 1000;
            // trackIRComponent.TrackingLostTimeoutSeconds = 3.0f;
            // trackIRComponent.TrackingLostRecenterDurationSeconds = 1.0f;
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class XRReferencesManager : MonoBehaviour
{
    public Camera xrCamera;
    public GameObject leftController;
    public GameObject rightController;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Delay slightly in case the XR Rig is initialized a frame later
        StartCoroutine(FindXRReferences());
    }

    private System.Collections.IEnumerator FindXRReferences()
    {
        yield return null; // Wait one frame

        // Try to find the XR Camera
        xrCamera = Camera.main;

        // Alternatively, find using tags or specific names
        leftController = GameObject.Find("Left Controller");
        rightController = GameObject.Find("Right Controller");

        Debug.Log("objects found: camera: " + xrCamera + "  left controller: " + leftController + "  right controller: " + rightController); 

        GazeTracker.Instance.ChangeCamera(xrCamera);
        VRTracker.Instance.ChangeTrackedObjects(xrCamera.transform, leftController.transform, rightController.transform); 

    }
}

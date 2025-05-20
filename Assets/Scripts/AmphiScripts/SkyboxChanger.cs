using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material[] Skyboxes; // Assign this in the Inspector

    void Start()
    {
        //RenderSettings.skybox = newSkybox;

        // Optional: update lighting if needed
        DynamicGI.UpdateEnvironment();
    }

    public void UpdateSkyBox(int index)
    {
        RenderSettings.skybox = Skyboxes[index];
        DynamicGI.UpdateEnvironment();
    }
}

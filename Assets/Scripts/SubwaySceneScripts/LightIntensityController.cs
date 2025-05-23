using UnityEngine;

public class LightIntensityController : MonoBehaviour
{
    public Light[] targetLightsSmallRoom; 
    public Light[] targetLightsMediumRoom;   
    public Light[] targetLightsLargeRoom; 

    private Light[] targetLights;   
    public float intensityStep = 0.2f;    // How much to change per key press
    public float minIntensity = 0f;       // Minimum allowed intensity
    public float maxIntensity = 8f; 
    
    public float minIntensityLargeRoom;
    public float maxIntensityLargeRoom;       // Maximum allowed intensity

    void Start()
    {

    } 
    void Update()
    {
        if (targetLights == null || targetLights.Length == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AdjustLights(intensityStep);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AdjustLights(-intensityStep);
        }
    }

    void AdjustLights(float delta)
    {
        if(targetLights == null)
        {
            targetLights = targetLightsMediumRoom;
        }
        foreach (Light light in targetLights)
        {
            if (light != null)
            {
                light.intensity = Mathf.Clamp(light.intensity + delta, minIntensity, maxIntensity);
                Debug.Log($"{light.name} intensity: {light.intensity}");
            }
        }
    }


    public void SetLightIntensity(float sliderValue)
    {
        float intensity;
        if(RoomManager.currentRoom == 0)
        {
            targetLights = targetLightsSmallRoom;
            intensity = Mathf.Lerp(minIntensity, maxIntensity, sliderValue);
        }
        else if (RoomManager.currentRoom == 1)
        {
            targetLights = targetLightsMediumRoom;
            intensity = Mathf.Lerp(minIntensity, maxIntensity, sliderValue);
        }
        else
        {
            targetLights = targetLightsLargeRoom; 
            intensity = Mathf.Lerp(minIntensityLargeRoom, maxIntensityLargeRoom, sliderValue);
        }

        foreach (Light light in targetLights)
        {
            if (light != null)
            {
                light.intensity = intensity;
            }
        }
    }
}
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CrowdDensityController : MonoBehaviour
{
    public Slider crowdDensitySlider; 
    private SpawnDissolveShader spawnDissolveShader; 
    private float maxValue;
    private float minValue; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(crowdDensitySlider!=null)
        {
            crowdDensitySlider.onValueChanged.AddListener(UpdateCrowdDensity);
        }
        if(crowdDensitySlider!=null)
        {
            minValue = crowdDensitySlider.minValue;
            maxValue = crowdDensitySlider.maxValue;
        }
    }

    // Update is called once per frame
    void UpdateCrowdDensity(float value)
    {
        Debug.Log("slider changed to value: " + value); 
        spawnDissolveShader = GetComponent<SpawnDissolveShader>();
        if (spawnDissolveShader != null)
        {
            spawnDissolveShader.spawnInterval = maxValue - value + 0.1f;
            Debug.Log("changed crowd density to: " + (maxValue - value + 1)); 
        }
    }
}

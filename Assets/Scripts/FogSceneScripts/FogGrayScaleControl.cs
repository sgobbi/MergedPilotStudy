using UnityEngine;
using UnityEngine.UI;

public class FogGrayscaleControl : MonoBehaviour
{
    public ParticleSystem[] fogParticleSystems;
    public Slider grayscaleSlider;

    void Start()
    {
        if (grayscaleSlider != null)
        {
            // Hook up the event
            grayscaleSlider.onValueChanged.AddListener(UpdateFogColor);
            UpdateFogColor(grayscaleSlider.value); //set la valeur de base 
        }
        Debug.Log("number of particle systems: " + fogParticleSystems.Length); 
    }

    void UpdateFogColor(float value)
    {
        // Convert slider value to grayscale color (0 = white, 1 = black)
        float grayscale = 1.0f - value; // inverting: 0 = black, 1 = white
        Color grayColor = new Color(grayscale, grayscale, grayscale, 1f);

        foreach(ParticleSystem particles in fogParticleSystems)
        {
            if (particles != null)
            {
                var main = particles.main;
                main.startColor = grayColor;
            }
        }
        // Apply to Particle System Start Color
        
    }
}

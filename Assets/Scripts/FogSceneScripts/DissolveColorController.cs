using UnityEngine;
using UnityEngine.UI;

public class DissolveColorController : MonoBehaviour
{
    public Slider grayscaleSlider;
    public Material dissolveMaterial; // Assign your material here

    void Start()
    {
        if (grayscaleSlider != null)
        {
            grayscaleSlider.onValueChanged.AddListener(UpdateDissolveColor);
            UpdateDissolveColor(grayscaleSlider.value); // Set default
        }
    }

    void UpdateDissolveColor(float value)
    {
        float grayscale = 1.0f - value; // inverting: 0 = black, 1 = white
        Color grayColor = new Color(grayscale, grayscale, grayscale, 1f);
        dissolveMaterial.SetColor("_DissolveColor", grayColor);
        Debug.Log("changing the dissolvematColor to: " + grayColor); 
    }
}
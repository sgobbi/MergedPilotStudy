using UnityEngine;

public class BoutonTrigger : MonoBehaviour
{
    public int valeur = 3;
    public Color enterColor = Color.green;
    public Color exitColor = Color.white;
    private Renderer rend;

    public QuestionManagerSalome manager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = exitColor; // Set initial color
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object entered trigger: " + other.name);
        if (other.CompareTag("Hand"))
        {
            manager.OnAnswer(valeur);
            rend.material.color = enterColor;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        rend.material.color = exitColor;
        Debug.Log("Object exited trigger: " + other.name);
    }
        
        
}

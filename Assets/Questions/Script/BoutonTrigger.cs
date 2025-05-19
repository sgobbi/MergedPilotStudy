using UnityEngine;

public class BoutonTrigger : MonoBehaviour
{
    public int valeur = 3;  
    public QuestionManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))  
        {
            manager.OnAnswer(valeur);
        }
    }
}

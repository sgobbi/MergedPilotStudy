using UnityEngine;

public class BoutonTrigger : MonoBehaviour
{
    public int valeur = 3;  
    public QuestionManagerSalome manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))  
        {
            manager.OnAnswer(valeur);
        }
    }
}

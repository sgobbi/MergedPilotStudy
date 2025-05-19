using UnityEngine;

public class QKeyListener : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnQKeyPressed();
        }
    }

    void OnQKeyPressed()
    {
        Debug.Log("Q key was pressed!");
        // Call your actual method or logic here
        GeneralExperienceManager.Instance.LoadNextScene(); 
    }
}

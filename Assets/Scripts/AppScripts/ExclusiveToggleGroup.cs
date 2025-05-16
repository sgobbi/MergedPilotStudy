using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ExclusiveToggleGroup : MonoBehaviour
{
    public Toggle[] toggles;
    public GameObject[] environments; 
    public UnityEvent<int> onToggleChanged; // Int represents index of selected toggle
    public Transform[] playerTransforms; 
    public GameObject playerRig; 

    private int currentIndex = -1;

    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i; // Avoid closure issue
            toggles[i].onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    OnToggleSelected(index);
                }
            });
        }

        // Ensure one toggle is always selected (default to the first)
        bool anySelected = false;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                anySelected = true;
                currentIndex = i;
                break;
            }
        }

        if (!anySelected)
        {
            toggles[0].isOn = true;
            currentIndex = 0;
        }
    }

    void OnToggleSelected(int selectedIndex)
    {
        RoomManager.currentRoom = selectedIndex; 
        if (selectedIndex == currentIndex)
            return;

        // Uncheck others
        for (int i = 0; i < toggles.Length; i++)
        {
            if (i != selectedIndex)
            {
                toggles[i].isOn = false;
                //environments[i].SetActive(false);
                
            }
                
            else
            {
                //environments[i].SetActive(true); 
                playerRig.transform.position = playerTransforms[i].position; 
            }
        }

        currentIndex = selectedIndex;     

        onToggleChanged.Invoke(currentIndex);
    }
}

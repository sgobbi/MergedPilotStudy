using UnityEngine;



public class MyObjectSpawner : MonoBehaviour
{
    public GameObject[] prefabObjects; // Array to hold the prefabs
    public float[] desiredObjectSizes;
    public Transform spawnLocation; // Location where the objects will be spawned
    private static GameObject[] localObjects;
    private static Transform localSpawnLocation;
    private static float[] localDesiredObjectSizes;
    private static float spawnHeight;
    public float spawnDistance;
    private static float localSpawnDistance;
    public Camera playerCamera;
    private static Camera localPlayerCamera;

    public Vector3[] objectRotations;
    private static Vector3[] localObjectRotation;

    void Start()
    {
        localObjectRotation = objectRotations;
        spawnHeight = spawnLocation.position.y;
        localSpawnDistance = spawnDistance;
        localSpawnLocation = spawnLocation;
        localDesiredObjectSizes = desiredObjectSizes;
        int arrayLength = prefabObjects.Length;
        localObjects = new GameObject[arrayLength];
        localPlayerCamera = playerCamera;

        for (int i = 0; i < prefabObjects.Length; i++)
        {

            if (prefabObjects[i] == null)
            {
                Debug.LogError("Prefab at index " + i + " is " + prefabObjects[i] + " but it is null.");
            }
            else
            {
                //Debug.Log("Prefab at index " + i + ": " + prefabObjects[i]);
                localObjects[i] = prefabObjects[i];
                //Debug.Log("Local Prefab at index " + i + ": " + localObjects[i]);
            }
        }
    }

    public void SpawnObject(int index)
    {
        if (localPlayerCamera != null)
        {
            Debug.Log("camera is not null ");
        }
        else
        {
            Debug.Log("camera is null");
        }

        if (index >= 0 && index < localObjects.Length)
        {

            Vector3 cameraForward = localPlayerCamera.transform.forward;
            Vector3 cameraPosition = localPlayerCamera.transform.position;
            Vector3 intermediate = cameraForward * localSpawnDistance;
            Vector3 spawnPosition = cameraPosition + cameraForward * localSpawnDistance;
            spawnPosition.y = spawnHeight;
            Debug.Log("spawn distance: " + spawnDistance + "CameraForward: " + cameraForward + " intermediate calcul: " + intermediate);
            Debug.Log("CameraForward: " + cameraForward + "  cameraPosition: " + cameraPosition + "  spawnPosition: " + spawnPosition);
            Vector3 objectRotation = localObjectRotation[index];
            Debug.Log("object rotation: " + objectRotation);

            GameObject spawnedObject = Instantiate(localObjects[index], spawnPosition, Quaternion.Euler(objectRotation));
            Renderer renderer = spawnedObject.GetComponentInChildren<Renderer>();
            Collider collider = spawnedObject.GetComponentInChildren<Collider>();

            if (!spawnedObject.TryGetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>(out _))
            {
                spawnedObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            }

            if (!spawnedObject.TryGetComponent<Rigidbody>(out _))
            {
                Rigidbody rb = spawnedObject.AddComponent<Rigidbody>();
                rb.useGravity = true;
            }

            if (!spawnedObject.TryGetComponent<Collider>(out _))
            {
                spawnedObject.AddComponent<BoxCollider>(); // Or whatever makes sense for the shape
            }


            /* Vector3 objectSize = Vector3.zero;
            if (renderer != null)
            {
                // Get the size of the object
                objectSize = renderer.bounds.size;
                //Debug.Log("object renderer found, object size: " + objectSize + "  objectDesiredSize: " + localDesiredObjectSizes[index]);
                float scaleFactor = localDesiredObjectSizes[index] / objectSize.y;
                spawnedObject.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
                float lowestPoint = renderer.bounds.min.y;
                Vector3 adjustedPosition = spawnedObject.transform.position;
                if (lowestPoint < 0)
                {
                    adjustedPosition.y -= lowestPoint; 
                }
                else
                {
                    adjustedPosition.y += lowestPoint; 
                }
                
                spawnedObject.transform.position = adjustedPosition;
            }
            else if (collider != null)
            {
                objectSize = collider.bounds.size;
                //Debug.Log("object collider found, object size: " + objectSize + "  objectDesiredSize: " + desiredObjectSizes[index]);
                float scaleFactor = localDesiredObjectSizes[index] / objectSize.y;
                spawnedObject.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
            }
            else
            {
                Debug.LogWarning("Renderer component not found on the prefab.");
            } */

        }
        else
        {
            Debug.LogError("Index out of range. Make sure the index is valid.");
        }
    }
    
    public void SpawnObject0()
    {
        SpawnObject(0);
    }
}
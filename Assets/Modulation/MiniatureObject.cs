using UnityEngine;

public class MiniatureObject : MonoBehaviour
{
    public GameObject realPrefab;
    public MiniatureMapper mapper;

    private GameObject realInstance;
    private bool hasSpawned = false;

    void Awake()
    {
        // Recherche automatique du mapper si non assigné
        if (mapper == null)
        {
            mapper = FindObjectOfType<MiniatureMapper>();
            if (mapper == null)
            {
                Debug.LogError($"[MiniatureObject] Aucun MiniatureMapper trouvé dans la scène pour {gameObject.name}");
            }
        }
    }

    void Update()
    {
        if (realInstance != null)
        {
            UpdateRealObject();
        }
        else if (!hasSpawned && realPrefab != null && mapper != null)
        {
            SpawnRealObject();
        }
    }

    public void SpawnRealObject()
    {
        if (realInstance == null && realPrefab != null && mapper != null)
        {
            realInstance = Instantiate(realPrefab);
            hasSpawned = true;
            UpdateRealObject();
        }
    }

    public void DespawnRealObject()
    {
        if (realInstance != null)
        {
            Destroy(realInstance);
            realInstance = null;
            hasSpawned = false;
        }
    }

    private void UpdateRealObject()
    {
        if (mapper == null) return;

        Vector3 localPos = mapper.transform.InverseTransformPoint(transform.position);
        Quaternion localRot = Quaternion.Inverse(mapper.transform.rotation) * transform.rotation;

        realInstance.transform.position = mapper.ConvertMiniatureToWorld(localPos);
        realInstance.transform.rotation = mapper.ConvertMiniatureRotation(localRot);
    }
}
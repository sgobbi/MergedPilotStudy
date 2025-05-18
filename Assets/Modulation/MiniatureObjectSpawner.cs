using UnityEngine;

public class MiniatureObject : MonoBehaviour
{
    public GameObject realPrefab;
    public MiniatureMapper mapper;

    private GameObject realInstance;

    void Update()
    {
        if (realInstance != null)
        {
            UpdateRealObject();
        }
    }

    public void SpawnRealObject()
    {
        if (realInstance == null && realPrefab != null && mapper != null)
        {
            realInstance = Instantiate(realPrefab);
            UpdateRealObject();
        }
    }

    public void DespawnRealObject()
    {
        if (realInstance != null)
        {
            Destroy(realInstance);
            realInstance = null;
        }
    }

    private void UpdateRealObject()
    {
        if (mapper == null) return;

        Vector3 localPos = transform.localPosition;
        Quaternion localRot = transform.localRotation;

        realInstance.transform.position = mapper.ConvertMiniatureToWorld(localPos);
        realInstance.transform.rotation = mapper.ConvertMiniatureRotation(localRot);
    }
}
using UnityEngine;
using System.Collections.Generic;

public class MiniatureSpawnZone : MonoBehaviour
{
    private HashSet<MiniatureObject> objectsInZone = new HashSet<MiniatureObject>();

    private void OnTriggerEnter(Collider other)
    {
        MiniatureObject miniature = other.GetComponent<MiniatureObject>();
        if (miniature != null && !objectsInZone.Contains(miniature))
        {
            objectsInZone.Add(miniature);
            miniature.SpawnRealObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MiniatureObject miniature = other.GetComponent<MiniatureObject>();
        if (miniature != null && objectsInZone.Contains(miniature))
        {
            objectsInZone.Remove(miniature);
            miniature.DespawnRealObject();
        }
    }
}
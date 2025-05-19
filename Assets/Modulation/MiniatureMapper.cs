using UnityEngine;

public class MiniatureMapper : MonoBehaviour
{
    [Tooltip("Transform du point d'origine du monde r�el")]
    public Transform realWorldOrigin;

    // Plus besoin de le d�finir manuellement
    private float scaleFactor;

    void Awake()
    {
        ComputeScaleFactor();
    }

    void ComputeScaleFactor()
    {
        // On suppose que l��chelle est uniforme (m�me sur X, Y, Z)
        float miniatureScale = transform.lossyScale.x;
        float worldScale = realWorldOrigin.lossyScale.x;

        if (worldScale == 0)
        {
            Debug.LogWarning("Le scale du monde r�el est � 0 !");
            scaleFactor = 1f;
        }
        else
        {
            scaleFactor = miniatureScale / worldScale;
        }

        Debug.Log($"[MiniatureMapper] ScaleFactor automatique : {scaleFactor}");
    }

    public Vector3 ConvertMiniatureToWorld(Vector3 localMiniaturePos)
    {
        // Position monde de l�objet miniature
        Vector3 miniatureWorldPos = transform.TransformPoint(localMiniaturePos);

        // Offset depuis le centre de la maquette
        Vector3 offset = miniatureWorldPos - transform.position;

        // Application de l��chelle
        Vector3 scaledOffset = offset / scaleFactor;

        // Position finale dans le monde r�el
        return realWorldOrigin.position + scaledOffset;
    }

    public Quaternion ConvertMiniatureRotation(Quaternion localMiniatureRot)
    {
        return realWorldOrigin.rotation * localMiniatureRot;
    }
}
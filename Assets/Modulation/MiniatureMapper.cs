using UnityEngine;

public class MiniatureMapper : MonoBehaviour
{
    [Tooltip("�chelle entre la maquette et le monde r�el. Exemple : 0.1 pour une maquette 1:10")]
    public float scaleFactor = 0.1f;

    [Tooltip("Transform du point d'origine du monde r�el")]
    public Transform realWorldOrigin;

    public Vector3 ConvertMiniatureToWorld(Vector3 localMiniaturePos)
    {
        // Convertir la position locale de l�objet dans la maquette en position monde
        Vector3 miniatureWorldPos = transform.TransformPoint(localMiniaturePos);

        // Calculer la position relative � la racine miniature
        Vector3 offset = miniatureWorldPos - transform.position;

        // Appliquer l��chelle
        Vector3 scaledOffset = offset / scaleFactor;

        // Position finale dans le monde r�el
        return realWorldOrigin.position + scaledOffset;
    }

    public Quaternion ConvertMiniatureRotation(Quaternion localMiniatureRot)
    {
        // Rotation du monde r�el = rotation d'origine + (rotation locale dans la maquette)
        return realWorldOrigin.rotation * localMiniatureRot;
    }
}
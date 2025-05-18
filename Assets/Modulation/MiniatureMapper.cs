using UnityEngine;

public class MiniatureMapper : MonoBehaviour
{
    [Tooltip("Échelle entre la maquette et le monde réel. Exemple : 0.1 pour une maquette 1:10")]
    public float scaleFactor = 0.1f;

    [Tooltip("Transform du point d'origine du monde réel")]
    public Transform realWorldOrigin;

    public Vector3 ConvertMiniatureToWorld(Vector3 localMiniaturePos)
    {
        // Convertir la position locale de l’objet dans la maquette en position monde
        Vector3 miniatureWorldPos = transform.TransformPoint(localMiniaturePos);

        // Calculer la position relative à la racine miniature
        Vector3 offset = miniatureWorldPos - transform.position;

        // Appliquer l’échelle
        Vector3 scaledOffset = offset / scaleFactor;

        // Position finale dans le monde réel
        return realWorldOrigin.position + scaledOffset;
    }

    public Quaternion ConvertMiniatureRotation(Quaternion localMiniatureRot)
    {
        // Rotation du monde réel = rotation d'origine + (rotation locale dans la maquette)
        return realWorldOrigin.rotation * localMiniatureRot;
    }
}
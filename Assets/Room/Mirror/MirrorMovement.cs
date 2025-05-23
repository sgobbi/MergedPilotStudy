using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{

    public Transform playerTarget;
    public Transform mirror;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localplayer = mirror.InverseTransformPoint(playerTarget.position);
        transform.position = mirror.TransformPoint(new Vector3(localplayer.x, localplayer.y, -localplayer.z));

        Vector3 lookatmirror = mirror.TransformPoint(new Vector3(localplayer.x, -localplayer.y, localplayer.z));
        transform.LookAt(lookatmirror);

    }
}

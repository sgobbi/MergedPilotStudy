using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbedObjectRotator : MonoBehaviour
{
    public XRNode rotationControllerNode = XRNode.LeftHand;  // Controller to use for joystick input
    public float rotationSpeed = 60f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;

    private float totalXRotation = 0f;
    private float totalZRotation = 0f;

    public Transform meshTransform;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void Start()
{
    if (meshTransform == null)
        meshTransform = GetComponentInChildren<Renderer>().transform;
}

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    void Update()
    {
        if (!isGrabbed) return;

        InputDevice device = InputDevices.GetDeviceAtXRNode(rotationControllerNode);
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 input))
        {
            Debug.Log("this is the input: " + input);
            float xDelta = -input.y * rotationSpeed * Time.deltaTime;  // Up/Down joystick -> rotate around X
            float zDelta = input.x * rotationSpeed * Time.deltaTime;   // Left/Right joystick -> rotate around Z

            //meshTransform.Rotate(Vector3.right, xDelta, Space.World);
            //meshTransform.Rotate(Vector3.forward, zDelta, Space.World);

            transform.Rotate(Vector3.right, xDelta, Space.World);
            transform.Rotate(Vector3.forward, zDelta, Space.World);
        }
    }
}

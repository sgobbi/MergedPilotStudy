using UnityEngine;
using UnityEngine.XR;

public class JoystickObjectMover : MonoBehaviour
{
    [Header("Controller Configuration")]
    public XRNode movementControllerNode = XRNode.RightHand;  // Joystick for movement
    public XRNode rotationControllerNode = XRNode.LeftHand;   // Joystick for rotation

    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;

    [Header("Rotation Settings")]
    public float rotateSpeed = 60f;
    public float pitchMin = -80f;
    public float pitchMax = 80f;

    [Header("Visual Feedback")]
    public Material highlightMaterial;

    private bool isSelected = false;
    private bool isBeingMoved = false;

    private Vector2 moveInput;
    private Vector2 rotateInput;

    private Rigidbody rb;
    private Renderer rend;
    private Material originalMaterial;
    private Transform playerHead;

    // Tracked rotation
    private float totalYaw = 0f;
    private float totalPitch = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;

        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
            originalMaterial = rend.material;

        playerHead = Camera.main.transform;
    }

    void Update()
    {
        InputDevice moveDevice = InputDevices.GetDeviceAtXRNode(movementControllerNode);
        InputDevice rotateDevice = InputDevices.GetDeviceAtXRNode(rotationControllerNode);

        // Start movement on trigger
        if (moveDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed) && triggerPressed)
        {
            if (!isBeingMoved && isSelected)
                StartMoving();
        }

        // Stop movement on trigger release
        if (isBeingMoved && moveDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerReleased) && !triggerReleased)
        {
            StopMoving();
        }

        if (isBeingMoved)
        {
            // Handle movement
            if (moveDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out moveInput))
            {
                Vector3 forward = playerHead.forward;
                forward.y = 0;
                forward.Normalize();

                Vector3 right = playerHead.right;
                right.y = 0;
                right.Normalize();

                Vector3 movement = (forward * moveInput.y + right * moveInput.x) * moveSpeed * Time.deltaTime;
                transform.position += movement;
            }

            // Handle rotation
            if (rotateDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out rotateInput))
            {
                float yawDelta = rotateInput.x * rotateSpeed * Time.deltaTime;
                float pitchDelta = -rotateInput.y * rotateSpeed * Time.deltaTime;

                totalYaw += yawDelta;
                totalPitch += pitchDelta;
                totalPitch = Mathf.Clamp(totalPitch, pitchMin, pitchMax);

                Quaternion yawRotation = Quaternion.AngleAxis(totalYaw, Vector3.up);
                Quaternion pitchRotation = Quaternion.AngleAxis(totalPitch, Vector3.right);

                transform.rotation = yawRotation * pitchRotation;
            }
        }
    }

    private void StartMoving()
    {
        isBeingMoved = true;
        rb.useGravity = false;
        rb.isKinematic = true;

        if (highlightMaterial && rend)
            rend.material = highlightMaterial;
    }

    private void StopMoving()
    {
        isBeingMoved = false;
        rb.useGravity = true;
        rb.isKinematic = false;

        if (rend && originalMaterial)
            rend.material = originalMaterial;
    }

    public void Select()
    {
        isSelected = true;

        if (highlightMaterial && rend)
            rend.material = highlightMaterial;
    }

    public void Deselect()
    {
        isSelected = false;

        if (!isBeingMoved && originalMaterial && rend)
            rend.material = originalMaterial;
    }
}

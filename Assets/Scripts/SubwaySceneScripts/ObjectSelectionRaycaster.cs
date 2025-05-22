using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectSelectionRaycaster : MonoBehaviour
{
    public XRNode inputSource = XRNode.RightHand;
    public LayerMask selectableLayer;
    public float maxDistance = 10f;

    private JoystickObjectMover currentSelected;

    void Update()
    {
        // Check trigger press
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
        {
            if (triggerPressed)
            {
                TrySelectObject();
            }
            else if (currentSelected != null)
            {
                currentSelected.Deselect();
                currentSelected = null;
            }
        }
    }

    void TrySelectObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, selectableLayer))
        {
            JoystickObjectMover mover = hit.collider.GetComponentInParent<JoystickObjectMover>();
            if (mover != null && mover != currentSelected)
            {
                if (currentSelected != null)
                {
                    currentSelected.Deselect();
                }

                currentSelected = mover;
                mover.Select();
            }
        }
        else if (currentSelected != null)
        {
            currentSelected.Deselect();
            currentSelected = null;
        }
    }
}

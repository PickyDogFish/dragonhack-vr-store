using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Input;

public class Controller : MonoBehaviour {
    // Start is called before the first frame update
    private bool inArea = false;
    private bool grabbing = false;
    private Grabable lastGrabable;
    public InputActionReference hapticAction;
    private InputDevice controllerDevice;

    private void OnTriggerEnter(Collider other) {
        Grabable grabable = other.GetComponent<Grabable>();
        if (grabable != null) {
            inArea = true;
            if (!grabbing)
                lastGrabable = grabable;
            //grabable.OnGrab(gameObject);
        }
    }


    private void OnTriggerExit(Collider other) {
        Grabable grabable = other.GetComponent<Grabable>();
        if (grabable != null) {
            inArea = false;
            //grabable.OnRelease(gameObject);
        }
    }

    public void OnGripPressed(InputAction.CallbackContext ctx) {
        if (ctx.performed && inArea) {
            grabbing = true;
            lastGrabable.OnGrab(gameObject);
            HapticImpulse(0.08f, 0.05f);
        } 
        if (ctx.canceled && grabbing) {
            grabbing = false;
            lastGrabable.OnRelease(gameObject);
        }
    }

    public void OnTriggerChange(InputAction.CallbackContext ctx){
        if (grabbing){
            lastGrabable.OnTriggerMove(ctx.ReadValue<float>());
        }
    }

    public void HapticImpulse(float intensity, float duration, float frequency = 0) {
        if (controllerDevice == null)
            FindAssignedDevice();
        OpenXRInput.SendHapticImpulse(hapticAction, intensity, frequency, duration, controllerDevice);
    }

    private void FindAssignedDevice() {
        string target = "";
        if (hapticAction.action.bindings[0].path.Contains("Left")) {
            target = "LeftHand";
        } else if (hapticAction.action.bindings[0].path.Contains("Right")) {
            target = "RightHand";
        }
        foreach (InputDevice device in InputSystem.devices)
            if (device.usages.Count != 0 && device.usages[0].Equals(target))
                controllerDevice = device;
    }
}

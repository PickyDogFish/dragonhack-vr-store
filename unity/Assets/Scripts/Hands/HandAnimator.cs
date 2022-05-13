using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class HandAnimator : MonoBehaviour
{
    [SerializeField]
    private InputActionReference triggerAction, gripAction;
    [SerializeField]
    private Animator handAnimator;
    
    void Update() {
        handAnimator.SetFloat("Trigger", triggerAction.action.ReadValue<float>());
        handAnimator.SetFloat("Grip", gripAction.action.ReadValue<float>());
    }
}

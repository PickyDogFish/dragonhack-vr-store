using UnityEngine;
using System;

public class CategorySelector : Grabable {
    [Range(0, 0.3f)]
    [SerializeField]
    private float offsetRange;
    [Range(0, 1)]
    [SerializeField]
    private float deadzone = 0.05f;
    [SerializeField]
    private int categoryCount = 3;
    private float center;
    private GameObject currentlyHeldHand = null;
    private float posOnGrabbed;
    private float handPosOnGrabbed;

    void Start() {
        center = transform.localPosition.y;
    }

    void Update() {
        if (currentlyHeldHand != null) {
            // Calculate up/down position
            Vector3 handCurrentPos = currentlyHeldHand.transform.parent.localPosition;
            float yPosUnclamped = posOnGrabbed + (handCurrentPos.y - handPosOnGrabbed);
            float yPos = Mathf.Clamp(yPosUnclamped, center - offsetRange, center + offsetRange);
            // Apply the newly calculated position
            Vector3 localPos = transform.localPosition;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(localPos.x, yPos, localPos.z), 0.5f);
        }
    }

    public override void OnGrab(GameObject hand) {
        if (currentlyHeldHand == null) {
            currentlyHeldHand = hand;
            posOnGrabbed = transform.localPosition.y;
            handPosOnGrabbed = hand.transform.parent.localPosition.y;
        }
    }

    public override void OnRelease(GameObject hand) {
        if (currentlyHeldHand == hand) {
            currentlyHeldHand = null;
            // Clipping
            float betweenTwo = 1.0f / (categoryCount - 1);
            int offsetAmount = (int)Math.Round(GetValue() / betweenTwo);
            float finalY = Remap(betweenTwo * offsetAmount, 0, 1, center - offsetRange, center + offsetRange);
            Vector3 localPos = transform.localPosition;
            transform.localPosition = new Vector3(localPos.x, finalY, localPos.z);
        }
    }

    public int GetSelectedCategory() {
        float betweenTwo = 1.0f / (categoryCount - 1);
        return (int)Math.Round((1 - GetValue()) / betweenTwo);
    }

    private float GetValue() {
        return Remap(transform.localPosition.y, center - offsetRange, center + offsetRange, 0, 1);
    }

    private static float Remap(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

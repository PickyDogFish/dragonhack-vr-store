using UnityEngine;

public abstract class Grabable : MonoBehaviour {
    /// <summary>
    /// Called when the grabable gets grabbed.
    /// </summary>
    /// <param name="hand">The controller gameobject that grabbed the grabable</param>
    public abstract void OnGrab(GameObject hand);
    /// <summary>
    /// Called when the grabable gets released.
    /// </summary>
    /// <param name="hand">The controller gameobject that was holding the grabable</param>
    public abstract void OnRelease(GameObject hand);
    /// <summary>
    /// Called whenever the grabable is being held and the trigger on the controller moves.
    /// Not abstract, as not all grabables require this information.
    /// </summary>
    /// <param name="currentPosition">The new trigger value</param>
    public virtual void OnTriggerMove(float currentPosition) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : Grabable
{
    //Slidiness, more means more slidy after letting go
    [SerializeField]
    private float slidiness = 0.96f;
    
    // A vector that defines the direction
    // in which the object can slide
    [SerializeField]
    private Vector3 slideDirection;

    private bool slidingAway = false;
    private GameObject holdingHand = null;

    private Vector3 handGrabPos;
    private Vector3 sliderGrabLocalPos;
    private Vector3 handLastFrameMove = Vector3.zero;
    private Vector3 lastPosChange = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        slideDirection = slideDirection.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (!slidingAway){
            if (holdingHand != null){
                Vector3 handMove = holdingHand.transform.position - handGrabPos;
                Vector3 projected = Vector3.Project(handMove, slideDirection);
                gameObject.transform.localPosition = sliderGrabLocalPos + projected;
                lastPosChange = Vector3.Project(handMove - handLastFrameMove, slideDirection);
                handLastFrameMove = handMove;
            } else {
                //Move according to inertia
                lastPosChange = scaleInertia(lastPosChange);
                gameObject.transform.localPosition += lastPosChange;
            }
        }
    }

    public override void OnGrab(GameObject obj){
        if (holdingHand == null){
            holdingHand = obj;
            handGrabPos = obj.transform.position;
            sliderGrabLocalPos = gameObject.transform.localPosition;
        }
    }

    public override void OnRelease(GameObject obj){
        if (holdingHand == obj){
            holdingHand = null;
        }
    }

    private Vector3 scaleInertia(Vector3 speed){
        return Vector3.Lerp(Vector3.zero,lastPosChange, slidiness);
    }

    public void slideAway(){
        slidingAway = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : Grabable
{
    //ref to next and previous closet in the category
    public Slider nextCloset;
    public Slider previousCloset;
    //Slidiness, more means more slidy after letting go
    [SerializeField]
    private float slidiness = 0.96f;

    // A vector that defines the direction in which the object can slide
    public Vector3 slideDirection;
    private GameObject holdingHand = null;
    private Vector3 handGrabPos;
    private Vector3 sliderGrabLocalPos;
    private Vector3 handLastFrameMove = Vector3.zero;
    private Vector3 lastPosChange = Vector3.zero;

    //sliding the closet in vars
    private bool slideIn = false;
    private bool slideInFromLeft = true;

    //sliding the closet out vars
    private bool slideOut = false;
    private bool slideOutFromLeft = true;

    //stop locations
    private Vector3 midStopLocation = Vector3.zero;
    public Vector3 startStopLocation;
    public Vector3 endStopLocation;
    

    void Start()
    {
        slideDirection = slideDirection.normalized;
        startStopLocation = slideDirection * -10;
        endStopLocation = slideDirection * 10;
    }

    void Update()
    {
        if (!slideIn && !slideOut){
            if (holdingHand != null){

                Vector3 handMove = holdingHand.transform.position - handGrabPos;
                Vector3 projected = Vector3.Project(handMove, slideDirection);
                gameObject.transform.localPosition = sliderGrabLocalPos + projected;
                lastPosChange = Vector3.Project(handMove - handLastFrameMove, slideDirection);
                handLastFrameMove = handMove;
            } else {
                //if true we are switching the closet
                if (lastPosChange.magnitude > 0.028f){
                    if (Vector3.Dot(lastPosChange.normalized, slideDirection.normalized) == 1){
                        Debug.Log("sliding true");
                        setSlideOut(true);
                        // setAutoSlidePos(gameObject.transform.localPosition + Vector3.Project(new Vector3(10,10,10), slideDirection), 0.1f, outDir);
                    } else {
                        Debug.Log("sliding false");
                        setSlideOut(false);
                        // setAutoSlidePos(gameObject.transform.localPosition - Vector3.Project(new Vector3(10,10,10), slideDirection), 0.1f, outDir);
                    }
                    lastPosChange = Vector3.zero;
                //not switching, letting it slide
                } else {
                    lastPosChange = scaleSlide();
                    gameObject.transform.localPosition += lastPosChange;
                }
            }
        } else {
            if (slideIn){
                autoSlideIn(0.05f);
            } else if (slideOut){
                autoSlideOut(0.08f);
                if (slideOutFromLeft){
                    nextCloset.setSlideIn(true);
                } else {
                    previousCloset.setSlideIn(false);
                }
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

    private Vector3 scaleSlide(){
        return Vector3.Lerp(Vector3.zero, lastPosChange, slidiness);
    }

    public void setSlideOut(bool fromLeft){
        slideOut = true;
        slideOutFromLeft = fromLeft;
    }

    public void setSlideIn(bool fromLeft){
        slideIn = true;
        slideInFromLeft = fromLeft;
        if (fromLeft){
            gameObject.transform.localPosition = startStopLocation;
        } else {
            gameObject.transform.localPosition = endStopLocation;
        }
    }

    //Slides in from current position towards autoSlideTo
    //With speed given by lerp value
    //if direction true slide to right, false slide to left
    private void autoSlideOut(float speed){
        Vector3 goal;
        if (slideOutFromLeft){
            goal = endStopLocation;
        } else {
            goal = startStopLocation;
        }

        Vector3 slideDist = gameObject.transform.localPosition - goal;
        if (gameObject.transform.localPosition.magnitude > goal.magnitude){
            slideOut = false;
        } else {
            // gameObject.transform.localPosition += Vector3.Project(new Vector3(1,1,1), slideDirection).normalized * speed;
            if (slideOutFromLeft){
                gameObject.transform.localPosition += Vector3.Project(new Vector3(1,1,1), slideDirection).normalized * speed;
            } else {
                gameObject.transform.localPosition -= Vector3.Project(new Vector3(1,1,1), slideDirection).normalized * speed;
            }
        }
    }

    //if direction true slide left to right, false slide right to left
    private void autoSlideIn(float speed){
        Vector3 dist = midStopLocation - gameObject.transform.localPosition;
        if (dist.magnitude < 0.01){
            slideIn = false;
        } else {
            // gameObject.transform.localPosition += Vector3.Lerp(Vector3.zero, dist, speed);
            if (!slideInFromLeft){
                gameObject.transform.localPosition += Vector3.Lerp(Vector3.zero, dist, speed);
            } else {
                gameObject.transform.localPosition += Vector3.Lerp(Vector3.zero, dist, speed);
            }
        }
    }
}

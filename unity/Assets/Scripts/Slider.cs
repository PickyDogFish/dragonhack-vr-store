using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : Grabable
{
    //ref to next and previous closet in the category
    public GameObject nextCloset;
    public GameObject previousCloset;


    //Slidiness, more means more slidy after letting go
    [SerializeField]
    private float slidiness = 0.96f;
    
    // A vector that defines the direction
    // in which the object can slide
    [SerializeField]
    private Vector3 slideDirection;

    //true when sliding in or out
    private bool autoSliding = false;
    private bool autoSlideDir = true;
    private Vector3 autoSlideTo = Vector3.zero;
    private float autoSlideSpeed;


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
        if (!autoSliding){
            if (holdingHand != null){
                Vector3 handMove = holdingHand.transform.position - handGrabPos;
                Vector3 projected = Vector3.Project(handMove, slideDirection);
                gameObject.transform.localPosition = sliderGrabLocalPos + projected;
                lastPosChange = Vector3.Project(handMove - handLastFrameMove, slideDirection);
                handLastFrameMove = handMove;
            } else {
                //Move according to inertia
                if (lastPosChange.magnitude > 0.039f){
                    if (Vector3.Dot(lastPosChange.normalized, slideDirection.normalized) == 1){
                        setAutoSlidePos(gameObject.transform.localPosition + Vector3.Project(new Vector3(10,10,10), slideDirection), 0.1f);
                        autoSlideDir = true;
                    } else {
                        setAutoSlidePos(gameObject.transform.localPosition - Vector3.Project(new Vector3(10,10,10), slideDirection), 0.1f);
                        autoSlideDir = false;
                    }
                } else {
                    lastPosChange = scaleSlide();
                    gameObject.transform.localPosition += lastPosChange;
                }
            }
        } else {
            autoSlideOut(autoSlideSpeed, autoSlideDir);
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

    public void setAutoSlidePos(Vector3 pos, float speed){
        Debug.Log("autosliding set");
        autoSlideTo = pos;
        autoSliding = true;
        autoSlideSpeed = speed;
    }

    //Slides in from current position towards autoSlideTo
    //With speed given by lerp value
    //if direction true slide to right, false slide to left
    private void autoSlideOut(float speed, bool direction){
        Vector3 slideDist = gameObject.transform.localPosition - autoSlideTo;
        if (slideDist.magnitude < 0.001){
            autoSliding = false;
        } else {
            gameObject.transform.localPosition += Vector3.Project(new Vector3(1,1,1), slideDirection).normalized * speed;
        }
    }

    //if direction true slide to right, false slide to left
    private void autoSlideIn(float speed, bool direction){
        if (direction){
            Vector3 dist = autoSlideTo - gameObject.transform.localPosition;
            gameObject.transform.localPosition += Vector3.Lerp(Vector3.zero, dist, speed);
        } else {
            Vector3 dist = gameObject.transform.localPosition - autoSlideTo;
            gameObject.transform.localPosition += Vector3.Lerp(Vector3.zero, dist, speed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Grabable
{
    private GameObject holdingHand = null;
    private Transform originalParent;
    // Start is called before the first frame update
    void Start()
    {
        originalParent = gameObject.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnGrab(GameObject obj){
        if (holdingHand == null){
            Debug.Log("object grabbed");
            holdingHand = obj;
            gameObject.transform.parent = obj.transform;
        }
    }

    public override void OnRelease(GameObject obj){
        if (holdingHand == obj){
            holdingHand = null;
            gameObject.transform.parent = originalParent;
        }
    }
}

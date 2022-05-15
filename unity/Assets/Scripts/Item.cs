using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Grabable
{
    private GameObject holdingHand = null;
    private Transform originalParent;
    public Product product;
    
    void Start()
    {
        originalParent = gameObject.transform.parent;
    }

    void Update()
    {
        
    }

    public override void OnGrab(GameObject obj){
        if (holdingHand == null){
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

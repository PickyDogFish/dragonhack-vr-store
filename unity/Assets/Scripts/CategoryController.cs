using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawns the necesarry amount of closets
public class CategoryController : MonoBehaviour
{
    [SerializeField]
    private GameObject shirtCloset;
    private GameObject beanieCloset;
    private int itemsPerCloset = 8;

    //list of instanced closets in the category
    private List<GameObject> closetList;

    //index of the closet the player is currently seeing
    private int curCloset = -1;
    // Start is called before the first frame update

    private Vector3 spawnPos = new Vector3();
    void Start()
    {
        //make a request to db for objects of a category
        //figure out the amount of objects you have to spawn
        int numOfItems = 15;

        //figure out the closet type of the category
        string category = "shirt";

        GameObject closet;
        if (category == "shirt"){
            closet = shirtCloset;
        } else {
            closet = beanieCloset;
        }

        int numOfClosets = Mathf.CeilToInt(numOfItems/itemsPerCloset);
        for (int i = 0; i < numOfClosets; i++)
        {
            closetList.Add(Instantiate(closet, spawnPos, Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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
        StartCoroutine(DataHandler.GetCategories(modelSetup));

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

        setNextPreviousCloset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setNextPreviousCloset(){
        Slider first = closetList[0].GetComponent<Slider>();
        Slider last = closetList[closetList.Count-1].GetComponent<Slider>();
        first.nextCloset = closetList[0].GetComponent<Slider>();
        first.previousCloset = last;
        last.previousCloset = closetList[closetList.Count - 2].GetComponent<Slider>();
        last.nextCloset = first;


        for (int i = 1; i < closetList.Count - 1; i++)
        {
            GameObject closet = closetList[i];
            Slider slider = closet.GetComponent<Slider>();
            slider.nextCloset = closetList[i+1].GetComponent<Slider>();
            slider.previousCloset = closetList[i-1].GetComponent<Slider>();
        }
    }

    void modelSetup(Category[] categories){
        Debug.Log(categories[1].Name);
    }
}

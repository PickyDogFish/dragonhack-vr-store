using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawns the necesarry amount of closets
public class CategoryController : MonoBehaviour
{
    [SerializeField]
    private GameObject shirtCloset;
    [SerializeField]
    private GameObject beanieCloset;
    private Vector3 slideDirection = new Vector3(1,0,0);
    private int itemsPerCloset = 8;

    //list of instanced closets in the category
    private List<GameObject> closetList = new List<GameObject>();

    //index of the closet the player is currently seeing
    private int curCloset = -1;
    // Start is called before the first frame update

    [SerializeField]
    private Vector3 spawnPos = new Vector3(-5,0,0);
    void Start()
    {
        //make a request to db for objects of a category
        //figure out the amount of objects you have to spawn
        StartCoroutine(DataHandler.GetCategories(modelSetup));

        int numOfItems = 20;

        //figure out the closet type of the category
        string category = "shirt";

        GameObject closet;
        if (category == "shirt"){
            closet = shirtCloset;
        } else {
            closet = beanieCloset;
        }

        int numOfClosets = Mathf.CeilToInt(numOfItems/itemsPerCloset);
        Debug.Log((float)numOfItems/itemsPerCloset);
        for (int i = 0; i < numOfClosets; i++)
        {
            GameObject c = Instantiate(closet, spawnPos, Quaternion.identity);
            c.GetComponent<Slider>().slideDirection = slideDirection;
            closetList.Add(c);
        }
        reparentClosets();
        if (closetList.Count > 1){
            setNextPreviousCloset();
        }
        closetList[0].GetComponent<Slider>().setSlideIn(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setNextPreviousCloset(){
        Slider first = closetList[0].GetComponent<Slider>();
        Slider last = closetList[closetList.Count-1].GetComponent<Slider>();
        first.nextCloset = closetList[1].GetComponent<Slider>();
        first.previousCloset = last;
        last.previousCloset = closetList[closetList.Count - 2].GetComponent<Slider>();
        last.nextCloset = first;


        for (int i = 1; i < closetList.Count - 2; i++)
        {
            GameObject closet = closetList[i];
            Slider slider = closet.GetComponent<Slider>();
            slider.nextCloset = closetList[i+1].GetComponent<Slider>();
            slider.previousCloset = closetList[i-1].GetComponent<Slider>();
        }
    }

    void reparentClosets(){
        foreach (GameObject closet in closetList)
        {
            Debug.Log("setting parent");
            closet.transform.parent = gameObject.transform;
            closet.transform.localPosition = spawnPos;
        }
    }

    void modelSetup(Category[] categories){
        Debug.Log(categories[1].Name);
    }
}

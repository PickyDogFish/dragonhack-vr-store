using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawns the necesarry amount of closets
public class CategoryController : MonoBehaviour
{
    [SerializeField]
    private ModelCustomiser modelCustomiser;
    //Closet prefabs
    [SerializeField]
    private GameObject shirtCloset;
    [SerializeField]
    private GameObject beanieCloset;

    //closet slide direction
    private Vector3 slideDirection = new Vector3(1,0,0);
    private int itemsPerCloset = 8;

    private int itemCount = 0;

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
        Category category = new Category();
        category.id = 2;
        StartCoroutine(DataHandler.GetProducts(category, loadModels));


        
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

    void setClosetsModelCustomizer(){
        foreach (GameObject closet in closetList)
        {
            closet.GetComponent<shirtClosetItemManager>().modelCustomiser = modelCustomiser;
        }
    }

    void reparentClosets(){
        foreach (GameObject closet in closetList)
        {
            closet.transform.parent = gameObject.transform;
            closet.transform.localPosition = spawnPos;
        }
    }

    void loadModels(Product[] products){
        itemCount = products.Length;
        Debug.Log(itemCount);
        //figure out the closet type of the category
        string cat = "shirt";

        GameObject closet;
        if (cat == "shirt"){
            closet = shirtCloset;
            itemsPerCloset = 6;
        } else {
            closet = beanieCloset;
        }

        int numOfClosets = Mathf.CeilToInt(itemCount/itemsPerCloset);
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
        setClosetsModelCustomizer();

        for (int i = 0; i < closetList.Count; i++)
        {
            GameObject tempCloset = closetList[i];
            if (products.Length >= i* itemsPerCloset + 6){
                
                Debug.Log(itemsPerCloset);
                Product[] closetItems = new Product[6];
                Array.Copy(products, i* itemsPerCloset, closetItems, 0, 6);
                tempCloset.GetComponent<shirtClosetItemManager>().loadModelData(closetItems);
                Debug.Log("post");
            } else {
                // not enough products to fill up the whole closet
                // int productsLeft = products.Length - i * itemsPerCloset;
                // Debug.Log("stuff and things");
                // Product[] closetItems = new Product[productsLeft];
                // Array.Copy(products, i* itemsPerCloset, closetItems, 0, productsLeft);
                // tempCloset.GetComponent<shirtClosetItemManager>().loadModelData(closetItems);
                // break;
            }
        }
        

        closetList[0].GetComponent<Slider>().setSlideIn(true);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawns the necesarry amount of closets
public class CategoryController : MonoBehaviour {
    [SerializeField]
    private ModelCustomiser modelCustomiser;
    //Closet prefabs
    [SerializeField]
    private GameObject shirtCloset;
    [SerializeField]
    private GameObject beanieCloset;
    [SerializeField]
    private GameObject customCloset;

    //closet slide direction
    private Vector3 slideDirection = new Vector3(1, 0, 0);
    private int itemsPerCloset = 8;

    private int itemCount = 0;

    //list of instanced closets in the category
    private List<GameObject> closetList = new List<GameObject>();

    //index of the closet the player is currently seeing
    private int curCloset = -1;

    [SerializeField]
    private Vector3 spawnPos = new Vector3(-5, 0, 0);
    private Category category;
    void Start() {
    }

    public void loadProducts(Category categoryToLoad){
        unloadCategory();
        category = categoryToLoad;
        Debug.Log(category.id);
        StartCoroutine(DataHandler.GetProducts(category, loadModels));
    }

    void setNextPreviousCloset() {
        Slider first = closetList[0].GetComponent<Slider>();
        Slider last = closetList[closetList.Count - 1].GetComponent<Slider>();
        first.nextCloset = closetList[1].GetComponent<Slider>();
        first.previousCloset = last;
        last.previousCloset = closetList[closetList.Count - 2].GetComponent<Slider>();
        last.nextCloset = first;


        for (int i = 1; i < closetList.Count - 1; i++) {
            GameObject closet = closetList[i];
            Slider slider = closet.GetComponent<Slider>();
            slider.nextCloset = closetList[i + 1].GetComponent<Slider>();
            slider.previousCloset = closetList[i - 1].GetComponent<Slider>();
        }
    }

    void setClosetsModelCustomizer() {
        foreach (GameObject closet in closetList) {
            closet.GetComponent<ShirtClosetItemManager>().modelCustomiser = modelCustomiser;
        }
    }

    void reparentClosets() {
        foreach (GameObject closet in closetList) {
            closet.transform.parent = gameObject.transform;
            closet.transform.localPosition = spawnPos;
        }
    }

    void loadModels(Product[] products) {
        Debug.Log("loading models");
        Debug.Log(category.Name);
        Debug.Log(products.Length);
        GameObject closet;
        if (category.DrawerType == "hangers") {
            closet = shirtCloset;
            itemsPerCloset = 6;
        } else if (category.DrawerType == "shelves"){
            closet = beanieCloset;
            itemsPerCloset = 8;
        } else {
            closet = customCloset;
        }

        itemCount = products.Length;
        int numOfClosets = Mathf.CeilToInt((float)itemCount / itemsPerCloset);
        for (int i = 0; i < numOfClosets; i++) {
            GameObject c = Instantiate(closet, spawnPos, Quaternion.identity);
            c.GetComponent<Slider>().slideDirection = slideDirection;
            closetList.Add(c);
        }
        reparentClosets();
        if (closetList.Count > 1) {
            setNextPreviousCloset();
        }
        setClosetsModelCustomizer();

        for (int i = 0; i < closetList.Count; i++) {
            GameObject tempCloset = closetList[i];
            if (products.Length >= i * itemsPerCloset + itemsPerCloset) {
                Product[] closetItems = new Product[itemsPerCloset];
                Array.Copy(products, i * itemsPerCloset, closetItems, 0, itemsPerCloset);
                tempCloset.GetComponent<ShirtClosetItemManager>().loadModelData(closetItems);
            } else {
                // not enough products to fill up the whole closet
                int productsLeft = products.Length - i * itemsPerCloset;
                Product[] closetItems = new Product[productsLeft];
                Array.Copy(products, i * itemsPerCloset, closetItems, 0, productsLeft);
                tempCloset.GetComponent<ShirtClosetItemManager>().loadModelData(closetItems);
                break;
            }
        }

        closetList[0].GetComponent<Slider>().setSlideIn(true);
    }

    void unloadCategory(){
        while (closetList.Count > 0)
        {
            GameObject closetToBeDestroyed = closetList[0];
            closetList.RemoveAt(0);
            Destroy(closetToBeDestroyed);
        }
    }
}

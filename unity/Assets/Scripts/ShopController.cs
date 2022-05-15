using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private CategoryController categoryController;
    private List<Category> categoryList = new List<Category>();
    [SerializeField]
    private CategorySelector categorySelector;
    private int currentCategory = 0;
    private bool firstLoad = true;
    void Start()
    {
        StartCoroutine(DataHandler.GetCategories(OnCategoriesReceived));
    }

    void Update()
    {   
        if (categoryList.Count > 0){
            int selectedCategory = categorySelector.GetSelectedCategory();
            if (selectedCategory != currentCategory){
                currentCategory = selectedCategory;
                categoryController.loadProducts(categoryList[currentCategory]);
            }
        } 
    }

    void OnCategoriesReceived(Category[] categories){
        categoryList.AddRange(categories);
        categorySelector.categoryCount = categoryList.Count;
    }
}

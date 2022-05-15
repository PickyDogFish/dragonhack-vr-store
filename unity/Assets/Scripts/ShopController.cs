using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
    [SerializeField]
    private CategoryController categoryController;
    private List<Category> categoryList = new List<Category>();
    [SerializeField]
    private CategorySelector categorySelector;
    [SerializeField]
    private GameObject textUIprefab, textCanvas;
    private int currentCategory = 0;
    private bool firstLoad = true;
    void Start() {
        StartCoroutine(DataHandler.GetCategories(OnCategoriesReceived));
    }

    // Update is called once per frame
    void Update() {
        if (categoryList.Count > 0) {
            int selectedCategory = categorySelector.GetSelectedCategory();
            if (selectedCategory != currentCategory) {
                currentCategory = selectedCategory;
                categoryController.loadProducts(categoryList[currentCategory]);
            }
        }
    }

    void OnCategoriesReceived(Category[] categories) {
        categoryList.AddRange(categories);
        categorySelector.categoryCount = categoryList.Count;
        foreach (Category category in categories) {
            GameObject newText = Instantiate(textUIprefab);
            newText.GetComponentInChildren<TMP_Text>().text = category.Name;
            newText.transform.parent = textCanvas.transform;
            newText.transform.localPosition = Vector3.zero;
            newText.transform.localRotation = Quaternion.Euler(0,0,0);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)textCanvas.transform);
    }
}

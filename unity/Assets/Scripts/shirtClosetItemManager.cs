using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shirtClosetItemManager : MonoBehaviour
{
    public ModelCustomiser modelCustomiser;
    private Dictionary<int, Transform> productEmptyTransformMap = new Dictionary<int, Transform>();
    private Dictionary<int, TextMeshProUGUI> productTextMap = new Dictionary<int, TextMeshProUGUI>(); 

    [SerializeField]
    private Transform emptyParent;

    [SerializeField]
    private Transform canvas;
    private List<Product> productList = new List<Product>(); 

    public void loadModelData(Product[] products){
        productList.AddRange(products);
        mapProductsToEmpties(products);
        for (int i = 0; i < products.Length; i++)
        {
            TextMeshProUGUI text = productTextMap[products[i].ModelId];
            text.text = products[i].Price.ToString() + " €";
            StartCoroutine(DataHandler.GetModelData(products[i].ModelId, (Model model) => {
                // Debug.Log("product");
                // Debug.Log(model.id);
                // Debug.Log(model.BuiltinModel);
                StartCoroutine(modelCustomiser.GenerateModel(model, setModels));
            }));
        }
    }

    void setModels(GameObject obj, Model model){
        obj.GetComponentInChildren<Item>().product = findProductWithModelId(model.id);
        obj.transform.parent = productEmptyTransformMap[model.id];
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(0,140,0);
    }



    void mapProductsToEmpties(Product[] products){
        for (int i = 0; i < products.Length; i++)
        {
            productEmptyTransformMap.Add(products[i].ModelId, emptyParent.GetChild(i));
            productTextMap.Add(products[i].ModelId, canvas.GetChild(i).GetComponent<TextMeshProUGUI>());
        }
    }

    // Returns the product with the given modelId
    Product findProductWithModelId(int modelId){
        Product res = productList[0];
        foreach (Product prod in productList)
        {
            if (prod.ModelId == modelId){
                res = prod;
            }
        }
        return res;
    }

}

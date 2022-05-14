using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shirtClosetItemManager : MonoBehaviour
{
    public ModelCustomiser modelCustomiser;
    private Dictionary<int, Transform> productEmptyTransformMap = new Dictionary<int, Transform>();

    [SerializeField]
    private Transform emptyParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getModelData(Product[] products){
        mapProductsToEmpties(products);
        for (int i = 0; i < products.Length; i++)
        {
            StartCoroutine(DataHandler.GetModelData(products[i].id, (Model model) => {
                StartCoroutine(modelCustomiser.GenerateModel(model, setModels));
            }));
        }
    }

    void setModels(GameObject obj){
        
    }



    void mapProductsToEmpties(Product[] products){
        for (int i = 0; i < products.Length; i++)
        {
            productEmptyTransformMap.Add(products[i].id, gameObject.transform.GetChild(i));
        }
    }

}

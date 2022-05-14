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

    public void loadModelData(Product[] products){
        mapProductsToEmpties(products);
        for (int i = 0; i < products.Length; i++)
        {
            StartCoroutine(DataHandler.GetModelData(products[i].id, (Model model) => {
                StartCoroutine(modelCustomiser.GenerateModel(model, setModels));
            }));
        }
    }

    void setModels(GameObject obj, Model model){
        Debug.Log("setting the actual gameobject");
        if (obj == null){
            Debug.Log("gameobject is null");
        }
        obj.transform.parent = productEmptyTransformMap[model.id];
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(0,140,0);
    }



    void mapProductsToEmpties(Product[] products){
        for (int i = 0; i < products.Length; i++)
        {
            productEmptyTransformMap.Add(products[i].id, emptyParent.GetChild(i));
        }
    }

}

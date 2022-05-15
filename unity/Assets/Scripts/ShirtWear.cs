using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirtWear : MonoBehaviour {
    [SerializeField]
    private GameObject wornShirtTemplate;

    void OnTriggerEnter(Collider other) {
        Item item = other.gameObject.GetComponent<Item>();
        if (item != null && item.model.BuiltinModel.Equals("shirt")) {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
            GameObject newShirt = Instantiate(wornShirtTemplate);
            Material mat = item.gameObject.transform.Find("model/override").GetComponent<MeshRenderer>().material;
            newShirt.GetComponentInChildren<MeshRenderer>().material = mat;
            newShirt.transform.parent = transform;
            newShirt.transform.localPosition = Vector3.zero;
            newShirt.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Destroy(item.gameObject);
        }
    }
}

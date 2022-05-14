using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
class Wrapper<T> {
    public T[] array;
}

public class DataHandler {
    public const string SERVER_URL = "http://192.168.7.104:3000/";
    public const string API_URL = SERVER_URL + "api/";

    public static IEnumerator GetCategories(Action<Category[]> callback) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(API_URL + "categories")){
            yield return webRequest.SendWebRequest();

            switch (webRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Category[] categories = JsonUtility.FromJson<Wrapper<Category>>(webRequest.downloadHandler.text).array;
                    callback(categories);
                    break;
            }
        }
    }

    public static IEnumerator GetProducts(Category productCategory, Action<Product[]> callback) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(API_URL + "products?category=" + productCategory.id)){
            yield return webRequest.SendWebRequest();

            switch (webRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Product[] products = JsonUtility.FromJson<Wrapper<Product>>(webRequest.downloadHandler.text).array;
                    callback(products);
                    break;
            }
        }
    }

    public static IEnumerator GetModelData(int modelId, Action<Model> callback) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(API_URL + "model?id=" + modelId)){
            yield return webRequest.SendWebRequest();

            switch (webRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Model[] models = JsonUtility.FromJson<Wrapper<Model>>(webRequest.downloadHandler.text).array;
                    callback(models[0]);
                    break;
            }
        }
    }
}

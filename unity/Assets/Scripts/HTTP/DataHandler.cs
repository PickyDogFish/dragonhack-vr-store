using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
class Wrapper<T> {
    public T[] array;
}

public class DataHandler {
    private const string API_URL = "http://192.168.7.104:3000/api/";

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
                    Debug.Log(categories[0].Name);
                    callback(categories);
                    break;
            }
        }
    }
}

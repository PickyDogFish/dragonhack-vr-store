using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataHandler {
    private const string API_URL = "http://192.168.1.104:3000/api/";

    IEnumerator GetCategories(Action<Category[]> callback) {
        using (UnityWebRequest webRequest = new UnityWebRequest(API_URL + "categories")){
            yield return webRequest.SendWebRequest();

            switch (webRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class ModelCustomiser : MonoBehaviour {
    private const string MODELS_API_LOCATION = DataHandler.SERVER_URL + "models/";
    private const string TEXTURES_API_LOCATION = DataHandler.SERVER_URL + "textures/";

    // Poor man's Dictionary<string, GameObject> implementation, as Unity doesn't have Editor UI for those
    [SerializeField]
    private List<GameObject> builtinModels;
    [SerializeField]
    private List<String> builtinNames;

    // Start is called before the first frame update
    void Start() {
        Model modelToLoad = new Model();
        modelToLoad.builtinModel = "shirt";
        modelToLoad.textureOverride = "shirt_1337.png";

        StartCoroutine(LoadBuiltinModel(modelToLoad, (GameObject loaded) => {
            loaded.transform.parent = transform;
        }));
    }

    // Update is called once per frame
    void Update() {

    }

    public IEnumerator GenerateModel(Model modelData, Action<GameObject> callback) {
        if (modelData.customModel == null || modelData.customModel.Equals(""))
            return LoadBuiltinModel(modelData, callback);
        else
            return LoadCustomModel(modelData, callback);
    }

    private IEnumerator LoadCustomModel(Model modelData, Action<GameObject> callback) {
        if (modelData.customModel == null)
            return null;
        throw new NotImplementedException();
    }

    private IEnumerator LoadBuiltinModel(Model modelData, Action<GameObject> callback) {
        if (modelData.builtinModel == null || modelData.textureOverride == null)
            yield break;
        GameObject output = Instantiate(builtinModels[builtinNames.IndexOf(modelData.builtinModel)]);
        Material mat = output.transform.Find("override").gameObject.GetComponent<MeshRenderer>().material; // Thank you Unity, very cool. You could just implement a GetChild() method, you know?
        using (UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(TEXTURES_API_LOCATION + modelData.textureOverride)){
            yield return textureRequest.SendWebRequest();
            switch (textureRequest.result) {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(textureRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Texture downloaded = DownloadHandlerTexture.GetContent(textureRequest);
                    mat.SetTexture("_MainTex", downloaded);
                    break;
            }
        }
        callback(output);
    }
}

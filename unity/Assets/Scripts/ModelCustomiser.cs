using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using GLTFast;
using System.Threading.Tasks;

public class ModelCustomiser : MonoBehaviour {
    private const string MODELS_API_LOCATION = DataHandler.SERVER_URL + "models/";
    private const string TEXTURES_API_LOCATION = DataHandler.SERVER_URL + "textures/";

    // Poor man's Dictionary<string, GameObject> implementation, as Unity doesn't have Editor UI for those
    [SerializeField]
    private List<GameObject> builtinModels;
    [SerializeField]
    private List<String> builtinNames;

    void Start() {
        Model modelToLoad = new Model();
        modelToLoad.CustomModel = "polica.glb";

        StartCoroutine(GenerateModel(modelToLoad, (GameObject loaded, Model modelData) => {
            loaded.transform.parent = transform;
        }));
    }

    public IEnumerator GenerateModel(Model modelData, Action<GameObject, Model> callback) {
        if (modelData.CustomModel == null || modelData.CustomModel.Equals(""))
            return LoadBuiltinModel(modelData, callback);
        else
            return LoadCustomModel(modelData, callback);
    }

    private IEnumerator LoadCustomModel(Model modelData, Action<GameObject, Model> callback) {
        if (modelData.CustomModel == null)
            yield break;

        GameObject output = new GameObject("custom model");
        GltfImport importer = new GltfImport();
        Task task = importer.Load(MODELS_API_LOCATION + modelData.CustomModel);
        yield return new WaitUntil(() => task.IsCompleted); // Sometimes you just need to make coroutines and await love each other
        importer.InstantiateMainScene(output.transform);
        output.AddComponent<Item>();
        output.AddComponent<BoxCollider>();
        output.GetComponent<BoxCollider>().size = new Vector3(0.3f,0.3f,0.3f);
        output.GetComponent<BoxCollider>().center = new Vector3(0,0.1f,0);

        callback(output, modelData);
    }

    private IEnumerator LoadBuiltinModel(Model modelData, Action<GameObject, Model> callback) {
        if (modelData.BuiltinModel == null || modelData.TextureOverride == null)
            yield break;
        GameObject output = Instantiate(builtinModels[builtinNames.IndexOf(modelData.BuiltinModel)]);
        Material mat = output.transform.Find("model/override").gameObject.GetComponent<MeshRenderer>().material; // Thank you Unity, very cool. You could just implement a GetChild() method, you know?
        using (UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(TEXTURES_API_LOCATION + modelData.TextureOverride)) {
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
        callback(output, modelData);
    }
}

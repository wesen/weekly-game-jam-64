using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;

public class GhostManager : MonoBehaviour {
    public static GhostManager Instance;
    public Ghost GhostPrefab;
    
    public string ServerURL = "https://wgj64-server.herokuapp.com/";

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        _fetchPathsFromServer();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            _fetchPathsFromServer();
        }
    }
    
    private void _fetchPathsFromServer() {
        StartCoroutine(CRGetPaths());
    }
    
    private IEnumerator CRGetPaths() {
        string url = ServerURL + "api/paths";
        using (UnityWebRequest www = UnityWebRequest.Get(url)) {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string data1 = www.downloadHandler.text;
                var pathsFromServer = JsonConvert.DeserializeObject<List<PathObject>>(data1);
                List<GhostInformation> informations  =
                    pathsFromServer.Select(pathObject => GhostInformation.FromPathObject(pathObject)).ToList();
                Debug.Log("Got path informations");
                
                foreach (GhostInformation ghostInformation in informations) {
                    Ghost ghost = Instantiate(GhostPrefab, transform.position, Quaternion.identity);
                    ghost.ExecuteMoves(ghostInformation);


                }
            }
        }
    }
}

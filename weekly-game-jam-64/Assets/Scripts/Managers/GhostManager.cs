using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;
using Random = UnityEngine.Random;

public class GhostManager : MonoBehaviour {
    public static GhostManager Instance;
    public Ghost GhostPrefab;
    private GhostInformation[] _informations;
    public float AverageInterval_s = 2.0f;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
            StartCoroutine(CR_GetPaths());
            StartCoroutine(CR_PlaybackGhosts());
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            StartCoroutine(CR_GetPaths());
        }
    }

    private IEnumerator CR_PlaybackGhosts() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(AverageInterval_s * 0.5f, AverageInterval_s * 1.5f));
            if (_informations.Length > 0) {
                GhostInformation ghostInformation = _informations[Random.Range(0, _informations.Length - 1)];
                Ghost ghost = Instantiate(GhostPrefab, transform.position, Quaternion.identity);
                ghost.transform.parent = transform;
                ghost.ExecuteMoves(ghostInformation);
            }
        }
    }

    private IEnumerator CR_GetPaths(int count = 50) {
        string url = APIClient.ServerURL + "api/paths/" + count;
        using (UnityWebRequest www = UnityWebRequest.Get(url)) {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string data1 = www.downloadHandler.text;
                var pathsFromServer = JsonConvert.DeserializeObject<List<PathObject>>(data1);
                _informations =
                    pathsFromServer.Select(pathObject => GhostInformation.FromPathObject(pathObject)).ToArray();
                Debug.Log("Got path informations");
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Ghost : MonoBehaviour {
    public float Speed;
    private Int32 onStep = 0;
    private List<GhostInformation> _informations;

    private void Awake() {
        RunGetPaths();
    }

    void Update()
    void Update() {
//        if (nextPos.x == 999 && nextPos.y == 999 && pathsloaded) {
////            nextPos.x = pathsFromServer[1].movement[onStep][0];
//        }
//        else if (transform.position != nextPos){
//            StartCoroutine(Move(nextPos));
//        }
    }

    bool isMoving = false;
    float speed = 2f;

    IEnumerator Move(Vector3 offsetFromCurrent) {
        if (isMoving) yield break; // exit function
        isMoving = true;
        Vector3 from = transform.position;
        Vector3 to = from + offsetFromCurrent;
        for (float t = 0f; t < 1f; t += Time.deltaTime * speed) {
            transform.position = Vector2.Lerp(from, to, t);
            yield return null;
        }

        transform.position = to;
        isMoving = false;
    }

    //Path Get
    public string ServerURL = "https://wgj64-server.herokuapp.com/";

    public void RunGetPaths() {
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
                _informations = pathsFromServer.Select(pathObject => GhostInformation.FromPathObject(pathObject)).ToList();
            }
        }
    }

}
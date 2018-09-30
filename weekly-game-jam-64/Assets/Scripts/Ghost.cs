using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Ghost : MonoBehaviour {

    public float Speed;
    public Vector3 nextPos = new Vector3(999,999);
    private bool pathsloaded = false;
    private Int32 onStep = 0;

    private void Awake(){
        RunGetPaths();
    }

    void Update()
    {
        if (nextPos.x == 999 && nextPos.y == 999 && pathsloaded) {
            nextPos.x = pathsFromServer[1].movement[onStep][0];
        }
        else if (transform.position != nextPos){
            StartCoroutine(Move(nextPos));
        }
    }

    bool isMoving = false;
    float speed = 2f;

    IEnumerator Move(Vector3 offsetFromCurrent){
        if (isMoving) yield break; // exit function
        isMoving = true;
        Vector3 from = transform.position;
        Vector3 to = from + offsetFromCurrent;
        for (float t = 0f; t < 1f; t += Time.deltaTime * speed){
            transform.position = Vector2.Lerp(from, to, t);
            yield return null;
        }
        transform.position = to;
        isMoving = false;
    }




    //Path Get
    public List<pathObject> pathsFromServer = new List<pathObject>();
    public string ServerURL = "https://wgj64-server.herokuapp.com/";

    public void RunGetPaths(){
        StartCoroutine(CRGetPaths());
    }

    private IEnumerator CRGetPaths(){
        string url = ServerURL + "api/paths";
        using (UnityWebRequest www = UnityWebRequest.Get(url)){
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError){
                Debug.Log(www.error);
            }
            else{
                string data1 = www.downloadHandler.text;

                Debug.Log(data1);

                pathsFromServer = JsonConvert.DeserializeObject<List<pathObject>>(data1);

                foreach (pathObject pd in pathsFromServer){
                    Debug.Log(pd._id);
                }
            }
        }
        pathsloaded = true;
    }

    //Path Obj
    [System.Serializable]
    public class pathObject{
        public ArrayList position;
        public string _id;
        public string name;
        public string room;
        public ArrayList movement;
        public ArrayList interaction;
        public string createDate;
        public Int32 __v;
    }



}

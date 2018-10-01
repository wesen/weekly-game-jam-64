using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Ghost : MonoBehaviour {
    public float Speed;
    private Int32 onStep = 0;
    private List<GhostInformation> _informations;

    private void Awake() {
        RunGetPaths();
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

    public class GhostInteraction {
        public Vector2 Position;
        public string Interaction;

        public GhostInteraction(Vector2 position, string interaction) {
            this.Interaction = interaction;
            this.Position = position;
        }
    }

    public class GhostInformation {
        public Vector2 Position;
        public Vector2[] Movement;
        public GhostInteraction[] Interactions;
        public string Room;
        
        public static GhostInformation FromPathObject(PathObject pathObject) {
            GhostInformation information = new GhostInformation();
            information.Room = pathObject.room;
            information.Movement = pathObject.movement.Cast<JArray>().Select<JArray, Vector2>((JArray jarray) => {
                return new Vector2(jarray[0].Value<float>(), jarray[1].Value<float>());
            }).ToArray();
            information.Interactions = pathObject.interaction.Cast<JArray>().Select(jarray => {
                float x = jarray[0].Value<float>();
                float y = jarray[1].Value<float>();
                string interaction = jarray[2].Value<string>();
                return new GhostInteraction(new Vector2(x, y), interaction);
            }).ToArray();

            return information;
        }
    }

    //Path Obj
    [System.Serializable]
    public class PathObject {
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
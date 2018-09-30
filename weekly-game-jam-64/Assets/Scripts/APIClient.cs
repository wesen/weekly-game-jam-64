using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;


public class APIClient : MonoBehaviour {
    //Message Get
    public List<messageObject> messagesFromServer = new List<messageObject>();

    public void RunGetMessages() {
        StartCoroutine(CRGetMessages());
    }

    private IEnumerator CRGetMessages() {
        string url = ServerURL + "api/messages";
        using (UnityWebRequest www = UnityWebRequest.Get(url)) {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string data1 = www.downloadHandler.text;

                Debug.Log(data1);

                messagesFromServer = JsonConvert.DeserializeObject<List<messageObject>>(data1);

                foreach (messageObject md in messagesFromServer) {
                    Debug.Log(md._id);
                }
            }
        }
    }

    //Message Send
    public void TestSendMessage() {
        Int32 xpos = 14;
        Int32 ypos = 32;

        string name = "Goldfsh";
        string room = "TestPlace_" + UnityEngine.Random.Range(0, 1000);
        string message = "This is another test!";

        RunSendMessage(xpos, ypos, name, room, message);
    }


    public void RunSendMessage(Int32 x, Int32 y, string name, string room, string message) {
        ArrayList playerPos = new ArrayList();
        playerPos.Add(x);
        playerPos.Add(y);

        messageObject mesObj = new messageObject();
        mesObj.position = playerPos;
        mesObj.name = name;
        mesObj.room = room;
        mesObj.message = message;

        StartCoroutine(CRSendMessage(mesObj));
    }

    private IEnumerator CRSendMessage(messageObject mo) {
        string json = JsonConvert.SerializeObject(mo);

        Debug.Log(json);

        var request = new UnityWebRequest(ServerURL, "POST");
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();

        if (request.error != null) {
            Debug.Log("Error: " + request.error);
        } else {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
        }
    }

    //Message Obj
    [System.Serializable]
    public class messageObject {
        public ArrayList position;
        public string _id;
        public string name;
        public string room;
        public string message;
        public string createDate;
        public Int32 __v;
    }


    //Path Get
    public List<pathObject> pathsFromServer = new List<pathObject>();
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

                Debug.Log(data1);

                pathsFromServer = JsonConvert.DeserializeObject<List<pathObject>>(data1);

                foreach (pathObject pd in pathsFromServer) {
                    Debug.Log(pd._id);
                }
            }
        }
    }

    //Path Send
    public void TestSendPath() {
        Int32 xpos = 14;
        Int32 ypos = 32;

        string name = "Goldfsh";
        string room = "TestPlace_" + UnityEngine.Random.Range(0, 1000);

        ArrayList movement = new ArrayList();
        movement.Add(new int[] {14, 19});
        movement.Add(new int[] {12, 12});

        ArrayList interaction = new ArrayList();
        interaction.Add(new object[] {16, 12, "UP"});
        interaction.Add(new object[] {18, 12, "UP"});


        RunSendPath(xpos, ypos, name, room, movement, interaction);
    }


    public void RunSendPath(Int32 x, Int32 y, string name, string room, ArrayList movement, ArrayList interaction) {
        ArrayList playerPos = new ArrayList();
        playerPos.Add(x);
        playerPos.Add(y);

        pathObject pathObj = new pathObject();
        pathObj.position = playerPos;
        pathObj.name = name;
        pathObj.room = room;
        pathObj.movement = movement;
        pathObj.interaction = interaction;


        StartCoroutine(CRSendPath(pathObj));
    }

    private IEnumerator CRSendPath(pathObject po) {
        string json = JsonConvert.SerializeObject(po);

        Debug.Log(json);

        var request = new UnityWebRequest("https://wgj64-server.herokuapp.com/api/paths", "POST");
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();

        if (request.error != null) {
            Debug.Log("Error: " + request.error);
        } else {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
        }
    }

    //Path Obj
    [System.Serializable]
    public class pathObject {
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
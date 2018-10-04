using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;


public class APIClient : MonoBehaviour {
    //Message Get
    public static string ServerURL = "https://wgj64-server.herokuapp.com/";
    
    //Message Send
    public void RunSendMessage(float x, float y, string name, string room, string message) {
        ArrayList playerPos = new ArrayList();
        playerPos.Add(x);
        playerPos.Add(y);

        MessageObject mesObj = new MessageObject();
        mesObj.position = playerPos;
        mesObj.name = name;
        mesObj.room = room;
        mesObj.message = message;

        StartCoroutine(CRSendMessage(mesObj));
    }

    private IEnumerator CRSendMessage(MessageObject mo) {
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


    //Path Get
    public void RunSendPath(Int32 x, Int32 y, string name, string room, ArrayList movement, ArrayList interaction) {
        ArrayList playerPos = new ArrayList();
        playerPos.Add(x);
        playerPos.Add(y);

        PathObject pathObj = new PathObject();
        pathObj.position = playerPos;
        pathObj.name = name;
        pathObj.room = room;
        pathObj.movement = movement;
        pathObj.interaction = interaction;


        StartCoroutine(CRSendPath(pathObj));
    }

    private IEnumerator CRSendPath(PathObject po) {
        string json = JsonConvert.SerializeObject(po);

        Debug.Log(json);

        var request = new UnityWebRequest(ServerURL + "api/paths", "POST");
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

}
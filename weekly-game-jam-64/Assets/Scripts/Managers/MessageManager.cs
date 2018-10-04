using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class MessageManager : MonoBehaviour {
	public static MessageManager Instance;
	private GhostMessage[] _messages;
	public Message MessagePrefab;
	
	void Awake () {
		if (Instance == null) {
			Instance = this;
			StartCoroutine(CR_GetMessages());
		} else {
			Destroy(gameObject);
		}
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			StartCoroutine(CR_GetMessages());
		}
	}

	private IEnumerator CR_GetMessages(int count = 50) {
		string url = APIClient.ServerURL + "api/messages";
        using (UnityWebRequest www = UnityWebRequest.Get(url)) {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                string data1 = www.downloadHandler.text;

                Debug.Log(data1);

                List<MessageObject> messagesFromServer = JsonConvert.DeserializeObject<List<MessageObject>>(data1);
	            _messages = messagesFromServer.Select(msgObject => GhostMessage.FromObject(msgObject)).ToArray();

                foreach (GhostMessage msg in _messages) {
	                Message _msg = Instantiate(MessagePrefab, msg.Position / 10.0f, Quaternion.identity);
	                _msg.FillFromMessage(msg);
	                 _msg.transform.parent = transform;
                }
            }
        }
		
	}
}

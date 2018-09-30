using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public GameObject DialogueBox;
    public Text Message;
    public Text ContinueMessage;

    public bool IsDialogActive = false;
    
    void Update() {
        if (IsDialogActive && Input.GetButtonDown("Submit")) {
            IsDialogActive = true;
        }
    }
}

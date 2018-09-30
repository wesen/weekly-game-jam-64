using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public GameObject DialogueBox;
    public Text Message;
    public Text ContinueMessage;

    public bool IsDialogActive = false;

    void Awaake() {
        if (IsDialogActive) {
            DialogueBox.SetActive(true);
        } else {
            DialogueBox.SetActive(false);
        }
    }

    void Update() {
        if (IsDialogActive && Input.GetButtonDown("Submit")) {
            DialogueBox.SetActive(false);
            IsDialogActive = false;
        }
    }

    public void ShowBox(string text) {
        Message.text = text;
        DialogueBox.SetActive(true);
        IsDialogActive = true;
    }
}
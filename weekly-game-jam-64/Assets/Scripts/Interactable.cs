using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Interactable : MonoBehaviour {
    private DialogueManager _DialogueManager;
    void Awake() {
        _DialogueManager = FindObjectOfType<DialogueManager>();
    }
    
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "player") {
            _DialogueManager.ShowBox("Some stupid description");
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.name == "player") {
        }
    }
}
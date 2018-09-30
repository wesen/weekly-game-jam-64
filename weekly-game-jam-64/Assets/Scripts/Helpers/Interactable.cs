using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Interactable : MonoBehaviour {
    private DialogueManager _DialogueManager;
    private FloatingText _floatingText;
    void Awake() {
        _DialogueManager = FindObjectOfType<DialogueManager>();
        _floatingText = GetComponentInChildren<FloatingText>();
    }
    
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "player") {
            _DialogueManager.ShowBox("Some stupid description");
            _floatingText.ShowText("BEAUTIFUL");
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.name == "player") {
        }
    }
}
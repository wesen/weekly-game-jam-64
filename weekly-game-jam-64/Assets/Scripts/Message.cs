using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour {
    private DialogueTrigger _dialogueTrigger;
    private Transform _player;

    public void Awake() {
        _dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    public void Start() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void FillFromMessage(GhostMessage message) {
        _dialogueTrigger.dialogue.sentences = message.Message.Split('\n');
    }

    public void Update() {
        if (Vector2.Distance(transform.position, _player.position) < 0.2 && Input.GetKeyDown(KeyCode.E)) {
            _dialogueTrigger.TriggerDialogue();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour {
    private Dialogue _dialogue = new Dialogue();

    private Transform _player;

    public void Start() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void FillFromMessage(GhostMessage message) {
        _dialogue.name = message.Name;
        _dialogue.sentences = message.Message.Split('\n');
    }

    public void Update() {
        if (Vector2.Distance(transform.position, _player.position) < 0.2 && Input.GetKeyDown(KeyCode.E)) {
            DialogueManager.Instance.StartDialogue(_dialogue);
        }
    }
}
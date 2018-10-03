using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue_Controller : MonoBehaviour {
    public Transform player;
    public GameObject bubble;
    public DialogueTrigger dialogueTrig;
    private bool hasTalked = false;
    private int timeSinceTalk = 0;
    public int talkResetDelay = 1000;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeSinceTalk = 0;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < 0.2 && Input.GetKeyDown(KeyCode.E)) {
            if (hasTalked == false) {
                dialogueTrig.TriggerDialogue();
                hasTalked = true;
                bubble.SetActive(false);
            }
        } else {
            if (hasTalked && timeSinceTalk < talkResetDelay && talkResetDelay != -1)
            {
                timeSinceTalk += 1;
            }
            else if (hasTalked && timeSinceTalk >= talkResetDelay && talkResetDelay != -1) {
                timeSinceTalk = 0;
                bubble.SetActive(true);
                hasTalked = false;
            }
        }
        
    }
}

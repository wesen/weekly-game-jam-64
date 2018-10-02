using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue_Controller : MonoBehaviour {
    public Transform player;
    public GameObject bubble;
    public DialogTrigger dialogTrig;
    private bool hasTalked;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < 0.2 && Input.GetKeyDown(KeyCode.E)) {
            if (hasTalked == false) {
                dialogTrig.TriggerDialogue();
                hasTalked = true;
                bubble.SetActive(false);
            }
        } else {
            Debug.Log("we can't talk. :(");
        }
        
    }
}

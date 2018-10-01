using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {

    Player pScript;
    
    public int freqOfSteps = 1;
    private int waitStep = 0;
    AudioSource[] footsteps;
    

    // Use this for initialization
    void Start () {
        pScript = transform.parent.GetComponent<Player>();
        footsteps = GetComponents<AudioSource>();
        Debug.Log(footsteps.Length);
    }
	
	// Update is called once per frame
	void Update () {
        // Debug.Log(pScript.isMoving);
        if (pScript.isMoving) {
            if (waitStep >= 10*freqOfSteps){
                int rnd = Random.Range(0, footsteps.Length-1);
                Debug.Log("play sound " + rnd);
                footsteps[rnd].Play();
                waitStep = 0;
            }
            else{
                waitStep += 1;
            }
        }
        else {
            waitStep = 0;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {

    public Text dialogueText;
    public Animator animator;
    private bool crRunning = false;
    private Coroutine cr;


    public static DialogueManager Instance = null;

    private Queue<string> sentences;

	// Use this for initialization
	void Start () {
	    if (DialogueManager.Instance == null) {
	        DialogueManager.Instance = this;
            sentences = new Queue<string>();
	    } else {
	        Destroy(gameObject);
	    }
	}

    public void StartDialogue(Dialogue dialogue) {
        animator.SetBool("IsOpen", true);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        if (crRunning){
            StopCoroutine(cr);
        }
        crRunning = true;
        cr = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence){
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }

        crRunning = false;
    }

    void EndDialogue() {
        animator.SetBool("IsOpen", false);
    }
}

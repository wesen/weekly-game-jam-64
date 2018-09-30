using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Vector3 _moveDelta;
    private BoxCollider2D _boxCollider;

    void Awake() {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _moveDelta = new Vector3(x, y, 0);

        // control sprite direction
        if (_moveDelta.x > 0) {
            transform.localScale = Vector3.one;
        } else if (_moveDelta.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        RaycastHit2D hit =
            Physics2D.BoxCast(transform.position, _boxCollider.size, 0, _moveDelta,
                _moveDelta.magnitude * Time.deltaTime,
                LayerMask.GetMask("Collision", "Actors"));
        if (hit.collider == null) {
            transform.Translate(_moveDelta * Time.deltaTime);
        }
    }

    //This block will send_player_pos json to server
    bool isOpen = false;
    int[] prevPos = new int[1];
    int[] curPos = new int[1];
    ArrayList posAL = new ArrayList();
    public GameObject apiObj;

    void OnGUI()
    {
        if (isOpen)
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "x:"+ Mathf.RoundToInt(transform.position.x)+", y:"+ Mathf.RoundToInt(transform.position.y))) //Display and use the Yes button
            {
                Debug.Log("Yes");
                isOpen = false;
            }
            if (GUI.Button(new Rect(120, 10, 100, 50), "Show Stack in Debug")) //Display and use the Yes button
            {
                int i = 1;
                foreach (var pos in posAL)
                {
                    Debug.Log(i + ": " + pos);
                    
                }
            }
            if (GUI.Button(new Rect(230, 10, 100, 50), "Send Stack to Server")) //Display and use the Yes button
            {
                foreach (var pos in posAL)
                {
                    //POST
                }
            }
        }
        curPos[0] = Mathf.RoundToInt(transform.position.x);
        curPos[0] = Mathf.RoundToInt(transform.position.y);
        if (prevPos != curPos)
        {
            posAL.Add(prevPos);
            prevPos = curPos;
        }
    }

    void OnMouseDown() //Get the mouse click
    {
        isOpen = true;   //Set the buttons to appear
    }
    //End send_player_pos block
}
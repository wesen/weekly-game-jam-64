using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Vector3 _moveDelta;
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    public float Speed = 0.5f;
    

    void Awake() {
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _moveDelta = new Vector3(x, y, 0) * Time.deltaTime * Speed;

        RaycastHit2D hit =
            Physics2D.BoxCast(transform.position, _boxCollider.size, 0, _moveDelta,
                _moveDelta.magnitude,
                LayerMask.GetMask("Collision", "Actors"));
        if (hit.collider == null) {
            transform.Translate(_moveDelta);
        }

        if (_moveDelta.x > 0) {
            _animator.Play("player_walking_right");
        } else if (_moveDelta.x < 0) {
            _animator.Play("player_walking_left");
        } else if (_moveDelta.y < 0) {
            _animator.Play("player_walking_frontal");
        } else if (_moveDelta.y > 0) {
            _animator.Play("player_walking_up");
        } else {
            _animator.Play("player_idle_frontal");
        }
    }

    //This block will send_player_pos json to server
    bool isOpen = false;
    string prevPos = "";
    Stack posStack = new Stack();

    void OnGUI() {
        if (isOpen) {
            if (GUI.Button(new Rect(10, 10, 100, 50), transform.position.ToString())) //Display and use the Yes button
            {
                Debug.Log("Yes");
                isOpen = false;
            }

            if (GUI.Button(new Rect(120, 10, 100, 50), "Show Stack in Debug")) //Display and use the Yes button
            {
                int i = 0;
                foreach (var pos in posStack) {
                    Debug.Log(i + ": " + pos);
                }
            }

            if (GUI.Button(new Rect(230, 10, 100, 50), "Send Stack to Server")) //Display and use the Yes button
            {
                foreach (var pos in posStack) {
                    //POST
                }
            }
        }

        if (prevPos != transform.position.ToString()) {
            posStack.Push(prevPos);
            prevPos = transform.position.ToString();
        }
    }

    //Get the mouse click
    void OnMouseDown() {
        isOpen = true; //Set the buttons to appear
    }

    //End send_player_pos block
}
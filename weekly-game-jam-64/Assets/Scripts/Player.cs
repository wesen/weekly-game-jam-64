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
}
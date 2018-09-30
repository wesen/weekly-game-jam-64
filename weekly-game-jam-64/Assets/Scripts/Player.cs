using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Vector3 _moveDelta;
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    public float Speed = 0.5f;

    void Awake(){
        _animator = GetComponent<Animator>();
        _animator.speed = Speed * 2;
    }

    void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _moveDelta = new Vector3(x, y, 0) * Time.deltaTime * Speed;
        transform.Translate(_moveDelta);

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

    private void OnDrawGizmos() {
        if (_boxCollider != null) {
            Gizmos.color = Color.red;
            Vector3 offset = new Vector3(_boxCollider.offset.x, _boxCollider.offset.y, 0);
            Gizmos.DrawWireCube(_boxCollider.transform.position + offset,
                _boxCollider.size);
        }
    }
}
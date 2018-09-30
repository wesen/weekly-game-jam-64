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
        _animator.speed = Speed * 2;
    }

    void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _moveDelta = new Vector3(x, y, 0) * Time.deltaTime * Speed;

        Vector2 size = new Vector2(_boxCollider.size.x, _boxCollider.size.y);

        Vector2 origin = _boxCollider.transform.position;
        origin += _boxCollider.offset;
        
        RaycastHit2D hit =
            Physics2D.BoxCast(origin, _boxCollider.size, 0, _moveDelta,
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

    private void OnDrawGizmos() {
        if (_boxCollider != null) {
            Gizmos.color = Color.red;
            Vector3 offset = new Vector3(_boxCollider.offset.x, _boxCollider.offset.y, 0);
            Gizmos.DrawWireCube(_boxCollider.transform.position + offset,
                _boxCollider.size);
        }
    }
}
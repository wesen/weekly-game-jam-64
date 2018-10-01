using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Vector3 _moveDelta;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private SpriteRenderer _renderer;

    public float Speed = 0.5f;
    public float SmokeInterval_s = 2.0f;
    private float _timeSinceSmoke = 0.0f;
    public Transform SmokeAnimation;
    public bool isMoving = true;

    void Awake() {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator.speed = Speed * 2;
    }

    void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _moveDelta = new Vector3(x, y, 0) * Time.deltaTime * Speed;
        transform.Translate(_moveDelta);

        float angle = 0.0f;
        
        if (_moveDelta.x > 0) {
            _animator.Play("player_walking_right");
            angle = 90;
            isMoving = true;
        } else if (_moveDelta.x < 0) {
            _animator.Play("player_walking_left");
            angle = -90;
            isMoving = true;
        } else if (_moveDelta.y < 0) {
            angle = 0;
            _animator.Play("player_walking_frontal");
            isMoving = true;
        } else if (_moveDelta.y > 0) {
            angle = 180;
            _animator.Play("player_walking_up");
            isMoving = true;
        } else {
            _animator.Play("player_idle_frontal");
            isMoving = false;
        }

        if (isMoving) {
            _timeSinceSmoke += Time.deltaTime;

            if (_timeSinceSmoke > SmokeInterval_s) {
                _timeSinceSmoke = 0.0f;
                Vector3 feetPosition = transform.position
                                       - new Vector3(0, _renderer.bounds.size.y / 2.0f, 0);

                Vector3 smokePosition = feetPosition - _moveDelta * 5.0f;
                // SmokeAnimation.GetComponent<SpriteRenderer>().bounds.size / 2.0f;
                Instantiate(SmokeAnimation, smokePosition, Quaternion.Euler(0, 0, angle));
            }
        } else {
            _timeSinceSmoke = 0.0f;
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
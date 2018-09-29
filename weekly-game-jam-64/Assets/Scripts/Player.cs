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
        if (hit.collider != null) {
            Debug.Log("hit " + hit.collider + " " + _moveDelta);
        }

        if (hit.collider == null) {
            transform.Translate(_moveDelta * Time.deltaTime);
        }
    }
}
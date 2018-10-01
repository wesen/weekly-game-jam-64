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
    public int direction; //idle=0,up=1,right=2,down=3,left=4
    public int fuDirection;
    public bool isDashing = false;
    public int dashI = 0;
    public int dashTime = 150;
    public float origSpeed;
    public int framesIdle = 0;
    private GameObject footstepsObject;
    public int freqOfSteps = 1;
    private int waitStep = 0;
    AudioSource[] footsteps;

    void Awake() {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator.speed = Speed * 2;
        origSpeed = Speed;
    }

    private void Start() {
        footstepsObject = this.transform.Find("footstepsObject").gameObject;
        footsteps = footstepsObject.GetComponents<AudioSource>();
        //Debug.Log(footstepsObject.name);
    }

    void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _moveDelta = new Vector3(x, y, 0) * Time.deltaTime * Speed;
        transform.Translate(_moveDelta);

        float angle = 0.0f;

        //Move Logic
        if (_moveDelta.x > 0) {
            _animator.Play("player_walking_right");
            angle = 90;
            isMoving = true;
            fuDirection = 2;
            framesIdle = 0;
        } else if (_moveDelta.x < 0) {
            _animator.Play("player_walking_left");
            angle = -90;
            isMoving = true;
            fuDirection = 4;
            framesIdle = 0;
        } else if (_moveDelta.y < 0) {
            angle = 0;
            _animator.Play("player_walking_frontal");
            isMoving = true;
            fuDirection = 3;
            framesIdle = 0;
        } else if (_moveDelta.y > 0) {
            angle = 180;
            _animator.Play("player_walking_up");
            isMoving = true;
            fuDirection = 1;
            framesIdle = 0;
        } else {
            _animator.Play("player_idle_frontal");
            isMoving = false;            
            framesIdle += 1;
            isDashing = false;
            fuDirection = 0;
        }

        //Footsteps + Smoke
        if (isMoving) {
            if (waitStep >= 10 * freqOfSteps) {
                int rnd = UnityEngine.Random.Range(0, footsteps.Length - 1);
                Debug.Log("play sound " + rnd);
                footsteps[rnd].Play();
                waitStep = 0;
                Vector3 feetPosition = transform.position
                                       - new Vector3(0, _renderer.bounds.size.y / 2.0f, 0);

                Vector3 smokePosition = feetPosition - _moveDelta * 5.0f;
                Instantiate(SmokeAnimation, smokePosition, Quaternion.Euler(0, 0, angle));
            }
            else {
                waitStep += Mathf.RoundToInt(Speed);
            }
        }
        else {
            waitStep = 0;
        }


        //Fixed update Dash code
        if (isDashing)
        {
            Speed = origSpeed * 2;
            dashI += 1;
        }
        else
        {
            Speed = origSpeed;
            dashI = 0;
        }


        //Smoke Animation
        //if (isMoving) {
        //    _timeSinceSmoke += Time.deltaTime;

        //    if (_timeSinceSmoke > SmokeInterval_s) {
        //        _timeSinceSmoke = 0.0f;
        //        Vector3 feetPosition = transform.position
        //                               - new Vector3(0, _renderer.bounds.size.y / 2.0f, 0);

        //        Vector3 smokePosition = feetPosition - _moveDelta * 5.0f;
        //        // SmokeAnimation.GetComponent<SpriteRenderer>().bounds.size / 2.0f;
        //        Instantiate(SmokeAnimation, smokePosition, Quaternion.Euler(0, 0, angle));
        //    }
        //} else {
        //    _timeSinceSmoke = 0.0f;
        //}
    }

    private void Update() {
        //Dash Logic

        if (!isMoving && framesIdle > 10)
        {
            isDashing = false;
            direction = 0;
        }
        else
        {
            if (dashI >= dashTime)
            {
                isDashing = false;
            }
            else if (direction == 1 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isDashing == false)
            {
                isDashing = true;
            }
            else if (direction == 2 && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && isDashing == false)
            {
                isDashing = true;
            }
            else if (direction == 3 && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && isDashing == false)
            {
                isDashing = true;
            }
            else if (direction == 4 && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && isDashing == false)
            {
                isDashing = true;
            }
        }

        if (fuDirection > 0) { direction = fuDirection; }
    }

    //is this still being used?
    private void OnDrawGizmos() {
        if (_boxCollider != null) {
            Gizmos.color = Color.red;
            Vector3 offset = new Vector3(_boxCollider.offset.x, _boxCollider.offset.y, 0);
            Gizmos.DrawWireCube(_boxCollider.transform.position + offset,
                _boxCollider.size);
        }
    }
}
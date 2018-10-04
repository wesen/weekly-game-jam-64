using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class TitleScreen : MonoBehaviour {
    public List<TitleButton> Buttons;
    public Transform Arrow;

    private int _activeIndex = 0;

    void Awake() {
    }

    void Start() {
        _selectEntry(0);
    }

    private void _selectEntry(int idx) {
        for (int i = 0; i < Buttons.Count; i++) {
            if (i == idx) {
                Buttons[i].SelectButton();
                Arrow.position = new Vector2(Arrow.position.x, Buttons[i].transform.position.y);
            } else {
                Buttons[i].UnselectButton();
            }
        }

        _activeIndex = idx;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            int idx = Math.Max(0, _activeIndex - 1);
            _selectEntry(idx);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            int idx = Math.Min(Buttons.Count - 1, _activeIndex + 1);
            _selectEntry(idx);
        } else if (Input.GetKeyDown(KeyCode.Return)) {
            Debug.Log("Select action");
        }
    }
}
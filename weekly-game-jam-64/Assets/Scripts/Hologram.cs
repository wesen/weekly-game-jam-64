using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Hologram : MonoBehaviour {
    [Range(0.001f, 10)] public float Scale = 2.0f;

    [Range(0, 10)] public float Speed = 2.0f;

    private SpriteRenderer _renderer;
    private MaterialPropertyBlock _block;

    void Awake() {
        _block = new MaterialPropertyBlock();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (_block == null) {
            _block = new MaterialPropertyBlock();
        }
        if (_renderer != null) {
            _renderer.GetPropertyBlock(_block);
            _block.SetFloat("_Scale", Scale);
            _block.SetFloat("_Speed", Speed);
            _renderer.SetPropertyBlock(_block);
        }
    }
}
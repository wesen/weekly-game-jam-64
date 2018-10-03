using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GlitchedSprite : MonoBehaviour {
    public List<Sprite> sprites = new List<Sprite>();
    private SpriteRenderer _renderer;
    private Vector3 _originalPosition;
    private Sprite _originalSprite;
    public float BounceRange = 0.02f;
    public float BounceInterval = 0.1f;
    private Transform _player;

    public bool Glitched = false;

    void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _originalSprite = _renderer.sprite;
        _originalPosition = transform.position;
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(CR_glitcher());
    }

    private IEnumerator CR_glitcher() {
        while (true) {
            yield return new WaitForSeconds(BounceInterval);
            if (Vector2.Distance(_originalPosition, _player.position) < 0.5f) {
                transform.position =
                    _originalPosition + new Vector3(Random.Range(-BounceRange, BounceRange),
                        Random.Range(-BounceRange, BounceRange), 0);
                _renderer.sprite = sprites[Random.Range(0, sprites.Count - 1)];
                _renderer.flipX = Random.value > 0.5f;
                _renderer.flipY = Random.value > 0.5f;
                Color c = Color.HSVToRGB(Random.value, Random.value, 1.0f);
                _renderer.color = new Color(c.r, c.g, c.b, Random.Range(0.5f, 1.0f));
            } else {
                transform.position = _originalPosition;
                _renderer.sprite = _originalSprite;
                _renderer.flipX = false;
                _renderer.flipY = false;
                _renderer.color = Color.white;
            }
        }
    }

    // Update is called once per frame
    void Update() {
    }
}
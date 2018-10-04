using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton : MonoBehaviour {
	public Sprite ActiveSprite;
	public Sprite InactiveSprite;

	private SpriteRenderer _renderer;

	void Awake() {
		_renderer = GetComponent<SpriteRenderer>();
	}

	public void SelectButton() {
		_renderer.sprite = ActiveSprite;
	}

	public void UnselectButton() {
		_renderer.sprite = InactiveSprite;
	}
}


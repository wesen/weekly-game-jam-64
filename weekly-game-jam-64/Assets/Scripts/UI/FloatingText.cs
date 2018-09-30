using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour {
    private TextMesh _textMesh;
    private bool _isVisible = false;

    void Awake() {
        _textMesh = GetComponent<TextMesh>();

        LeanTween.color(_textMesh.gameObject, Color.clear, 0.0f);
    }

    public void ShowText(string text) {
        _textMesh.text = text;
        
        LTDescr tween = LeanTween.color(_textMesh.gameObject, Color.white, 1.0f)
            .setEase(LeanTweenType.easeInOutExpo)
            .setOnComplete(() => {
                _isVisible = true; 
                HideText();
                
            });
    }

    public void HideText() {
        LTDescr tween = LeanTween.color(_textMesh.gameObject, Color.clear, 1.0f)
            .setEase(LeanTweenType.easeInOutExpo)
            .setOnComplete(() => { _isVisible = false; });
    }
}
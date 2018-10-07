using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGlitcher : MonoBehaviour {
    private SpriteRenderer _renderer;
    private MaterialPropertyBlock _propertyBlock;

    void Start() {
        _propertyBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<SpriteRenderer>();

        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetFloat("_TearsDistance", 0.1f);
        _renderer.SetPropertyBlock(_propertyBlock);

        Debug.Log("Material " + _propertyBlock);
        StartCoroutine(CR_Glitch());
    }

    IEnumerator CR_Glitch() {
        while (true) {
            yield return new WaitForSeconds(Random.RandomRange(0.5f, 2.0f));

            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat("_Displacement", Random.Range(-0.02f, 0.02f));
            _renderer.SetPropertyBlock(_propertyBlock);
            
            _renderer.sharedMaterial.SetFloat("_Displacement", 0.01f);
            Debug.Log("displacement " + _renderer.sharedMaterial.GetFloat("_Displacement"));
            Debug.Log("Glitch get " + _propertyBlock.GetFloat("_Displacement"));

            yield return new WaitForSeconds(Random.RandomRange(0.01f, 0.1f));
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat("_Displacement", 0f);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
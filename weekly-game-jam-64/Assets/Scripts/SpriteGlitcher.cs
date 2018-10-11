using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGlitcher : MonoBehaviour {
    private SpriteRenderer _renderer;
    private MaterialPropertyBlock _propertyBlock;

    void Start() {
        _propertyBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<SpriteRenderer>();

        StartCoroutine(CR_Glitch());
        StartCoroutine(CR_Glitch2());
    }

    IEnumerator CR_Glitch() {
        while (true) {
            yield return new WaitForSeconds(Random.RandomRange(0.5f, 2.0f));

            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat("_Displacement", Random.Range(-0.02f, 0.02f));
            _renderer.SetPropertyBlock(_propertyBlock);
            
            yield return new WaitForSeconds(Random.RandomRange(0.1f, 0.3f));
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat("_Displacement", 0f);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
    
    IEnumerator CR_Glitch2() {
        while (true) {
            yield return new WaitForSeconds(Random.RandomRange(0.5f, 2.0f));

            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat("_TearsDistance", Random.Range(-0.1f, 0.1f));
            _renderer.SetPropertyBlock(_propertyBlock);
            
            yield return new WaitForSeconds(Random.RandomRange(0.01f, 0.1f));
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat("_TearsDistance", 0f);
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }

}
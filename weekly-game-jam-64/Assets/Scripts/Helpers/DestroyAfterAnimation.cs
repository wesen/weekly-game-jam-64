using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour {
    private bool _shouldDie = false;

    public void Update() {
        if (_shouldDie) {
            DestroyImmediate(this.gameObject);
        }
    }
    
    public void Kill() {
//        _shouldDie = true;
        Destroy(this.gameObject);
    }

    void OnDestroy() {
        Debug.Log("Destroyed");
    }
}

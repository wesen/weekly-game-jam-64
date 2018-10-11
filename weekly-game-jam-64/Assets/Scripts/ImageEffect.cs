using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour {
    public Material ShaderMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        Graphics.Blit(src, dst, ShaderMaterial);
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class BloomEffect : MonoBehaviour {
    [Range(1, 16)] public int DownsampleIterations;
    public Shader BloomShader;
    [Range(0, 10)] public float Threshold = 1;
    [Range(0, 1)] public float SoftThreshold = 0.5f;

    public bool Debug;

    [NonSerialized] private Material _material = null;

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (_material == null) {
            _material = new Material(BloomShader);
        }

        _material.SetTexture("_SourceTex", src);

        float knee = Threshold * SoftThreshold;
        Vector4 filter;
        filter.x = Threshold;
        filter.y = filter.x - knee;
        filter.z = 2f * knee;
        filter.w = 0.25f / (knee + 0.00001f);
        _material.SetVector("_Filter", filter);

        int width = src.width;
        int height = src.height;

        int actualIterations = DownsampleIterations;

        RenderTexture[] textures = new RenderTexture[DownsampleIterations];
        for (actualIterations = 0; actualIterations < DownsampleIterations; actualIterations++) {
            if (width < 2 || height < 2) {
                break;
            }

            textures[actualIterations] = RenderTexture.GetTemporary(width, height, 0, src.format);
            width /= 2;
            height /= 2;
        }

        Graphics.Blit(src, textures[0], _material, InitialPrefilterPass);
        for (int i = 1; i < actualIterations; i++) {
            Graphics.Blit(textures[i - 1], textures[i], _material, DownsamplePass);
        }

        for (int i = actualIterations - 1; i > 0; i--) {
            Graphics.Blit(textures[i], textures[i - 1], _material, UpsamplePass);
        }

        if (Debug) {
            Graphics.Blit(textures[0], dest, _material, DebugPass);
        } else {
            Graphics.Blit(textures[0], dest, _material, FinalPass);
        }

        for (int i = 0; i < actualIterations; i++) {
            RenderTexture.ReleaseTemporary(textures[i]);
        }
    }

    private const int InitialPrefilterPass = 0;
    private const int DownsamplePass = 1;
    private const int UpsamplePass = 2;
    private const int FinalPass = 3;
    private const int DebugPass = 4;
}
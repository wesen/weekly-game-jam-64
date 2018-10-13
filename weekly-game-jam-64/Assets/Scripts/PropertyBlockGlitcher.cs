using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class PropertyGlitchSettings {
    [MinMax(0, 3, ShowEditRange = true)] public Vector2 GlitchedDuration = new Vector2(1, 1.5f);
    [MinMax(0, 3, ShowEditRange = true)] public Vector2 NormalDuration = new Vector2(1, 1.5f);
    [Range(0, 1)] public float LerpDuration = 0.0f;
    public float RandomAmplitude = 0.01f;
    public float Value = 1.0f;
    public string PropertyName;
}

public class PropertyGlitchState {
    public PropertyGlitchState(PropertyGlitchSettings settings) {
        Settings = settings;
    }

    public PropertyGlitchSettings Settings;

    public float T = 0;
    public float GlitchDuration = 1f;
    public float CalmDuration = 1f;

    public float StartValue = 0f;
    public float CurrentValue = 0f;
    public float TargetValue = 0f;

    enum StateEnum {
        GLITCHING,
        LERP_TO_NORMAL,
        NORMAL,
        LERP_TO_GLITCHED,
    };

    private StateEnum State = StateEnum.GLITCHING;

    public void Update() {
//        Debug.Log("Update property glitch state " + Settings.PropertyName + " T " + T);
        T += Time.deltaTime;
        switch (State) {
            case StateEnum.GLITCHING:
                if (T > GlitchDuration) {
                    State = StateEnum.LERP_TO_NORMAL;
                    CalmDuration = Random.Range(Settings.NormalDuration.x, Settings.NormalDuration.y);
                    StartValue = CurrentValue;
                    TargetValue = Settings.Value;
                    T -= GlitchDuration;
                    Debug.Log(Settings.PropertyName + " switching to NORMAL,"
                                                    + " start " + StartValue
                                                    + " target " + TargetValue
                                                    + " duration " + CalmDuration
                                                    + " lerp duration " + Settings.LerpDuration);
                }

                break;

            case StateEnum.NORMAL:
                if (T > CalmDuration) {
                    State = StateEnum.LERP_TO_GLITCHED;
                    GlitchDuration = Random.Range(Settings.GlitchedDuration.x, Settings.GlitchedDuration.y);
                    StartValue = CurrentValue;
                    TargetValue = Settings.Value + Random.Range(-Settings.RandomAmplitude, Settings.RandomAmplitude);
                    T -= CalmDuration;
                    Debug.Log(Settings.PropertyName + " switching to GLITCHING,"
                                                    + " start " + StartValue
                                                    + " target " + TargetValue
                                                    + " duration " + GlitchDuration
                                                    + " lerp duration " + Settings.LerpDuration);
                }

                break;

            case StateEnum.LERP_TO_NORMAL:
            case StateEnum.LERP_TO_GLITCHED:
                break;
        }

        // update lerp value
        switch (State) {
            case StateEnum.GLITCHING:
            case StateEnum.NORMAL:
                CurrentValue = TargetValue;
                break;

            case StateEnum.LERP_TO_NORMAL:
                if (T < Settings.LerpDuration) {
                    CurrentValue = Mathf.Lerp(StartValue, TargetValue, T / Settings.LerpDuration);
                } else {
                    Debug.Log(Settings.PropertyName + " lerp finished, back to NORMAL");
                    State = StateEnum.NORMAL;
                }

                break;

            case StateEnum.LERP_TO_GLITCHED:
                if (T < Settings.LerpDuration) {
                    CurrentValue = Mathf.Lerp(StartValue, TargetValue, T / Settings.LerpDuration);
                } else {
                    Debug.Log(Settings.PropertyName + " lerp finished, back to Glitching");
                    State = StateEnum.GLITCHING;
                }

                break;
        }
    }
}

public class PropertyBlockGlitcher : MonoBehaviour {
    public List<PropertyGlitchSettings> GlitchSettings = new List<PropertyGlitchSettings>();

    private Dictionary<string, PropertyGlitchState> _states = new Dictionary<string, PropertyGlitchState>();

    private MaterialPropertyBlock _block;
    private SpriteRenderer _renderer;

    void Start() {
        Debug.Log("Start");
        _block = new MaterialPropertyBlock();
        _renderer = GetComponent<SpriteRenderer>();
        _SyncDictionary();
    }

    private void _SyncDictionary() {
        Debug.Log("Sync dictionary");
        HashSet<string> activeProperties = new HashSet<string>();

        foreach (PropertyGlitchSettings setting in GlitchSettings) {
            Debug.Log("Checking list setting " + setting.PropertyName);
            activeProperties.Add(setting.PropertyName);
            if (!_states.ContainsKey(setting.PropertyName)) {
                _states[setting.PropertyName] = new PropertyGlitchState(setting);
                Debug.Log("Adding glitch state for " + setting.PropertyName);
            }
        }

        foreach (string propertyName in _states.Keys) {
            Debug.Log("Checking existing property " + propertyName);
            if (!activeProperties.Contains(propertyName)) {
                Debug.Log("Remove glitch state for " + propertyName);
                _states.Remove(propertyName);
            }
        }
    }

    void Update() {
        if (_renderer != null) {
            _renderer.GetPropertyBlock(_block);
            foreach (PropertyGlitchState state in _states.Values) {
                state.Update();
                Debug.Log("Current value " + state.Settings.PropertyName + " " + state.CurrentValue);
                _block.SetFloat(state.Settings.PropertyName, state.CurrentValue);
            }

            _renderer.SetPropertyBlock(_block);
        }
    }

    void OnValidate() {
        Debug.Log("On Validate");
        _SyncDictionary();
    }
}
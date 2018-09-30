using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PositionLogger : MonoBehaviour {
    public float EntryInterval_s = 0.5f;
    public float LogInterval_s = 10.0f;

    class PositionEntry {
        public Vector3 Position;
        public float Time_s;

        public PositionEntry(Vector2 position) {
            Time_s = Time.time;
            Position = position;
        }
    }

    private List<PositionEntry> _entries = new List<PositionEntry>();

    private float _lastEntryTime_s = 0.0f;
    private float _lastLogTime_s = 0.0f;

    private APIClient m_ApiClient;

    void Awake() {
        m_ApiClient = GetComponent<APIClient>();
        _lastLogTime_s = Time.time;
        _addCurrentPosition();
    }

    void _addCurrentPosition() {
        _entries.Add(new PositionEntry(transform.position));
        _lastEntryTime_s = Time.time;
    }

    void LateUpdate() {
        if ((Time.time - _lastEntryTime_s) > EntryInterval_s) {
            _addCurrentPosition();
        }

        if ((Time.time - _lastLogTime_s) > LogInterval_s) {
            PositionEntry[] entries = _entries.ToArray();
            foreach (PositionEntry entry in entries) {
                Debug.Log("pos " + entry.Position + " at " + entry.Time_s);
            }

            _lastLogTime_s = Time.time;
            
            _entries.Clear();
            _addCurrentPosition();
        }
    }
}
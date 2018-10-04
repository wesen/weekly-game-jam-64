using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PositionLogger : MonoBehaviour {
    public float EntryInterval_s = 0.5f;
    public float LogInterval_s = 3.0f;

    class PositionEntry {
        public Vector2 Position;
        public float Time_s;

        public PositionEntry(Vector2 position) {
            Time_s = Time.time;
            Position = position;
        }
    }

    private List<PositionEntry> _entries = new List<PositionEntry>();

    private float _lastEntryTime_s = 0.0f;
    private float _lastLogTime_s = 0.0f;

    private APIClient m_ApiClient = new APIClient();

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
            _lastLogTime_s = Time.time;

            if (entries.Length > 0) {
                bool isStatic = true;
                for (int i = 1; i < entries.Length; i++) {
                    if (entries[i].Position != entries[0].Position) {
                        isStatic = false;
                        break;
                    }
                }

                if (!isStatic) {
                    ArrayList movements = new ArrayList();
                    foreach (PositionEntry entry in entries) {
                        movements.Add(new float[] {entry.Position.x, entry.Position.y});
                    }

                    ArrayList interactions = new ArrayList();

                    m_ApiClient.RunSendPath(0, 0, "foobar",
                        SceneManager.GetActiveScene().name,
                        movements, interactions);
                }
            }

            _entries.Clear();
            _addCurrentPosition();
        }
    }
}
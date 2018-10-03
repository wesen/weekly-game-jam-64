using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour {
    private void Awake() {
    }

    public void ExecuteMoves(GhostInformation information) {
        StartCoroutine(CR_ExecuteMoves(information));
    }

    private IEnumerator CR_ExecuteMoves(GhostInformation _information) {
        int idx = 0;
        if (_information.Movement.Length >= 2) {
            Vector2 start = _information.Movement[idx++];
            Vector2 target = _information.Movement[idx++];
            transform.position = start;

            while (idx < _information.Movement.Length) {
                Debug.Log("Moving from " + start + " to " + target);
                for (float t = 0.0f; t < 0.5f; t += Time.deltaTime) {
                    transform.position = Vector2.Lerp(start, target, t * 2);
                    yield return null;
                }

                start = target;
                target = _information.Movement[idx++];
            }
        }

        Debug.Log("Finished moving");

        Destroy(gameObject);
    }

}
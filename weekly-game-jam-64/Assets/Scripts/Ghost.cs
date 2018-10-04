using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour {
    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
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
                for (float t = 0.0f; t < 0.5f; t += Time.deltaTime) {
                    Vector2 prevPosition = transform.position;
                    Vector2 newPosition = Vector2.Lerp(start, target, t * 2);
                    Vector2 _moveDelta = newPosition - prevPosition;

                    if (_moveDelta.x > 0) {
                        _animator.Play("player_walking_right");
                    } else if (_moveDelta.x < 0) {
                        _animator.Play("player_walking_left");
                    } else if (_moveDelta.y < 0) {
                        _animator.Play("player_walking_frontal");
                    } else if (_moveDelta.y > 0) {
                        _animator.Play("player_walking_up");
                    } else {
                        _animator.Play("player_idle_frontal");
                    }

                    transform.position = newPosition;
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
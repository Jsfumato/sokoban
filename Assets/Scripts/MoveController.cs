using System.Collections;
using UnityEngine;

public class MoveController : MonoBehaviour {
    public float moveTime = 0.5f;
    public LayerMask blockingLayers;

    protected BoxCollider2D collider;
    protected Rigidbody2D rigidbody;
    protected bool moving;

    protected virtual void Start() {
        collider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public enum Direction {
        NONE = 0,

        LEFT,
        UP,
        DOWN,
        RIGHT
    }

    public bool Move(Direction dirToMove, out RaycastHit2D hit) {
        float xDir = 0.0f;
        float yDir = 0.0f;

        switch (dirToMove) {
            case Direction.RIGHT:
                xDir = 0.5f;
                yDir = 0.0f;
                break;
            case Direction.LEFT:
                xDir = -0.5f;
                yDir = 0.0f;
                break;
            case Direction.UP:
                xDir = 0.0f;
                yDir = 0.5f;
                break;
            case Direction.DOWN:
                xDir = 0.0f;
                yDir = -0.5f;
                break;
        }

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        int layer = gameObject.layer;
        gameObject.layer = 0;
        hit = Physics2D.Linecast(start, end, blockingLayers);
        gameObject.layer = layer;

        if (hit.transform == null) {
            moving = true;

            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    //
    public IEnumerator SmoothMovement(Vector3 end) {
        float remainingDistance = (transform.position - end).sqrMagnitude;

        while (remainingDistance > float.Epsilon) {
            Vector3 newPosition = Vector3.MoveTowards(rigidbody.position, end, (1 / moveTime) * Time.deltaTime);
            rigidbody.MovePosition(newPosition);
            remainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

        MoveEnded();
    }

    //
    protected virtual void MoveEnded() {
        moving = false;
    }
}
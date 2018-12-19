using UnityEngine;

public class PlayerController : MoveController {
    public LayerMask pushingLayers;

    public Animator animator;

    protected void Awake() {
        animator = GetComponent<Animator>();

        base.Start();
    }

    public void MovePlayer(Direction dir) {
        if (moving)
            return;

        animator.SetBool("walkLeft", false);
        animator.SetBool("walkRight", false);
        animator.SetBool("walkUp", false);
        animator.SetBool("walkDown", false);

        //if (horizontal != 0 || vertical != 0) {
        switch (dir) {
            case Direction.RIGHT:
                animator.SetTrigger("walkRight");
                break;
            case Direction.LEFT:
                animator.SetTrigger("walkLeft");
                break;
            case Direction.UP:
                animator.SetTrigger("walkUp");
                break;
            case Direction.DOWN:
                animator.SetTrigger("walkDown");
                break;
        }

        //
        if (dir != Direction.NONE) {
            RaycastHit2D hit = new RaycastHit2D();
            if (Push(dir, out hit)) {
                Move(dir, out hit);
            }
        }
    }

    protected override void MoveEnded() {
        animator.SetBool("walkLeft", false);
        animator.SetBool("walkRight", false);
        animator.SetBool("walkUp", false);
        animator.SetBool("walkDown", false);

        base.MoveEnded();
    }

    private bool Push(Direction dirToPush, out RaycastHit2D hit) {
        float xDir = 0.0f;
        float yDir = 0.0f;

        switch (dirToPush) {
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

        //
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        collider.enabled = false;
        hit = Physics2D.Linecast(start, end, pushingLayers);
        collider.enabled = true;

        if (hit.transform == null) {
            return true;
        }

        //
        return hit.transform.GetComponent<MoveController>().Move(dirToPush, out hit);
    }

}
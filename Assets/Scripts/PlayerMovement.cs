using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 1f;

    [HideInInspector]
    public bool isGrounded = false;

    GhostMaster ghostMaster;
    Animator animator;

	void Start()
	{
        ghostMaster = GameObject.Find("logic").GetComponent<GhostMaster>();
        animator = transform.FindChild("sprite").GetComponent<Animator>();
	}

	void FixedUpdate()
	{
        if (isGrounded == false) {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector2 force = new Vector2(h, v);
            force.Normalize();
            rigidbody2D.velocity = force * moveSpeed;
            ghostMaster.AddPosition(Time.fixedDeltaTime, transform.position);
        } else {
            rigidbody2D.velocity = Vector2.zero;
        }
        UpdateAnimation();
	}

    void UpdateAnimation()
    {
        animator.SetFloat("Velocity", rigidbody2D.velocity.magnitude);
        if (rigidbody2D.velocity.magnitude != 0f) {
            if (Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Abs(rigidbody2D.velocity.y)) {
                if (rigidbody2D.velocity.x > 0) {
                    animator.SetInteger("Direction", 3);
                } else {
                    animator.SetInteger("Direction", 1);
                }
            } else {
                if (rigidbody2D.velocity.y > 0) {
                    animator.SetInteger("Direction", 2);
                } else {
                    animator.SetInteger("Direction", 0);
                }
            }
        }
    }

    public void Reset()
    {
        rigidbody2D.rigidbody2D.position = new Vector2(-4.95f, -0.32f);
        rigidbody2D.velocity = Vector2.zero;
        isGrounded = false;
        GetComponent<PlayerShoot>().isGrounded = false;
    }
}

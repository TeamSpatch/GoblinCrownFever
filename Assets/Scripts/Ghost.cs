using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Ghost : MonoBehaviour
{
    public float time;
    public Dictionary<float, Vector3> positions;
    public Dictionary<float, Vector2> shots;
    public bool isRed;

    [HideInInspector]
    public bool isDead;

    float timer;
    Transform pivot;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Start()
    {
        isDead = false;
        pivot = GameObject.Find("pivot").transform;
        spriteRenderer = transform.FindChild("sprite").GetComponent<SpriteRenderer>();
        animator = transform.FindChild("sprite").GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isDead == false) {
            if (positions.ContainsKey(timer)) {
                UpdateAnimation(pivot.position - positions[timer] - transform.position);
                transform.position = new Vector3(pivot.position.x - positions[timer].x, positions[timer].y, 0f);
                if (shots.ContainsKey(timer)) {
                    Vector3 pos = new Vector3(pivot.position.x - shots[timer].x, shots[timer].y, 0f);
                    GameObject explosion = Instantiate(Resources.Load("target"), pos, Quaternion.identity) as GameObject;
                    explosion.GetComponent<Target>().mirror = true;
                }
                timer += Time.fixedDeltaTime;
            } else {
                if (isRed && isDead == false) {
                    GameObject.Find("logic").GetComponent<LevelDirector>().GhostGotCrown();
                }
                animator.SetFloat("Velocity", 0f);
                spriteRenderer.enabled = false;
            }
        }
    }

    void UpdateAnimation(Vector3 direction)
    {
        animator.SetFloat("Velocity", (direction.magnitude > 0.40f ? direction.magnitude : 0f));
        if (direction.magnitude != 0f) {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
                if (direction.x > 0) {
                    animator.SetInteger("Direction", 3);
                } else {
                    animator.SetInteger("Direction", 1);
                }
            } else {
                if (direction.y > 0) {
                    animator.SetInteger("Direction", 2);
                } else {
                    animator.SetInteger("Direction", 0);
                }
            }
        }
    }

    public void GetHit()
    {
        isDead = true;
        spriteRenderer.enabled = false;
    }

    public void Reset()
    {
        timer = 0f;
        transform.position = positions[0f];
        spriteRenderer.enabled = true;
        isDead = false;
    }
}

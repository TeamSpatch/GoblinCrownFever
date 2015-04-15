using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public float reloadDuration = 1f;
    public float shootingAnimationDuration = 0.5f;
    public float maxRange = 5f;
    public int maxAmmo = 10;

    [HideInInspector]
    public int ammo = 5;
    [HideInInspector]
    public bool isGrounded = false;

    float cooldown;
    float shooting;
    GhostMaster ghostMaster;
    AudioSource audioSource;
    Animator animator;

    void Start()
    {
        ghostMaster = GameObject.Find("logic").GetComponent<GhostMaster>();
        audioSource = GetComponent<AudioSource>();
        animator = transform.FindChild("sprite").GetComponent<Animator>();
    }
	
    void FixedUpdate()
    {
        if (shooting > 0f) {
            shooting -= Time.fixedDeltaTime;
            if (shooting <= 0f) {
                animator.SetBool("Shooting", false);
            }
        }
        if (Input.GetButton("Fire1") && cooldown <= 0f && isGrounded == false) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Vector3 trajectory = pos - transform.position;
            trajectory.z = 0f;
            if (ammo > 0 && trajectory.magnitude <= maxRange) {
                cooldown = reloadDuration;
                Instantiate(Resources.Load("target"), pos, Quaternion.identity);
                Instantiate(Resources.Load("smoke"), transform.position + new Vector3(0.1f, 0f, 0f), Quaternion.identity);
                ghostMaster.AddShot(Time.fixedDeltaTime, pos);
                audioSource.PlayOneShot(Resources.Load("shoot") as AudioClip);
                animator.SetBool("Shooting", true);
                shooting = shootingAnimationDuration;
                --ammo;
            } else {
                audioSource.PlayOneShot(Resources.Load("clink") as AudioClip);
                cooldown = 0.1f;
            }
        } else if (cooldown > 0f) {
            cooldown -= Time.fixedDeltaTime;
        }
    }

    public void Reset()
    {
        isGrounded = false;
    }
}

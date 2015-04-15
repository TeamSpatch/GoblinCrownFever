using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    public float landingTime = 2f;
    public float lifetime = 2.3f;

    [HideInInspector]
    public bool mirror = false;

    float timer;
    bool fired = false;

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= landingTime && fired == false) {
            Object explosion = (mirror ? Resources.Load("explosionMirror") : Resources.Load("explosion"));
            Instantiate(explosion, transform.position, Quaternion.identity);
            fired = true;
        } else if (timer >= lifetime) {
            Destroy(gameObject);
        }
    }
}

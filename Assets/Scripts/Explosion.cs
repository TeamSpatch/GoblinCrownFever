using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public float soundTime = 0.2f;
    public float startUpTime = 0.3f;
    public float effectLifetime = 0.5f;
    public float animationLifetime = 0.8f;

    float timer;
    AudioSource audioSource;
    bool soundPlayed;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        soundPlayed = false;
    }

    void Update()
    {
        if (timer >= animationLifetime) {
            Destroy(gameObject);
        } else if (timer > effectLifetime) {
            GetComponent<Collider2D>().enabled = false;
        } else if (timer >= startUpTime) {
            GetComponent<Collider2D>().enabled = true;
        } else if (timer >= soundTime) {
            if (soundPlayed == false) {
                audioSource.PlayOneShot(Resources.Load("boom") as AudioClip);
                soundPlayed = true;
            }
        }
        timer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Trigger") {
            other.GetComponent<Trigger>().GetHit();
        } else if (other.tag == "Ghost") {
            other.GetComponent<Ghost>().GetHit();
        } else if (other.tag == "Player") {
            other.GetComponent<Player>().GetHit();
        }
    }
}

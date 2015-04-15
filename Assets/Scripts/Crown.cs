using UnityEngine;
using System.Collections;

public class Crown : MonoBehaviour
{
    GameObject shield;

    void Start()
    {
        shield = transform.FindChild("sprite/shield").gameObject;
    }

    public void Unlock()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        shield.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Lock()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        shield.GetComponent<SpriteRenderer>().enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            GameObject.Find("logic").GetComponent<LevelDirector>().GotTheCrown();
        }
    }
}

using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour
{
    [HideInInspector]
    public GameObject dropZone = null;

    public void TriggerGetHit()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        transform.FindChild("sprite/shield").gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            GameObject.Find("logic").GetComponent<LevelDirector>().KeyGotPicked();
            Destroy(gameObject);
        }
    }
}

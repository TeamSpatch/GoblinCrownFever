using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int health = 1;

    [HideInInspector]
    public bool immobilism = true;
    [HideInInspector]
    public bool immobilismRedGhost = false;

    public void GetHit()
    {
        health--;
        if (health <= 0) {
            GameObject.Find("logic").GetComponent<LevelDirector>().PlayerGotHit();
            Destroy(gameObject);
        }
    }

    public void Reset()
    {
        immobilism = true;
        immobilismRedGhost = false;
    }
}

using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour
{
    [HideInInspector]
    public GameObject dropZone = null;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            PlayerShoot playerShoot = other.GetComponent<PlayerShoot>();
            if (playerShoot.ammo < playerShoot.maxAmmo) {
                playerShoot.ammo++;
                GameObject.Find("logic").GetComponent<LevelDirector>().AmmoGetPicked();
                Clean();
            }
        }
    }

    public void Clean()
    {
        dropZone.GetComponent<DropZone>().linked = null;
        Destroy(gameObject);
    }
}

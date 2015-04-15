using UnityEngine;
using System.Collections;

public class EndZone : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") {
            other.GetComponent<Player>().immobilism = false;
            if (other.GetComponent<Player>().immobilismRedGhost == true) {
                GameObject.Find("logic").GetComponent<LevelDirector>().GhostGotCrown();
            }
        }
    }
}

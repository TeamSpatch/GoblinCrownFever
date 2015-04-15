using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour
{
    [HideInInspector]
    public GameObject dropZone = null;
    [HideInInspector]
    public string color;

    bool dead = false;

    public void GetHit()
    {
        if (dead == false) {
            dead = true;
            GameObject.Find("key" + color).GetComponent<Key>().TriggerGetHit();
            dropZone.GetComponent<DropZone>().linked = null;
            Destroy(gameObject);
        }
    }
}

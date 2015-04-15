using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour
{
	public void DoMirror()
    {
        GameObject pivot = GameObject.Find("pivot");
        GameObject right = GameObject.Find(name + "RightSide");
        int count = transform.childCount;
        for (int i = 0; i < count; i++) {
            GameObject original = transform.GetChild(i).gameObject;
            GameObject clone = Instantiate(original) as GameObject;
            clone.transform.parent = right.transform;
            Vector3 position = clone.transform.position;
            position.x = pivot.transform.position.x - position.x;
            clone.transform.position = position;
            DropZone drop = clone.GetComponent<DropZone>();
            if (drop) {
                drop.mirror = original;
                original.GetComponent<DropZone>().mirror = clone;
            }
        }
	}
}

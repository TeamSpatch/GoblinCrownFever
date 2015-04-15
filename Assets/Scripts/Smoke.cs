using UnityEngine;
using System.Collections;

public class Smoke : MonoBehaviour
{
    public float duration = 1f;

    float timer;

	void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration) {
            Destroy(gameObject);
        }
	}
}

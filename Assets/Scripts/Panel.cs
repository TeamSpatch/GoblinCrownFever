using UnityEngine;
using System.Collections;

public class Panel : MonoBehaviour
{
    public Vector3 destination;
    public float animationTime = 2f;

    SpriteRenderer spriteRenderer;
    float timer;

    void Start()
    {
        spriteRenderer = transform.FindChild("sprite").GetComponent<SpriteRenderer>();
    }

	void Update()
    {
        Color color = spriteRenderer.color;
        color.a = Mathf.Lerp(color.a, 1f, Time.deltaTime * 1.5f);
        spriteRenderer.color = color;
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 2f);
        timer += Time.deltaTime;
        if (timer >= animationTime) {
            Destroy(gameObject);
        }
	}
}

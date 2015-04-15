using UnityEngine;
using System.Collections;

public class Levitate : MonoBehaviour
{
    public float animationPeriod = 1f;
    public float animationLength = 0.5f;
    public float animationForce = 0.02f;

    Transform sprite;
    float timer;
    Vector3 highPosition;
    Vector3 lowPosition;

    void Start()
    {
        sprite = transform.FindChild("sprite");
        lowPosition = sprite.position;
        highPosition = lowPosition;
        highPosition.y += animationLength;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= animationPeriod * 2f) {
            timer = 0f;
        } else if (timer >= animationPeriod) {
            sprite.position = Vector3.Lerp(sprite.position, lowPosition, animationForce);
        } else {
            sprite.position = Vector3.Lerp(sprite.position, highPosition, animationForce);
        }
    }
}

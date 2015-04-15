using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour
{
    public float speed = 1f;

    Vector3 direction;

    void Start()
    {
        direction = new Vector3(0f, -1f, 0f);
    }

    void Update()
    {
        if (Input.anyKeyDown) {
            GameObject.Find("logic").GetComponent<LevelDirector>().Spawn(transform.position);
            Destroy(gameObject);
        } else {
            transform.position += direction * Time.deltaTime * speed;
            if (transform.position.y <= -2.7f || transform.position.y >= 2.1f) {
                direction.y *= -1;
            }
        }
    }
}

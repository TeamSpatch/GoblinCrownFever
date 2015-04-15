using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    public float animationDuration = 2f;
    public Sprite left;
    public Sprite center;
    public Sprite right;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = transform.FindChild("sprite").GetComponent<SpriteRenderer>();
    }

    public void SetTimer(float timer)
    {
        timer = timer % (4 * animationDuration);
        if (timer >= 3 * animationDuration) {
            spriteRenderer.sprite = right;
        } else if (timer >= 2 * animationDuration) {
            spriteRenderer.sprite = center;
        } else if (timer >= 1 * animationDuration) {
            spriteRenderer.sprite = left;
        } else {
            spriteRenderer.sprite = center;
        }
    }

    public void ShootOnPlayer(Vector3 pos)
    {
        spriteRenderer.sprite = left;
        Instantiate(Resources.Load("smoke"), transform.position + new Vector3(-0.3f, -0.05f, 0f), Quaternion.identity);
        Instantiate(Resources.Load("explosionMirror"), pos, Quaternion.identity);
    }

    public void Reset()
    {
        spriteRenderer.sprite = center;
    }
}

using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{
    public Texture2D cursorOk;
    public Texture2D cursorKo;
    public Texture2D score;
    public Texture2D time;
    public Texture2D ammo;

    PlayerShoot playerShoot;
    LevelDirector levelDirector;
    bool isPlaying;

    void Start()
    {
        levelDirector = GetComponent<LevelDirector>();
        Screen.showCursor = false;
        isPlaying = false;
    }

    public void GameStarts()
    {
        playerShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();
        isPlaying = true;
    }

    public void GameStops()
    {
        isPlaying = false;
    }

    void OnGUI()
    {
        if (isPlaying) {
            ShowCursor();
            ShowAmmo();
            ShowTimer();
        }
        ShowScore();
    }

    void ShowCursor()
    {
        Texture2D cursor = cursorOk;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        if ((pos - playerShoot.transform.position).magnitude > playerShoot.maxRange) {
            cursor = cursorKo;
        }
        Rect rect = new Rect(Input.mousePosition.x - cursor.width / 2f, Screen.height - Input.mousePosition.y - cursor.height / 2f, cursor.width, cursor.height);
        GUI.Label(rect, cursor);
    }

    void ShowAmmo()
    {
        for (int i = 0; i < playerShoot.ammo; i++) {
            Rect rect = new Rect(22f * i, 38f, 32f, 32f);
            GUI.Label(rect, ammo);
        }
    }

    void ShowTimer()
    {
        for (int i = 1; i < levelDirector.timer; i += 2) {
            Rect rect = new Rect(Screen.width / 2f - 14f + 12f * i, 20f, 32f, 32f);
            GUI.Label(rect, time);
        }
    }

    void ShowScore()
    {
        for (int i = 0; i < levelDirector.score; i++) {
            Rect rect = new Rect(26f * i, 10f, 32f, 32f);
            GUI.Label(rect, score);
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostMaster : MonoBehaviour
{
    float currentTime;
    Dictionary<float, Vector3> currentPositions;
    Dictionary<float, Vector2> currentShots;
    Transform pivot;
    LevelDirector levelDirector;

    void Start()
    {
        currentTime = 0f;
        currentPositions = new Dictionary<float, Vector3>();
        currentShots = new Dictionary<float, Vector2>();
        pivot = GameObject.Find("pivot").transform;
        levelDirector = GetComponent<LevelDirector>();
    }

    public void AddPosition(float time, Vector3 position)
    {
        currentPositions.Add(currentTime, position);
        currentTime += time;
    }

    public void AddShot(float time, Vector3 position)
    {
        currentShots.Add(currentTime, position);
    }

    public void ResetGame()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts) {
            Destroy(ghost);
        }
        currentTime = 0f;
        currentPositions.Clear();
        currentShots.Clear();
    }

    public void GotTheCrown()
    {
        ResetGhosts();
        Vector3 pos = new Vector3(pivot.position.x - currentPositions[0f].x, currentPositions[0f].y, 0f);
        Object res = (levelDirector.score > 0 && levelDirector.score % 3 == 0 ? Resources.Load("ghostRed") : Resources.Load("ghostBlue"));
        GameObject g = Instantiate(res, pos, Quaternion.identity) as GameObject;
        Ghost ghost = g.GetComponent<Ghost>();
        ghost.time = currentTime;
        ghost.positions = new Dictionary<float, Vector3>(currentPositions);
        ghost.shots = new Dictionary<float, Vector2>(currentShots);
        currentTime = 0f;
        currentPositions.Clear();
        currentShots.Clear();
    }

    public void DeactivateGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject g in ghosts) {
            Ghost ghost = g.GetComponent<Ghost>();
            if (ghost) {
                ghost.isDead = true;
            }
        }
    }

    void ResetGhosts()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject g in ghosts) {
            Ghost ghost = g.GetComponent<Ghost>();
            if (ghost) {
                ghost.Reset();
            }
        }
    }
}

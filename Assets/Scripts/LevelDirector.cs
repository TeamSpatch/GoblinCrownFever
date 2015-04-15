using UnityEngine;
using System.Collections;

public class LevelDirector : MonoBehaviour
{
    public int ammoPerLevel = 4;
    public float levelTimer = 30f;

    [HideInInspector]
    public float timer;
    [HideInInspector]
    public int score;

    int doorLocks;
    GhostMaster ghostMaster;
    Transform dropZones;
    Transform dropZonesRightSide;
    GameObject[] previousTriggers;
    Turret turret;

    void Start()
    {
        previousTriggers = new GameObject[3];
        ghostMaster = GetComponent<GhostMaster>();
        GameObject.Find("maze").GetComponent<Mirror>().DoMirror();
        dropZones = GameObject.Find("dropZones").transform;
        dropZones.gameObject.GetComponent<Mirror>().DoMirror();
        dropZonesRightSide = GameObject.Find("dropZonesRightSide").transform;
        if (dropZones.childCount < 6 + ammoPerLevel) {
            Debug.Log("There's not enough drop zones, you need at least " + (3 + ammoPerLevel).ToString());
        }
        if (dropZonesRightSide.childCount < 3) {
            Debug.Log("There's not enough drop zones, you need at least 3");
        }
        timer = -1f;
        turret = GameObject.Find("turret").GetComponent<Turret>();
        GameObject.Find("title").GetComponent<SpriteRenderer>().enabled = true;
    }

    void Update()
    {
        if (timer > 0f) {
            timer -= Time.deltaTime;
            turret.SetTimer(timer);
            if (timer <= 0f) {
                timer = 0f;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player) {
                    TimeRanOut();
                }
            }
        }
    }

    void SpawnAmmo()
    {
        CleanAmmo();
        for (int i = 0; i < ammoPerLevel; i++) {
            GameObject drop = GetRandomFreeDropZone(dropZones);
            GameObject pill = Instantiate(Resources.Load("ammo"), drop.transform.position, Quaternion.identity) as GameObject;
            drop.GetComponent<DropZone>().linked = pill;
            pill.GetComponent<Ammo>().dropZone = drop;
        }
    }

    void SpawnTriggers()
    {
        string[] colors = new string[3] { "Red", "Green", "Blue" };
        for (int i = 0; i < 3; i++) {
            GameObject drop = GetRandomFreeDropZone(dropZonesRightSide);
            GameObject trigger = Instantiate(Resources.Load("trigger" + colors[i]), drop.transform.position, Quaternion.identity) as GameObject;
            drop.GetComponent<DropZone>().linked = trigger;
            trigger.GetComponent<Trigger>().dropZone = drop;
            trigger.GetComponent<Trigger>().color = colors[i];
            previousTriggers[i] = drop.GetComponent<DropZone>().mirror;
        }
        doorLocks = 3;
    }

    void SpawnKeys()
    {
        string[] colors = new string[3] { "Red", "Green", "Blue" };
        for (int i = 0; i < 3; i++) {
            GameObject drop;
            if (score > 0) {
                drop = previousTriggers[i];
            } else {
                drop = GetRandomFreeDropZone(dropZones);
            }
            GameObject key = Instantiate(Resources.Load("key" + colors[i]), drop.transform.position, Quaternion.identity) as GameObject;
            key.name = "key" + colors[i];
            drop.GetComponent<DropZone>().linked = key;
            key.GetComponent<Key>().dropZone = drop;
        }
    }

    GameObject GetRandomFreeDropZone(Transform drops)
    {
        GameObject free = null;
        while (free == null) {
            int count = Random.Range(0, drops.childCount - 1);
            GameObject child = drops.GetChild(count).gameObject;
            if (child.GetComponent<DropZone>().linked == null) {
                free = child;
            }
        }
        return free;
    }

    public void KeyGotPicked()
    {
        doorLocks--;
        if (doorLocks == 0) {
            GameObject.Find("crown").GetComponent<Crown>().Unlock();
        }
    }

    public void AmmoGetPicked()
    {
        if (GameObject.FindGameObjectsWithTag("Ammo").Length == 1) {
            SpawnAmmo();
        }
    }

    public void GotTheCrown()
    {
        Instantiate(Resources.Load("panelLevelUp"));
        GetComponent<AudioSource>().PlayOneShot(Resources.Load("levelUp") as AudioClip);
        StartCoroutine(WinLevel());
    }

    IEnumerator WinLevel()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().isGrounded = true;
        player.GetComponent<PlayerShoot>().isGrounded = true;
        ghostMaster.DeactivateGhosts();
        timer = -1f;
        score++;
        yield return new WaitForSeconds(2.5f);
        ghostMaster.GotTheCrown();
        GameObject.Find("crown").GetComponent<Crown>().Lock();
        CleanLevel();
        SpawnKeys();
        SpawnAmmo();
        SpawnTriggers();
        player.GetComponent<PlayerMovement>().Reset();
        player.GetComponent<PlayerShoot>().Reset();
        player.GetComponent<Player>().Reset();
        GameObject.Find("endZone").transform.position = player.rigidbody2D.position;
        timer = levelTimer;
        GameObject.Find("turret").GetComponent<Turret>().Reset();
    }

    public void GhostGotCrown()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<Player>().immobilism == true) {
            player.GetComponent<Player>().immobilismRedGhost = true;
        } else {
            Instantiate(Resources.Load("panelGameOver"));
            StartCoroutine(ResetGame());
        }
    }

    public void PlayerGotHit()
    {
        if (timer > 0f) {
            Instantiate(Resources.Load("panelGameOver"));
        }
        StartCoroutine(ResetGame());
    }

    void TimeRanOut()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player.immobilism == true) {
            if (score == 0) {
                Instantiate(Resources.Load("panelCongrats1"));
            } else if (score == 1) {
                Instantiate(Resources.Load("panelCongrats2"));
            } else {
                Instantiate(Resources.Load("panelCongrats3"));
            }
        } else {
            player.GetComponent<PlayerMovement>().isGrounded = true;
            player.GetComponent<PlayerShoot>().isGrounded = true;
            Instantiate(Resources.Load("panelTimeUp"));
            GameObject.Find("turret").GetComponent<Turret>().ShootOnPlayer(player.transform.position);
        }
        StartCoroutine(ResetGame());
    }

    IEnumerator ResetGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player) {
            player.GetComponent<PlayerMovement>().isGrounded = true;
            player.GetComponent<PlayerShoot>().isGrounded = true;
        }
        GetComponent<HUD>().GameStops();
        ghostMaster.DeactivateGhosts();
        yield return new WaitForSeconds(2.5f);
        CleanLevel();
        Destroy(player);
        ghostMaster.ResetGame();
        GameObject.Find("title").GetComponent<SpriteRenderer>().enabled = true;
        if (GameObject.FindGameObjectWithTag("Spawn") == null) {
            Instantiate(Resources.Load("spawn"));
        }
    }

    public void Spawn(Vector3 pos)
    {
        SpawnKeys();
        SpawnTriggers();
        SpawnAmmo();
        timer = levelTimer;
        score = 0;
        GameObject.Find("crown").GetComponent<Crown>().Lock();
        GameObject.Find("turret").GetComponent<Turret>().Reset();
        GameObject.Find("endZone").transform.position = pos;
        GameObject.Find("title").GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(Resources.Load("player"), pos, Quaternion.identity);
        GetComponent<HUD>().GameStarts();
    }

    void CleanAmmo()
    {
        GameObject[] ammos = GameObject.FindGameObjectsWithTag("Ammo");
        foreach (GameObject a in ammos) {
            Ammo ammo = a.GetComponent<Ammo>();
            if (ammo) {
                ammo.Clean();
            }
        }
    }

    void CleanLevel()
    {
        CleanAmmo();
        string[] tags = new string[3] { "Key", "Trigger", "Explosion" };
        for (int i = 0; i < tags.Length; i++) {
            GameObject[] items = GameObject.FindGameObjectsWithTag(tags[i]);
            foreach (GameObject item in items) {
                Destroy(item);
            }
        }
    }
}

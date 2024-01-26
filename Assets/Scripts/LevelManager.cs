using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Zombie Zombie { get; set; }
    [SerializeField] GameObject SpawnPoint;
    public Camera camera;
    public List<GameObject> zombiePrefabs;
    public GameObject heightLine;
    public GameObject playableArea;
    public float score;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    private float countdownTimer = 60f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        countdownTimer -= Time.deltaTime;

        countdownTimer = Mathf.Max(countdownTimer, 0f);

        int seconds = Mathf.FloorToInt(countdownTimer);

        if (timerText != null)
        {
            timerText.SetText("Timer " + seconds.ToString());
        }

        if (countdownTimer <= 0f)
        {
            // TODO: Finish the game...
        }
    }

    private void OnEnable()
    {
        Zombie.OnZombieStick += Zombie_OnZombieStick;
    }

    private void Zombie_OnZombieStick(Zombie zombie)
    {
        string formattedFloat = heightLine.transform.position.y.ToString("F2");
        score = float.Parse(formattedFloat);
        if (score > 0) scoreText.SetText("Score " + score);

        zombie.enabled = false;
        Instantiate(zombiePrefabs[Random.Range(0, zombiePrefabs.Count)], SpawnPoint.transform.position,
            Quaternion.identity);
    }

    private void OnDisable()
    {
        Zombie.OnZombieStick -= Zombie_OnZombieStick;
    }
}
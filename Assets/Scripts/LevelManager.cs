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
    [SerializeField] GameObject[] SpawnPoints;
    public Camera camera;
    public List<GameObject> zombiePrefabs;
    public GameObject heightLine;
    public GameObject playableArea;
    public float score;

    public TextMeshProUGUI timerTexts;
    public TextMeshProUGUI scoreText;
    private float countdownTimer = 60f;

    [SerializeField] private int currentPlayer = 0;
    [SerializeField] GameObject WinScreen;
    [SerializeField] TMPro.TMP_Text winnner;
    public ParticleSystem Choose;
    public GameObject SpaceToolTIp;

    private void Awake()
    {
        Time.timeScale = 1;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Instantiate(zombiePrefabs[Random.Range(0, zombiePrefabs.Count)], SpawnPoints[currentPlayer].transform.position,
            Quaternion.identity);
    }

    private void Update()
    {
        if (Zombie.bones[0].bodyType == RigidbodyType2D.Static) return;

        countdownTimer -= Time.deltaTime;

        countdownTimer = Mathf.Max(countdownTimer, 0f);

        int seconds = Mathf.FloorToInt(countdownTimer);

        if (timerTexts != null)
        {
            timerTexts.SetText(seconds.ToString()+" s");
        }

        if (countdownTimer <= 0f)
        {
            WinScreen.SetActive(true);
            winnner.SetText(currentPlayer == 0 ? "RIGHT" : "LEFT");
            Time.timeScale = 0;
        }
    }

    private void OnEnable()
    {
        Zombie.OnZombieStick += Zombie_OnZombieStick;
    }

    private void Zombie_OnZombieStick(Zombie zombie)
    {
        string formattedFloat = heightLine.transform.position.y.ToString("F2");
        score = (int)(float.Parse(formattedFloat) * 100);
        if (score > 0) scoreText.SetText("Score " + score);

        zombie.enabled = false;
        countdownTimer = 60f;
        if (currentPlayer == 0) currentPlayer = 1;
        else currentPlayer = 0;

        Instantiate(zombiePrefabs[0], SpawnPoints[currentPlayer].transform.position,
            Quaternion.identity);
        zombiePrefabs.RemoveAt(0);
    }

    private void OnDisable()
    {
        Zombie.OnZombieStick -= Zombie_OnZombieStick;
    }
}
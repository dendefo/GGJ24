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

    public TextMeshProUGUI[] timerTexts;
    public TextMeshProUGUI scoreText;
    private float[] countdownTimer = { 60f, 60f };

    [SerializeField] private int currentPlayer = 0;

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

        Instantiate(zombiePrefabs[Random.Range(0, zombiePrefabs.Count)], SpawnPoints[currentPlayer].transform.position,
            Quaternion.identity);
    }

    private void Update()
    {
        countdownTimer[currentPlayer] -= Time.deltaTime;

        countdownTimer[currentPlayer] = Mathf.Max(countdownTimer[currentPlayer], 0f);

        int seconds = Mathf.FloorToInt(countdownTimer[currentPlayer]);

        if (timerTexts[currentPlayer] != null)
        {
            timerTexts[currentPlayer].SetText(seconds.ToString());
        }

        if (countdownTimer[currentPlayer] <= 0f)
        {
            // TODO: Finish the game...
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
        score = float.Parse(formattedFloat);
        if (score > 0) scoreText.SetText("Score " + score);

        zombie.enabled = false;
        
        if (currentPlayer == 0) currentPlayer = 1;
        else currentPlayer = 0;
        
        Instantiate(zombiePrefabs[Random.Range(0, zombiePrefabs.Count)], SpawnPoints[currentPlayer].transform.position,
            Quaternion.identity);
    }

    private void OnDisable()
    {
        Zombie.OnZombieStick -= Zombie_OnZombieStick;
    }
}
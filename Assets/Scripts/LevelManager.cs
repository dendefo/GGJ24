using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Zombie Zombie { get; set; }
    [SerializeField] GameObject SpawnPoint;
    public Camera camera;
    public List<GameObject> zombiePrefabs;
    public GameObject heightLine;
    public GameObject playableArea;

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

    private void OnEnable()
    {
        Zombie.OnZombieStick += Zombie_OnZombieStick;
    }

    private void Zombie_OnZombieStick(Zombie zombie)
    {
        zombie.enabled = false;
        Instantiate(zombiePrefabs[Random.Range(0, zombiePrefabs.Count)], SpawnPoint.transform.position,
            Quaternion.identity);
    }

    private void OnDisable()
    {
        Zombie.OnZombieStick -= Zombie_OnZombieStick;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    public delegate void ZombieStick(Zombie zombie);
    public static event ZombieStick OnZombieStick;
    private void Awake()
    {
        LevelManager.Instance.Zombie = this;
    }

    public void Stick()
    {
        OnZombieStick?.Invoke(this);
    }
}

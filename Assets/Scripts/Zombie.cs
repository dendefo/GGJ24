using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private void Awake()
    {
        LevelManager.Instance.Zombie = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] Rigidbody2D leftHand;
    [SerializeField] float speed;
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
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            leftHand.AddForce(Vector2.up * speed,ForceMode2D.Impulse);
        }
    }
}

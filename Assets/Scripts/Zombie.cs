using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] Rigidbody2D leftHand;
    [SerializeField] Rigidbody2D rightHand;
    [SerializeField] Rigidbody2D leftFoot;
    [SerializeField] Rigidbody2D rightFoot;
    [SerializeField] float speed;

    public delegate void ZombieStick(Zombie zombie);

    public static event ZombieStick OnZombieStick;
    private Rigidbody2D limb;

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
            limb = leftHand;
            leftHand.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            limb = rightHand;
            rightHand.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            limb = leftFoot;
            leftFoot.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.C))
        {
            limb = rightFoot;
            rightFoot.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        }
    }
}
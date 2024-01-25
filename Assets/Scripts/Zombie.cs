using System;
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
    private Rigidbody2D stuckLimb;
    private Vector3 mousePosition;
    private Vector3 forceDirection;

    private void Awake()
    {
        LevelManager.Instance.Zombie = this;

        limb = leftHand; // Currently default to left hand
    }

    public void Stick()
    {
        OnZombieStick?.Invoke(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            limb = leftHand;
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            limb = rightHand;
        }

        else if (Input.GetKeyDown(KeyCode.Z))
        {
            limb = leftFoot;
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            limb = rightFoot;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            limb = null;
            stuckLimb = leftHand;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            limb = null;
            stuckLimb = rightHand;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            limb = null;
            stuckLimb = leftFoot;
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            limb = null;
            stuckLimb = rightFoot;
        }
    }

    private void FixedUpdate()
    {
        if (limb != null)
        {
            limb.constraints = RigidbodyConstraints2D.None;
        }

        if (stuckLimb != null)
        {
            stuckLimb.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        mousePosition = LevelManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
        forceDirection = (mousePosition - limb.transform.position).normalized;
        limb.AddForce(forceDirection * speed, ForceMode2D.Impulse);
    }
}
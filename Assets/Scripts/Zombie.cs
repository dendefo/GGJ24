using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] LimbEnd leftHand;
    [SerializeField] LimbEnd rightHand;
    [SerializeField] LimbEnd leftFoot;
    [SerializeField] LimbEnd rightFoot;
    [SerializeField] float speed;

    public delegate void ZombieStick(Zombie zombie);

    public static event ZombieStick OnZombieStick;
    private LimbEnd limb;
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
        if (Input.GetKeyDown(KeyCode.A)) UnstickLimb(leftHand);

        else if (Input.GetKeyDown(KeyCode.D)) UnstickLimb(rightHand);

        else if (Input.GetKeyDown(KeyCode.Z)) UnstickLimb(leftFoot);

        else if (Input.GetKeyDown(KeyCode.C)) UnstickLimb(rightFoot);

        OnZombieStick(this);
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.C)) { limb.isActive = false; limb = null; }
    }

    private void FixedUpdate()
    {
        //if (limb != null)
        //{
        //    limb.constraints = RigidbodyConstraints2D.None;
        //}

        //if (stuckLimb != null)
        //{
        //    stuckLimb.constraints = RigidbodyConstraints2D.FreezePosition;
        //}
        if (limb == null) return;
        mousePosition = LevelManager.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
        forceDirection = (mousePosition - limb.transform.position).normalized;
        limb.rb.AddForce(forceDirection * speed, ForceMode2D.Impulse);
    }

    void UnstickLimb(LimbEnd limbb)
    {
        limb = limbb;
        limb.isActive = true;
        limb.OtherJoints.ForEach(joint => Destroy(joint));
    }
}
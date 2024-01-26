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
    public List<Rigidbody2D> bones;

    public delegate void ZombieStick(Zombie zombie);

    public static event ZombieStick OnZombieStick;
    private LimbEnd limb;
    private Vector3 mousePosition;
    private Vector3 forceDirection;
    private float minAngle;
    private float maxAngle;

    private void Awake()
    {
        LevelManager.Instance.Zombie = this;
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

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.Z) ||
            Input.GetKeyUp(KeyCode.C))
        {
            limb.isActive = false;
            limb = null;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var bone in bones)
            {
                bone.gameObject.tag = "Stickable";
                bone.bodyType = RigidbodyType2D.Static;
                bone.includeLayers = LayerMask.GetMask("Zombie");
            }
            OnZombieStick(this);
        }
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
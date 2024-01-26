using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LimbEnd : MonoBehaviour
{
    //public FixedJoint2D mainJoint;
    public Rigidbody2D rb;
    public List<HingeJoint2D> OtherJoints;
    [SerializeField] private bool _isActive = false;
    public HingeJoint2D secondJoint;
    public HingeJoint2D thirdJoint;
    public SpriteRenderer spriteRenderer;
    public Sprite white;
    [SerializeField] Sprite green;
    public Sprite red;
    [SerializeField] Sprite empty;

    public bool isActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;

            if (value)
            {
                JointAngleLimits2D limits = new JointAngleLimits2D();
                limits.max = maxAngle;
                limits.min = minAngle;
                secondJoint.limits = limits;
                JointAngleLimits2D limits2 = new JointAngleLimits2D();
                limits2.max = SMaxAngle;
                limits2.min = SMinAngle;
                thirdJoint.limits = limits2;
                spriteRenderer.sprite = green;
            }
            else
            {
                JointAngleLimits2D limits = new JointAngleLimits2D();
                limits.max = secondJoint.jointAngle;
                limits.min = secondJoint.jointAngle;
                secondJoint.limits = limits;
                JointAngleLimits2D limits2 = new JointAngleLimits2D();
                limits2.max = thirdJoint.jointAngle;
                limits2.min = thirdJoint.jointAngle;
                thirdJoint.limits = limits2;
                if (OtherJoints.Where(join => join != null).Count() == 0) spriteRenderer.sprite = white;
            }
        }
    }

    public float minAngle;
    public float maxAngle;
    public float SMinAngle;
    public float SMaxAngle;

    private void Awake()
    {
        secondJoint = transform.parent.GetComponent<HingeJoint2D>();
        thirdJoint = secondJoint.connectedBody.GetComponent<HingeJoint2D>();
        minAngle = secondJoint.limits.min;
        maxAngle = secondJoint.limits.max;
        SMinAngle = thirdJoint.limits.min;
        SMaxAngle = thirdJoint.limits.max;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (isActive) return;
        if (!collision.gameObject.CompareTag("Stickable") && !collision.gameObject.CompareTag("Macapig") &&
            !collision.gameObject.CompareTag("StickableZombie")) return;
        var joint = this.AddComponent<HingeJoint2D>();
        joint.connectedBody = collision.collider.attachedRigidbody;
        //joint.anchor = collision.GetContact(0).normal;
        OtherJoints.Add(joint);
        JointAngleLimits2D limits = new JointAngleLimits2D();
        limits.max = maxAngle;
        limits.min = minAngle;
        secondJoint.limits = limits;
        JointAngleLimits2D limits2 = new JointAngleLimits2D();
        limits2.max = SMaxAngle;
        limits2.min = SMinAngle;
        thirdJoint.limits = limits2;
        spriteRenderer.sprite = red;
    }

    public void Disable()
    {
        spriteRenderer.sprite = empty;
    }
}
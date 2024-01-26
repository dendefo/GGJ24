using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LimbEnd : MonoBehaviour
{
    public FixedJoint2D mainJoint;
    public Rigidbody2D rb;
    public List<FixedJoint2D> OtherJoints;
    [SerializeField] private bool _isActive = false;
    public HingeJoint2D secondJoint;
    public HingeJoint2D thirdJoint;
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
        if (isActive) return;
        if (!collision.gameObject.CompareTag("Stickable")) return;
        var joint = collision.collider.AddComponent<FixedJoint2D>();
        joint.connectedBody = collision.otherCollider.attachedRigidbody;
        joint.anchor = collision.GetContact(0).normal;
        OtherJoints.Add(joint);
        JointAngleLimits2D limits = new JointAngleLimits2D();
        limits.max = maxAngle;
        limits.min = minAngle;
        secondJoint.limits = limits;
        JointAngleLimits2D limits2 = new JointAngleLimits2D();
        limits2.max = SMaxAngle;
        limits2.min = SMinAngle;
        thirdJoint.limits = limits2;
    }

}

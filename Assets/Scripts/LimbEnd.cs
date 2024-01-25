using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LimbEnd : MonoBehaviour
{
    public FixedJoint2D mainJoint;
    public Rigidbody2D rb;
    public List<FixedJoint2D> OtherJoints;
    public bool isActive = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive) return;
        if (!collision.gameObject.CompareTag("Stickable")) return;
        var joint = collision.collider.AddComponent<FixedJoint2D>();
        joint.connectedBody = collision.otherCollider.attachedRigidbody;
        joint.anchor = collision.GetContact(0).normal;
        OtherJoints.Add(joint);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class Zombie : MonoBehaviour
{
    [SerializeField] LimbEnd leftHand;
    [SerializeField] LimbEnd rightHand;
    [SerializeField] LimbEnd leftFoot;
    [SerializeField] LimbEnd rightFoot;
    [SerializeField] float speed;
    public List<Rigidbody2D> bones;

    private Random rnd = new Random();

    public delegate void ZombieStick(Zombie zombie);

    public static event ZombieStick OnZombieStick;
    private LimbEnd limb;
    private Vector3 mousePosition;
    private Vector3 forceDirection;
    private float minAngle;
    private float maxAngle;
    public float maxY = 0;
    [SerializeField] private List<AudioClip> voiceLines = new List<AudioClip>();

    private void Awake()
    {
        LevelManager.Instance.Zombie = this;

    }

    private void OnEnable()
    {
        AudioSource.PlayClipAtPoint(voiceLines[rnd.Next(0, voiceLines.Count)], LevelManager.Instance.camera.transform.position);
    }
    public void Stick()
    {
        OnZombieStick?.Invoke(this);
    }

    private void Update()
    {
        float[] limbYPosArray =
        {
            rightHand.gameObject.transform.position.y, leftHand.gameObject.transform.position.y,
            rightFoot.gameObject.transform.position.y,
            leftFoot.gameObject.transform.position.y
        };
        maxY = limbYPosArray.Max();
        if (LevelManager.Instance.heightLine.transform.position.y < maxY)
            LevelManager.Instance.playableArea.transform.localScale = new Vector3(
                LevelManager.Instance.playableArea.transform.localScale.x,
                10 + maxY * 2,
                LevelManager.Instance.playableArea.transform.localScale.z);

        if (leftFoot.spriteRenderer.sprite == leftFoot.white && rightFoot.spriteRenderer.sprite == rightFoot.white &&
                               leftHand.spriteRenderer.sprite == leftHand.white && rightHand.spriteRenderer.sprite == rightHand.white) return;
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
            if (LevelManager.Instance.heightLine.transform.position.y > maxY) return;
            if (!(leftFoot.spriteRenderer.sprite == leftFoot.red || rightFoot.spriteRenderer.sprite == rightFoot.red ||
                               leftHand.spriteRenderer.sprite == leftHand.red || rightHand.spriteRenderer.sprite == rightHand.red)) return;
            foreach (var bone in bones)
            {
                bone.gameObject.tag = "Stickable";
                bone.bodyType = RigidbodyType2D.Static;
                bone.includeLayers = LayerMask.GetMask("Zombie");
            }

            if (LevelManager.Instance.heightLine.transform.position.y < maxY)
                LevelManager.Instance.heightLine.transform.position = new Vector3(
                    LevelManager.Instance.heightLine.transform.position.x, maxY,
                    LevelManager.Instance.heightLine.transform.position.z);

            leftFoot.Disable();
            leftHand.Disable();
            rightFoot.Disable();
            rightHand.Disable();
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
        if (limb != null) return;
        limb = limbb;
        limb.isActive = true;
        limb.OtherJoints.ForEach(joint => Destroy(joint));
    }
}
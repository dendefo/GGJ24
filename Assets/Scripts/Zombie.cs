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
    public List<ParticleSystem> particles;
    [SerializeField] Animator animator;
    bool radioIsPlayed = false;

    private void Awake()
    {
        LevelManager.Instance.Zombie = this;
        bones.ForEach(bone => bone.bodyType = RigidbodyType2D.Static);
    }

    private void OnEnable()
    {
        AudioManager.Instance.PlayZombieTrack(voiceLines[rnd.Next(0, voiceLines.Count)]);
    }
    public void Stick()
    {
        OnZombieStick?.Invoke(this);
    }

    private void Update()
    {
        if (animator.GetNextAnimatorStateInfo(0).length != 0 || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Z)
            || Input.GetKeyDown(KeyCode.C))
        {
            bones.ForEach(bone => bone.bodyType = RigidbodyType2D.Dynamic);
            animator.enabled = false;
            if (!radioIsPlayed)
            {
                AudioManager.Instance.PlayRadioTrack();
                radioIsPlayed = true;
            }
        }

        float[] limbYPosArray =
        {
            rightHand.gameObject.transform.position.y, leftHand.gameObject.transform.position.y,
            rightFoot.gameObject.transform.position.y,
            leftFoot.gameObject.transform.position.y
        };
        maxY = limbYPosArray.Max();
        if (LevelManager.Instance.heightLine.transform.position.y < maxY)
        {
            LevelManager.Instance.playableArea.transform.localScale = new Vector3(
                LevelManager.Instance.playableArea.transform.localScale.x,
                10 + maxY * 2,
                LevelManager.Instance.playableArea.transform.localScale.z);
            LevelManager.Instance.SpaceToolTIp.SetActive(true);
        }
        else LevelManager.Instance.SpaceToolTIp.SetActive(false);


        if (Input.GetKeyDown(KeyCode.A)) UnstickLimb(leftHand);

        else if (Input.GetKeyDown(KeyCode.D)) UnstickLimb(rightHand);

        else if (Input.GetKeyDown(KeyCode.Z)) UnstickLimb(leftFoot);

        else if (Input.GetKeyDown(KeyCode.C)) UnstickLimb(rightFoot);

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.Z) ||
            Input.GetKeyUp(KeyCode.C))
        {
            if (limb != null)
            {
                limb.isActive = false;
                limb = null;
                LevelManager.Instance.Choose.gameObject.SetActive(false);
            }
        }
        if (leftFoot.spriteRenderer.sprite != leftFoot.red && rightFoot.spriteRenderer.sprite != rightFoot.red &&
            leftHand.spriteRenderer.sprite != leftHand.red && rightHand.spriteRenderer.sprite != rightHand.red)
        {
            limb = null;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (LevelManager.Instance.heightLine.transform.position.y > maxY) return;
            if (!(leftFoot.spriteRenderer.sprite == leftFoot.red || rightFoot.spriteRenderer.sprite == rightFoot.red ||
                               leftHand.spriteRenderer.sprite == leftHand.red || rightHand.spriteRenderer.sprite == rightHand.red)) return;
            foreach (var bone in bones)
            {
                bone.gameObject.tag = "StickableZombie";
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
        LevelManager.Instance.Choose.gameObject.SetActive(true);
        LevelManager.Instance.Choose.transform.parent = limb.transform;
        LevelManager.Instance.Choose.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        LevelManager.Instance.Choose.transform.localScale = Vector3.one * 2;
        LevelManager.Instance.Choose.transform.localPosition = Vector3.zero;
    }
}
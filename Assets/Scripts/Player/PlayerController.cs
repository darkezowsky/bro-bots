using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cichy.Utility;
using System;
using System.Numerics;
using TMPro;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public enum State
{
    WeaponStage1,
    WeaponStage2,
    WeaponStage3,
    WeaponStage4,
    Dash,
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float rotateSpeed;
    public float activeDashSpeed = 150f;
    public float dashAmount = 10f;
    public Transform cameraTransform;

    [Header("Dash Cooldown")]
    public float dashCooldown = 2f;
    private float dashCooldownTimer = 0f;
    private bool canDash = true;

    [Header("Weapon States Stats")]
    [SerializeField] private GameObject handhead; // Umożliwia przypisanie w Inspektorze
    public float firstWeaponAOERadius = 4f;
    public float secondWeaponAOERadius = 4f;
    public float thirdWeaponAOERadius = 4f;
    public float fourthWeaponAOERadius = 4f;

    public int secondWeaponStateScrap = 70;
    public int thirdWeaponStateScrap = 140;
    public int fourthWeaponStateScrap = 210;

    [Header("AoE Cooldown")]
    public bool aoeReady;
    public float aoeCooldown = 2f;
    private float aoeCooldownCurrent = 3f;
    public Slider aoeSlider;

    [Header(("Push Power"))]
    public float firstWeaponPushPower;
    public float secondWeaponPushPower;
    public float thirdWeaponPushPower;
    public float fourthWeaponPushPower;

    public ScrapManager scrapManager;
    public Animator characterAnim; // Animator postaci
    public Animator handheadAnim;   // Animator broni
    private Rigidbody _rb;
    private Vector3 _moveDir;
    private Vector3 _dashDir;
    private Vector3 _lastMoveDir;
    private float _dashSpeed;
    private bool _isDashButtonDown;
    private State _state;
    private InputHandler _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<InputHandler>();
        characterAnim = GetComponent<Animator>();

        // Zainicjalizuj animator broni
        if (handhead != null)
        {
            handheadAnim = handhead.GetComponent<Animator>();
            if (handheadAnim == null)
            {
                Debug.LogError("Animator broni (handhead) nie został znaleziony!");
            }
        }
        else
        {
            Debug.LogError("Obiekt 'handhead' nie został przypisany w Inspektorze!");
        }
    }

    void Update()
    {
        switch (_state)
        {
            case State.WeaponStage1:
                PlayerMovement();
                AoeAttackCooldown();
                PlayerDash();
                break;
            case State.WeaponStage2:
                PlayerDash();
                AttackAoe();
                break;
            case State.WeaponStage3:
                PlayerDash();
                AttackAoe();
                break;
            case State.WeaponStage4:
                AttackAoe();
                break;

            case State.Dash:
                DashMovement();
                break;
        }
        /*
        if (scrapManager.scrapNumber <= 69)
        {
            _state = State.WeaponStage1;
        }
        if (scrapManager.scrapNumber >= secondWeaponStateScrap)
        {
            _state = State.WeaponStage2;
        }

        if (scrapManager.scrapNumber >= thirdWeaponStateScrap)
        {
            _state = State.WeaponStage3;
        }
        if (scrapManager.scrapNumber >= fourthWeaponStateScrap)
        {
            _state = State.WeaponStage4;
        }
        */
    }

    private void AoeAttackCooldown()
    {
        if (aoeCooldownCurrent >= aoeCooldown)
        {
            aoeReady = true;
        }
        else
        {
            aoeCooldownCurrent += Time.deltaTime;
            aoeCooldownCurrent = Mathf.Clamp(aoeCooldownCurrent, 0.0f, aoeCooldown);
            aoeReady = false;
        }

        aoeSlider.value = aoeCooldownCurrent / aoeCooldown;

        if (aoeSlider.value >= 1.0f)
        {
            aoeSlider.gameObject.SetActive(false);
        }
        else
        {
            aoeSlider.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && aoeReady)
        {
            AttackAoe();
            characterAnim.SetTrigger("Attack_WeaponStage1"); // Animator postaci
            handheadAnim.SetTrigger("Hand_Throw"); // Animator broni
            aoeCooldownCurrent = 0.0f;
        }
    }

    private void PlayerMovement()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();

        Vector3 targetVector = (forward * _input.InputVector.y + right * _input.InputVector.x).normalized;

        var movementVector = MoveTowardTarget(targetVector);
        RotateTowardMovementVector(movementVector);
        _moveDir = movementVector.normalized;

        if (movementVector != Vector3.zero)
        {
            characterAnim.SetBool("isRunning", true);
        }
        else
        {
            characterAnim.SetBool("isRunning", false);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementVector)
    {
        if (movementVector != Vector3.zero)
        {
            var rotation = Quaternion.LookRotation(movementVector);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
        }
    }

    #region Attacking

    public void AttackAoe()
    {
        StartCoroutine(AttackSequenceAOE());
    }

    private IEnumerator AttackSequenceAOE()
    {
        yield return new WaitForSeconds(0.2f);
        CheckForEnemiesAndDealAoeDamage();
        yield return new WaitForSeconds(0.5f);
    }

    private void CheckForEnemiesAndDealAoeDamage()
    {
        //case State.WeaponStage1:
        Collider[] colliders = Physics.OverlapSphere(transform.position, firstWeaponAOERadius);
        foreach (Collider c in colliders)
        {
            if (c.GetComponent<EnemyMovement>())
            {
                c.GetComponent<EnemyMovement>().Push();
            }
        }
        /*
                break;
            case State.WeaponStage2:
                Collider[] colliders2 = Physics.OverlapSphere(transform.position, secondWeaponAOERadius);
                foreach (Collider c in colliders2)
                {
                    if (c.GetComponent<EnemyMovement>())
                    {
                        c.GetComponent<EnemyMovement>().Push();
                    }
                }
                break;
            case State.WeaponStage3:
                Collider[] colliders3 = Physics.OverlapSphere(transform.position, thirdWeaponAOERadius);
                foreach (Collider c in colliders3)
                {
                    if (c.GetComponent<EnemyMovement>())
                    {
                        c.GetComponent<EnemyMovement>().Push();
                    }
                }
                break;
            case State.WeaponStage4:
                Collider[] colliders4 = Physics.OverlapSphere(transform.position, fourthWeaponAOERadius);
                foreach (Collider c in colliders4)
                {
                    if (c.GetComponent<EnemyMovement>())
                    {
                        c.GetComponent<EnemyMovement>().Push();
                    }
                }
                break;
         */
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, firstWeaponAOERadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, secondWeaponAOERadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, thirdWeaponAOERadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fourthWeaponAOERadius);
    }

    #endregion

    #region Dash Movement

    private void DashMovement()
    {
        float dashSpeedDropMultiplier = 5f;
        _dashSpeed -= _dashSpeed * dashSpeedDropMultiplier * Time.deltaTime;

        float dashSpeedMinimum = 50f;
        if (_dashSpeed < dashSpeedMinimum)
        {
            _state = State.WeaponStage1;
        }
    }

    private void PlayerDash()
    {
        if (canDash && Input.GetKeyDown(KeyCode.Space))
        {
            _dashDir = _moveDir;
            _dashSpeed = activeDashSpeed;
            _state = State.Dash;

            characterAnim.SetTrigger("Dash");

            canDash = false;
            dashCooldownTimer = dashCooldown;
        }

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0f)
            {
                canDash = true;
            }
        }
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.Dash:
                _rb.linearVelocity = _dashDir * _dashSpeed;
                break;
        }
    }

    #endregion
}


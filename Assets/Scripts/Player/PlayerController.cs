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
    RollingDash,
}
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    /* public float moveX = 0f;
     public float moveZ = 0f;*/
    public float moveSpeed = 7f;
    public float rotateSpeed;
    public float activeRollSpeed = 150f;
    public float dashAmount = 10f;
    //public Vector2 InputVector { get; private set; }


    [Header("Weapon States Stats")]
    public float firstWeaponAOERadius = 4f;
    public float secondWeaponAOERadius = 4f;
    public float thirdWeaponAOERadius = 4f;
    public float fourthWeaponAOERadius = 4f;
    
    public int secondWeaponStateScrap =  70;
    public int thirdWeaponStateScrap = 140;
    public int fourthWeaponStateScrap = 210;

    [Header("AoE Cooldown")]
    public bool aoeReady;
    public float aoeCooldown = 2;
    public float aoeCooldownCurrent = 0;
    public Slider aoeSlider;

    [Header(("Push Power"))] 
    public float firstWeaponPushPower;
    public float secondWeaponPushPower;
    public float thirdWeaponPushPower;
    public float fourthWeaponPushPower;


    public ScrapManager scrapManager;
    public Animator characterAnim; // Dodanie referencji do Animatora
    private Rigidbody _rb;
    private Vector3 _moveDir;
    private Vector3 _rollDir;
    private Vector3 _lastMoveDir;
    private float _rollSpeed;
    private bool _isDashButtonDown;
    private State _state;
    private InputHandler _input;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<InputHandler>();
        characterAnim = GetComponent<Animator>(); // Przypisanie animatora w Awake
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

            case State.RollingDash:
                RollingDash();

                break;
        }
        //if (scrapManager.scrapNumber <= 69)
        //{
        //    _state = State.WeaponStage1;
        //}
        //if (scrapManager.scrapNumber >= secondWeaponStateScrap)
        //{
        //    _state = State.WeaponStage2;
        //}

        //if (scrapManager.scrapNumber >= thirdWeaponStateScrap)
        //{
        //    _state = State.WeaponStage3;
        //}
        //if (scrapManager.scrapNumber >= fourthWeaponStateScrap)
        //{
        //    _state = State.WeaponStage4;
        //}
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

        if (Input.GetMouseButtonDown(1) && aoeReady)
        {
            AttackAoe();
            aoeCooldownCurrent = 0.0f;
        }
    }

    private void PlayerMovement()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);
    
        var movementVector = MoveTowardTarget(targetVector);
        RotateTowardMovementVector(movementVector);
        _moveDir = movementVector.normalized;

        // Sprawdzenie czy postać biednie potrzebne do ustawienia animacji biegu w animatorze

        if (movementVector != Vector3.zero)
        {
            characterAnim.SetBool("isRunning", true); // Animacja biegu
        }
        else
        {
            characterAnim.SetBool("isRunning", false); // Zatrzymanie animacji biegu
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
        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }


    #region Attacking

    public void AttackAoe()
    {
        StartCoroutine(AttackSequenceAOE());

        //animation
    }

    private IEnumerator AttackSequenceAOE()
    {
        yield return new WaitForSeconds(0.2f);
        CheckForEnemiesAndDealAoeDamage();
        yield return new WaitForSeconds(0.5f);
        //particle
        //attack
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
            //    break;
            //case State.WeaponStage2:
            //    Collider[] colliders2 = Physics.OverlapSphere(transform.position, secondWeaponAOERadius);
            //    foreach (Collider c in colliders2)
            //    {
            //        if (c.GetComponent<EnemyMovement>())
            //        {
            //            c.GetComponent<EnemyMovement>().Push();
            //        }
            //    }
            //    break;
            //case State.WeaponStage3:
            //    Collider[] colliders3 = Physics.OverlapSphere(transform.position, thirdWeaponAOERadius);
            //    foreach (Collider c in colliders3)
            //    {
            //        if (c.GetComponent<EnemyMovement>())
            //        {
            //            c.GetComponent<EnemyMovement>().Push();
            //        }
            //    }
            //    break;
            //case State.WeaponStage4:
            //    Collider[] colliders4 = Physics.OverlapSphere(transform.position, fourthWeaponAOERadius);
            //    foreach (Collider c in colliders4)
            //    {
            //        if (c.GetComponent<EnemyMovement>())
            //        {
            //            c.GetComponent<EnemyMovement>().Push();
            //        }
            //    }
            //    break;
        

  
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

    #region movement
    private void RollingDash()
    {
        float rollSpeedDropMultiplier = 5f;
        _rollSpeed -= _rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

        float rollSpeedMinimum = 50f;
        if (_rollSpeed < rollSpeedMinimum)
        {
            _state = State.WeaponStage1;
        }
    }

    private void PlayerDash()
    {


        if (Input.GetKeyDown(KeyCode.Home))
        {
            _isDashButtonDown = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rollDir = _moveDir;
            _rollSpeed = activeRollSpeed;
            _state = State.RollingDash;

            characterAnim.SetTrigger("Dash"); // Uruchomienie animacji dasha
        }
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.RollingDash:
                _rb.velocity = _rollDir * _rollSpeed;
                break;
        }
    }
    #endregion
}

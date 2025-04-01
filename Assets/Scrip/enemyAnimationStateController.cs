using UnityEngine;
using System.Collections;
using Unity.Mathematics;

public class enemyAnimationStateController : MonoBehaviour
{
    Animator animator;
    DistanceChecker distance;
    public Transform enemyTransform;
    public float beforeAttackDelay1 = 0f;
    public float beforeAttackDelay2 = 0f;
    private float delayTime;
    public bool isNearHit;
    public bool isDirect;
    public float distanceToMoveX;
    public float distanceToMoveZ;
    private Vector3 debugEnemy;
    public int attack;
    private bool prePare;
    private bool canAttack;
    public float playerDistance;
    private float debugTime;
    public bool debug;
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        distance = GetComponent<DistanceChecker>();
        attack = 0;
    }

    void Update()
    {
        
        if (delayTime > 0)
        {
            delayTime -= Time.deltaTime; if (delayTime < 0) delayTime = 0;
        }
        if (debugTime > 0)
        {
            debugTime -= Time.deltaTime; if (debugTime < 0) debugTime = 0;
        }
        if (debug)
        {
            AnimationAttackPhase();
            //debug = false;
            //Reset();
        }

        if (attack == 0)
        {
            if (playerDistance == 0f)
            {
                distance.GetDistance();
            }
            // ถ้า delayTime หมดแล้ว และอยู่ในสถานะการโจมตีใกล้
            if (isNearHit && isDirect && delayTime == 0)
            {
                delayTime = beforeAttackDelay1;
                attack = 1; //Na
            }
            if (!isNearHit && isDirect && delayTime == 0 && playerDistance <= 10f)
            {
                delayTime = beforeAttackDelay1;
                attack = 7; //Na
            }
            // ถ้า delayTime หมดแล้ว และอยู่ในสถานะการโจมตีไกล
            if (!isNearHit && isDirect && delayTime == 0 && playerDistance <= 18f && playerDistance > 15f)
            {
                delayTime = beforeAttackDelay2;
                attack = 2; //Fa1
            }
            if (!isNearHit && isDirect && delayTime == 0 && playerDistance <= 30f && playerDistance > 18f)
            {
                delayTime = beforeAttackDelay2;
                attack = 3; //Fa2
            }
            //เลือกเดิน
            if (!isNearHit && isDirect && delayTime == 0 && playerDistance <= 13f && playerDistance > 10f)
            {
                attack = 4; //Ml
            }
            if (!isNearHit && isDirect && delayTime == 0 && playerDistance <= 15f && playerDistance > 13f)
            {
                attack = 5; //Lm
            }
            if (!isNearHit && isDirect && delayTime == 0 && playerDistance > 30f)
            {
                attack = 6; //Rt
            }

        }

        if (attack > 0 && attack < 8 && delayTime == 0)
        {
            AttackChose();
        }

        //Debug.Log(delayTime);
    }

    public void AnimationAttackPhase() // Reset animation to idle when end
    {
        Debug.Log("Animation End");

        // ตรวจสอบว่าจบสถานะเดิน
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MoveLittle"))
        {
            animator.SetBool("isMoveLittle", false);
            animator.SetBool("isEnd", true);
            SetMoveDirection();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MoveLong"))
        {
            animator.SetBool("isMoveLong", false);
            animator.SetBool("isEnd", true);
            SetMoveDirection();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunTo"))
        {
            animator.SetBool("isRunTo", false);
            animator.SetBool("isEnd", true);
            SetMoveDirection();
        }

        // ตรวจสอบว่าจบสถานะการโจมตีใกล้
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackNear2"))
        {
            animator.SetBool("isNearHit", false);
            animator.SetBool("isEnd", true);
            playerDistance = 0f;
            attack = 0;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Bite"))
        {
            animator.SetBool("isBite", false);
            animator.SetBool("isEnd", true);
            SetMoveDirection();
        }
    
        // ตรวจสอบว่าจบสถานะการโจมตีไกล
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("FarAttack"))
        {  
            animator.SetBool("isFarHit", false);
            animator.SetBool("isEnd", true);
            SetMoveDirection();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackFar2"))
        {  
            animator.SetBool("isFarHit2", false);
            animator.SetBool("isEnd", true);
            SetMoveDirection();
        }
    }

    //////////
    void SetMoveDirection()
    {
        Vector3 moveDirection = (-enemyTransform.right * distanceToMoveX) + (enemyTransform.forward * distanceToMoveZ);
        debugEnemy = enemyTransform.position + moveDirection;
        enemyTransform.position += moveDirection;

        //Debug.Log("Moved to: " + enemyTransform.position);
        //Debug.Log("Vector3: " + moveDirection);
        //Debug.Log("debugEnemy: " + debugEnemy);
        StartCoroutine(MoveToDebugEnemy());
    }
/// <summary>
/// ////////////
/// </summary>
    IEnumerator MoveToDebugEnemy()
    {
        float duration = 0.2f; // ระยะเวลาการย้าย (ปรับได้)
        float elapsedTime = 0f;
        Vector3 startPosition = enemyTransform.position;

        while (elapsedTime < duration)
        {
            enemyTransform.position = Vector3.Lerp(startPosition, debugEnemy, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ให้แน่ใจว่าตำแหน่งสุดท้ายตรงกับ debugEnemy
        enemyTransform.position = debugEnemy;
        Debug.Log("Enemy moved to debugEnemy: " + enemyTransform.position);

        StartCoroutine(ReEnableRotation());
    }

        public void PreEnd()
    {
        GetComponent<RotateEnemy>().enabled = false;
        Debug.Log("RotateEnemy False");
    }

    IEnumerator ReEnableRotation()
    {
        yield return new WaitForSeconds(0.05f); // รอให้ตำแหน่งอัปเดตก่อน
        GetComponent<RotateEnemy>().enabled = true;
        playerDistance = 0f;
        attack = 0;
    }


    void AttackChose()
    {
        if (attack == 1 )
        {
            //Debug.Log("Will Attack Near");
            prePare = true;
            if (canAttack)
            {
                canAttack = false;
                attack = 8;
                animator.SetBool("isNearHit", true);
                debugTime = 25f;
                //Debug.Log("Attack Near");
            }
        }
        if (attack == 2)
        {
            //Debug.Log("Will Attack Far");
            prePare = true;
            if (canAttack)
            {
                canAttack = false;
                attack = 8;
                animator.SetBool("isFarHit", true);
                distanceToMoveX = 31.66f;
                distanceToMoveZ = 0f;
                debugTime = 25f;
                //Debug.Log("Attack Far");
            }
        }
        if (attack == 3)
        {
            //Debug.Log("Will Attack Far2");
            prePare = true;
            if (canAttack)
            {
                canAttack = false;
                attack = 8;
                animator.SetBool("isFarHit2", true);
                distanceToMoveX = 41.59263f;
                distanceToMoveZ = 13.17362f;
                debugTime = 25f;
                //Debug.Log("Attack Far2");
            }
        }
        if (attack == 4)
        {
            //Debug.Log("Will Attack Far2");
            prePare = true;
            if (canAttack)
            {
                canAttack = false;
                attack = 8;
                animator.SetBool("isMoveLittle", true);
                distanceToMoveX = 4.199841f;
                distanceToMoveZ = 7.283287f;
                debugTime = 25f;
                //Debug.Log("Attack Far2");
            }
        }
        if (attack == 5)
        {
            //Debug.Log("Will Attack Far2");
            prePare = true;
            if (canAttack)
            {
                canAttack = false;
                attack = 8;
                animator.SetBool("isMoveLong", true);
                distanceToMoveX = 21.31273f;
                distanceToMoveZ = -7.948272f;
                debugTime = 25f;
                //Debug.Log("Attack Far2");
            }
        }
        if (attack == 6)
        {
            //Debug.Log("Will Attack Far2");
            prePare = true;
            if (canAttack)
            {
                canAttack = false;
                attack = 8;
                animator.SetBool("isRunTo", true);
                distanceToMoveX = 15.09278f;
                distanceToMoveZ = 0.3228941f;
                debugTime = 25f;
                //Debug.Log("Attack Far2");
            }
        }
        if (attack == 7)
        {
            //Debug.Log("Will Attack Far2");
            prePare = true;
            if (canAttack)
            {
                canAttack = false;
                attack = 8;
                animator.SetBool("isBite", true);
                distanceToMoveX = -2.72087f;
                distanceToMoveZ = 0.6649511f;
                debugTime = 25f;
                //Debug.Log("Attack Far2");
            }
        }
        if (prePare)
        {
            //Debug.Log("isEnd False");
            animator.SetBool("isEnd", false);
            canAttack = true;
            //debug = true;
        }
    }
    void Reset()
    {
        playerDistance = 0f;
        attack = 0;
        animator.SetBool("isEnd", true);
        animator.SetBool("isMoveLittle", false);
        animator.SetBool("isMoveLong", false);
        animator.SetBool("isRunTo", false);
        animator.SetBool("isNearHit", false);
        animator.SetBool("isBite", false);
        animator.SetBool("isFarHit", false);
        animator.SetBool("isFarHit2", false);
    }
}
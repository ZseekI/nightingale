using UnityEngine;
using System.Collections;
using Unity.Mathematics;

public class enemyAnimationStateController : MonoBehaviour
{
    Animator animator;
    public Transform enemyTransform;
    public float beforeAttackDelay1 = 0f;
    public float beforeAttackDelay2 = 0f;
    private float delayTime;
    public bool isNearHit;
    public bool isDirect;
    public float distanceToMove = -31.66f;
    private Vector3 debugEnemy;
    public int attack;
    private bool prePare;
    private bool canAttack;
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
        attack = 0;
    }

    void Update()
    {
        
        if (delayTime > 0)
        {
            delayTime -= Time.deltaTime; if (delayTime < 0) delayTime = 0;
        }


        // ถ้า delayTime หมดแล้ว และอยู่ในสถานะการโจมตีใกล้
        if (isNearHit && isDirect && delayTime == 0 && attack == 0)
        {
            delayTime = beforeAttackDelay1;
            attack = 1;
        }

        // ถ้า delayTime หมดแล้ว และอยู่ในสถานะการโจมตีไกล
        if (!isNearHit && isDirect && delayTime == 0 && attack == 0)
        {
            delayTime = beforeAttackDelay2;
            attack = 2;
        }

        if (attack > 0 && attack < 3 && delayTime == 0)
        {
            AttackChose();
        }

        //Debug.Log(delayTime);
    }

    public void AnimationAttackPhase() // Reset animation to idle when end
    {
        Debug.Log("Animation End");

        // ตรวจสอบว่าอยู่ในสถานะการโจมตีใกล้
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("NearAttack"))
        {
            animator.SetBool("isNearHit", false);
            animator.SetBool("isEnd", true);
            attack = 0;
        }
    
        // ตรวจสอบว่าอยู่ในสถานะการโจมตีไกล
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("FarAttack"))
        {  
            animator.SetBool("isFarHit", false);
            animator.SetBool("isEnd", true);

            Vector3 moveDirection = -enemyTransform.right * distanceToMove;
            debugEnemy = enemyTransform.position + moveDirection;
            enemyTransform.position += moveDirection;

            Debug.Log("Moved to: " + enemyTransform.position);
            Debug.Log("Vector3: " + moveDirection);
            Debug.Log("debugEnemy: " + debugEnemy);

            StartCoroutine(MoveToDebugEnemy());
        }
    }

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
                attack = 3;
                animator.SetBool("isNearHit", true);
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
                attack = 3;
                animator.SetBool("isFarHit", true);
                //Debug.Log("Attack Far");
            }
        }
        if (prePare)
        {
            //Debug.Log("isEnd False");
            animator.SetBool("isEnd", false);
            canAttack = true;
        }
    }
}
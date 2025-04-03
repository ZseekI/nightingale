using UnityEngine;
using System.Collections;

public class EnemyAnimationStateController : MonoBehaviour
{
    private Vector3 debugEnemy;
    private Animator animator;
    private DistanceChecker distanceChecker;

    [Header("DeBug")]
    public float delayTime;
    public bool isNearHit, isDirect, debug;
    public float playerDistance;
    public float distanceToMoveX, distanceToMoveZ;
    public int attack;
    public bool prePare, canAttack;

    [Header ("Move Real Enemy To End Animation Position")]
    public Transform enemyTransform;

    [Header("Delay Time")]
    public float biteDelay = 0f;
    public float nearAttackDelay = 0f;
    public float farAttackDelay1 = 0f;
    public float farAttackDelay2 = 0f;
    public float afterAttackDelay = 0f;

    void Start()
    {
        // ดึงค่าคอมโพเนนต์ Animator และ DistanceChecker
        animator = GetComponent<Animator>();
        distanceChecker = GetComponent<DistanceChecker>();
        attack = 0; // กำหนดค่าเริ่มต้นของการโจมตี
        canAttack = true;
    }

    void Update()
    {
        // อัปเดตตัวจับเวลา
        UpdateTimers();

        // ถ้า debug ทำงาน ให้บังคับเรียก AnimationAttackPhase()
        //if (debug) AnimationAttackPhase();
        
        // ตรวจสอบว่าควรเลือกท่าโจมตีหรือไม่
        if (attack == 0) DetermineAttack();

        // ถ้าอยู่ในสถานะที่สามารถโจมตีได้ ให้โจมตี
        if (attack > 0 && attack < 8 && delayTime == 0) ExecuteAttack();
    }

    void UpdateTimers()
    {
        // ลดค่า delayTime และ debugTime เมื่อเวลาผ่านไป
        if (delayTime > 0) delayTime = Mathf.Max(0, delayTime - Time.deltaTime);
        //if (debugTime > 0) debugTime = Mathf.Max(0, debugTime - Time.deltaTime);
    }

    void DetermineAttack()
    {
        // ตรวจสอบระยะของผู้เล่น
        if (playerDistance == 0f) distanceChecker.GetDistance();
        if (delayTime > 0) return; // ถ้ายังมี delayTime อยู่ ไม่ต้องเลือกท่าโจมตีใหม่

        // เงื่อนไขการเลือกท่าโจมตี
        if (isDirect)
        {
            if (isNearHit) { SetAttack(1, nearAttackDelay); Debug.Log("At DetermineAttack Delay: " + delayTime); return; } // ตีใกล้
            if (playerDistance <= 10f) { SetAttack(7, biteDelay); return; } // กัด
            if (playerDistance <= 18f) { SetAttack(2, farAttackDelay1); return; } // ตีไกล 1
            if (playerDistance <= 30f) { SetAttack(3, farAttackDelay2); return; } // ตีไกล 2
            if (playerDistance > 30f) { SetAttack(6); return; }  // วิ่งไปหา
            if (playerDistance > 13f) { SetAttack(5); return; } // เดินไกล
            if (playerDistance > 10f) { SetAttack(4); return; } // เดินใกล้
        }
    }

    void SetAttack(int attackType, float delay = 0f)
    {
        // ตั้งค่าท่าที่จะใช้และกำหนดเวลา delay ก่อนโจมตี
        attack = attackType;
        delayTime = delay;
    }

    void ExecuteAttack()
    {
        prePare = true;
        if (!canAttack) return; // ถ้ายังโจมตีไม่ได้ ให้รอก่อน
        
        canAttack = false;
        
        // ใช้ switch-case แทน if-else เพื่อลดความซับซ้อน
        switch (attack)
        {
            case 1: animator.SetBool("isNearHit", true); break;
            case 2: SetMoveValues("isFarHit", 31.66f, 0f); break;
            case 3: SetMoveValues("isFarHit2", 41.59f, 13.17f); Debug.Log(attack); break;
            case 4: SetMoveValues("isMoveLittle", 4.19f, 7.28f); break;
            case 5: SetMoveValues("isMoveLong", 21.31f, -7.94f); break;
            case 6: SetMoveValues("isRunTo", 15.09f, 0.32f); break;
            case 7: SetMoveValues("isBite", -2.72f, 0.66f); break;
        }

        // ปิดสถานะ "สิ้นสุด" เพื่อให้แอนิเมชันเล่นต่อ
        animator.SetBool("isEnd", false);
        canAttack = true;
    }

    void SetMoveValues(string animationBool, float moveX, float moveZ)
    {
        // ตั้งค่าระยะเคลื่อนที่และเปิดใช้งานแอนิเมชัน
        animator.SetBool(animationBool, true);
        distanceToMoveX = moveX;
        distanceToMoveZ = moveZ;
    }

    public void AnimationAttackPhase()
    {
        Debug.Log("Animation End");

        // เช็คว่าแอนิเมชันไหนเล่นจบ และรีเซ็ตสถานะ
        ResetAnimationState("MoveLittle", "isMoveLittle");
        ResetAnimationState("MoveLong", "isMoveLong");
        ResetAnimationState("RunTo", "isRunTo");
        ResetAnimationState("AttackNear2", "isNearHit", true);
        ResetAnimationState("Bite", "isBite");
        ResetAnimationState("FarAttack", "isFarHit");
        ResetAnimationState("AttackFar2", "isFarHit2");
    }

void ResetAnimationState(string animationName, string boolParam, bool resetAttack = false)
{
    var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    
    if (!stateInfo.IsName(animationName)) return;
    
    // เช็คว่าแอนิเมชันยังไม่จบจริงๆ ก่อนรีเซ็ต
    if (stateInfo.normalizedTime >= 1.0f) return; 

    animator.SetBool(boolParam, false);
    animator.SetBool("isEnd", true);
    delayTime = afterAttackDelay;
    attack = 0;
    prePare = false;
    if (boolParam != "isNearHit")
    {
        SetMoveDirection();
    }
}

    void SetMoveDirection()
    {
        // คำนวณระยะเคลื่อนที่ของศัตรู
        Vector3 moveDirection = (-enemyTransform.right * distanceToMoveX) + (enemyTransform.forward * distanceToMoveZ);
        debugEnemy = enemyTransform.position + moveDirection;
        enemyTransform.position += moveDirection;

        StartCoroutine(MoveToDebugEnemy());
    }

    IEnumerator MoveToDebugEnemy()
    {
        float duration = 0.2f, elapsedTime = 0f;
        Vector3 startPosition = enemyTransform.position;

        while (elapsedTime < duration)
        {
            enemyTransform.position = Vector3.Lerp(startPosition, debugEnemy, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemyTransform.position = debugEnemy;
        Debug.Log("Enemy moved to debugEnemy: " + enemyTransform.position);

        StartCoroutine(ReEnableRotation());
    }

    public void PreEnd()
    {
        // ปิดการหมุนของศัตรูก่อนย้ายตำแหน่ง
        GetComponent<RotateEnemy>().enabled = false;
        Debug.Log("RotateEnemy False");
    }

    IEnumerator ReEnableRotation()
    {
        yield return new WaitForSeconds(0.05f);
        GetComponent<RotateEnemy>().enabled = true;
        playerDistance = 0f;
        attack = 0;
        prePare = false;
    }
}
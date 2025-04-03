using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    private CombatSystem combatSystem;
    public EnemyAnimationStateController _mngrEAttack;
    // สร้างตัวแปร dmg ที่จะตั้งค่าให้กับท่าต่างๆ
    private int dmg = 10;

    void Update()
    {
        if (combatSystem == null)
        {
            combatSystem = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<CombatSystem>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // เช็คว่าเจอ player หรือไม่
        if (other.CompareTag("Player"))
        {
            // รับข้อมูลจาก Animator และเช็คว่า animation ไหนกำลังเล่น
            Animator animator = _mngrEAttack.GetComponent<Animator>();
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 คือ layer หลัก

            // ตรวจสอบ animation ที่กำลังเล่นแล้วตั้งค่าความเสียหาย (dmg)
            if (stateInfo.IsName("Bite"))
            {
                dmg = 10; // กำหนดความเสียหายที่ต้องการสำหรับ Bite
                Debug.Log("Bite attack triggered, dmg: " + dmg);
            }
            else if (stateInfo.IsName("FarAttack"))
            {
                dmg = 15; // กำหนดความเสียหายที่ต้องการสำหรับ FarAttack
                Debug.Log("FarAttack triggered, dmg: " + dmg);
            }
            else if (stateInfo.IsName("AttackFar2"))
            {
                dmg = 20; // กำหนดความเสียหายที่ต้องการสำหรับ AttackFar2
                Debug.Log("AttackFar2 triggered, dmg: " + dmg);
            }
            else if (stateInfo.IsName("AttackNear2"))
            {
                dmg = 5; // กำหนดความเสียหายที่ต้องการสำหรับ AttackNear2
                Debug.Log("AttackNear2 triggered, dmg: " + dmg);
            }

            // ส่งค่าความเสียหายให้กับ combatSystem ของ Player
            if (combatSystem != null)
            {
                combatSystem.TakeDamage(dmg); // เรียกฟังก์ชันที่รับความเสียหายจากท่าต่างๆ
            }
        }
    }

}

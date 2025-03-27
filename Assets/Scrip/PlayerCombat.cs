using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour 
{
    // กำหนดค่า Damage, Energy Gain, และค่า Stun สำหรับ 5 ท่าโจมตี
    public int[] attackDamages = {10, 12, 15, 18, 25}; // ท่าที่ 5 มีดาเมจสูงขึ้น
    public int[] energyGains   = {1, 1, 1, 1, 3};       // ท่าที่ 5 เพิ่มพลังงานมากขึ้น
    public int[] stunValues    = {0, 0, 0, 0, 1};       // ท่าที่ 5 มีค่าสตั้น (สามารถปรับได้)
    
    public float attackResetTime = 1.0f; // เวลาที่ใช้ Reset combo

    private int attackIndex = 0;
    private float attackTimer = 0f;

    // สมมติว่ามีการอ้างอิงศัตรูอยู่ในเกม
    public EnemyController enemy;

    void Update() {
        // ตัวอย่างการรับอินพุตโจมตี (เช่น กดปุ่ม J)
        if(Input.GetKeyDown(KeyCode.J)) {
            PerformAttack();
            attackTimer = attackResetTime;
        }
        
        // รีเซ็ต combo หากไม่มีการโจมตีต่อเนื่อง
        if(attackTimer > 0) 
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer <= 0) {
                attackIndex = 0;
            }
        }
    }
    
    void PerformAttack() {
        int dmg = attackDamages[attackIndex];
        int energyGain = energyGains[attackIndex];
        int stun = stunValues[attackIndex];

        // ส่งคำสั่งโจมตีไปยังศัตรู (สามารถเพิ่มฟังก์ชัน knockback ได้ใน EnemyController)
        enemy.TakeDamage(dmg, stun);

        // เพิ่มพลังงานร่วม (ผ่าน Energy Manager)
        EnergyManager.Instance.AddEnergy(energyGain);

        // เพิ่ม index ของ combo และวนลูปกลับไปที่ 0 เมื่อครบ 5 ท่า
        attackIndex = (attackIndex + 1) % attackDamages.Length;
    }
}


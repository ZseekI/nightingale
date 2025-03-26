using UnityEngine;

public class EnemyController : MonoBehaviour {
    public int maxHP = 50;
    private int currentHP;
    public int stunThreshold = 3;
    private int currentStun = 0;

    void Start() {
        currentHP = maxHP;
    }
    
    public void TakeDamage(int damage, int stun) {
        currentHP -= damage;
        currentStun += stun;
        // อัปเดต UI ของศัตรู

        if(currentHP <= 0) {
            Die();
        }
        // ถ้าค่าสตั้นเกิน threshold ให้หยุดเคลื่อนไหว
        if(currentStun >= stunThreshold) {
            StopMovement();
        }
    }
    
    void StopMovement() {
        // ปิดระบบการเคลื่อนไหวของศัตรู
        Debug.Log("Enemy stunned!");
    }
    
    void Die() {
        // ทำลายศัตรูและเรียกใช้งานเอฟเฟคที่ต้องการ
        Destroy(gameObject);
    }
}

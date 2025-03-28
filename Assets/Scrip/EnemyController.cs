using UnityEngine;

public class EnemyController : MonoBehaviour {
    public int maxHP = 50;
    private int currentHP;
    public int stunThreshold = 3;
    private int currentStun = 0;
    [Header("EnemyUI")]
    public Transform hpBar;    
    public Transform stunBar;

    void Start() {
        currentHP = maxHP;
        UpdateUI();
    }
    
    public void TakeDamage(int damage, int stun) {
        currentHP -= damage;
        currentStun += stun;
        // อัปเดต UI ของศัตรู
        UpdateUI();

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

    void UpdateUI() 
    {
        if (hpBar != null) {
            float hpScaleX = Mathf.Clamp((float)currentHP / maxHP, 0, 1);
            hpBar.localScale = new Vector3(hpScaleX, hpBar.localScale.y, hpBar.localScale.z);
        }

        if (stunBar != null) {
            float stunScaleX = Mathf.Clamp((float)currentStun / stunThreshold, 0, 1);
            stunBar.localScale = new Vector3(stunScaleX, stunBar.localScale.y, stunBar.localScale.z);
        }
    }
}

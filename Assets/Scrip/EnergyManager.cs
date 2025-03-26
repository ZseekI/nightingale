using UnityEngine;

public class EnergyManager : MonoBehaviour {
    public static EnergyManager Instance;
    
    public int maxEnergy = 100;
    public int currentEnergy = 0;
    
    void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    public void AddEnergy(int amount) {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        // อัปเดต UI ด้วย (ไม่แสดงตัวอย่างที่นี่)
    }
    
    public bool ConsumeEnergy(int amount) {
        if(currentEnergy >= amount) {
            currentEnergy -= amount;
            // อัปเดต UI ด้วย
            return true;
        }
        return false;
    }
}


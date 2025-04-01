using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance { get; private set; }

    [Header("Player Stats")]
    public int maxHP = 100;
    public int currentHP;
    public int damage = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ไม่ให้ถูกทำลายเมื่อล้างฉาก
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHP = maxHP; // เริ่มเกมให้เลือดเต็ม
    }

    public void TakeDamage(int damage)
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                PlayerDeath();
            }
        }
    }

    private void PlayerDeath()
    {
        Debug.Log("Player has died!");
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }
}
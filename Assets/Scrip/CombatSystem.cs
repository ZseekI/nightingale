using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("References")]
    private animationAttackStateController attackController;
    private EnergyManager energyManager;
    
    [Header("Attack Properties")]
    [SerializeField] private AttackData[] attackCombo;
    
    [Header("Combat Settings")]
    [SerializeField] private int playerMaxHP = 100;
    private int currentPlayerHP;
    public delegate void AttackEnergyUpdate(int energyRequired);
    public static event AttackEnergyUpdate OnAttackEnergyUpdate;
    
    void Start()
    {
        attackController = GameObject.FindGameObjectWithTag("CombatAnimationManager").GetComponent<animationAttackStateController>();
        energyManager = EnergyManager.Instance;
        PlayerStatsManager.Instance.currentHP = PlayerStatsManager.Instance.maxHP;
        

        if (attackCombo == null || attackCombo.Length == 0)
        {
            InitializeDefaultAttackCombo();
        }
    }
    
    
    public void ExecuteAttack()
    {
        int attackPhase = attackController.intAttack;
        
        if (attackPhase <= 0 || attackPhase > attackCombo.Length)
        {
            Debug.Log("GG");
            return;
        }
        
        AttackData currentAttack = attackCombo[attackPhase - 1];
        AttackData nextAttack = attackCombo[attackPhase];
        OnAttackEnergyUpdate?.Invoke(nextAttack.energyConsumption);
        
        if (currentAttack.energyConsumption > 0 && !energyManager.ConsumeEnergy(currentAttack.energyConsumption))
        {
            Debug.Log("Not enough energy for this attack!");
            return;
        }
        
        if (currentAttack.energyGain > 0)
        {
            energyManager.AddEnergy(currentAttack.energyGain);
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * currentAttack.attackRange * 0.5f, currentAttack.attackRange, LayerMask.GetMask("Enemy"));
        
        foreach (var hitCollider in hitColliders)
        {
            EnemyController enemy = hitCollider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentAttack.damage, currentAttack.stunValue);
                
                if (currentAttack.knockbackForce > 0)
                {
                    Rigidbody enemyRb = hitCollider.GetComponentInParent<Rigidbody>(); 
                    if (enemyRb != null)
                    {
                        Vector3 knockbackDirection = (hitCollider.transform.position - transform.position).normalized;
                        enemyRb.AddForce(knockbackDirection * currentAttack.knockbackForce, ForceMode.Impulse);
                    } 
                }
            }
        }       
    }
    private void InitializeDefaultAttackCombo()
    {
        attackCombo = new AttackData[6];
        
        attackCombo[0] = new AttackData { damage = 5, stunValue = 1, energyGain = 5, energyConsumption = 0, attackRange = 1.5f, knockbackForce = 0 };
        attackCombo[1] = new AttackData { damage = 7, stunValue = 1, energyGain = 7, energyConsumption = 0, attackRange = 1.5f, knockbackForce = 0 };
        attackCombo[2] = new AttackData { damage = 10, stunValue = 2, energyGain = 10, energyConsumption = 0, attackRange = 1.5f, knockbackForce = 0 };
        attackCombo[3] = new AttackData { damage = 12, stunValue = 2, energyGain = 10, energyConsumption = 0, attackRange = 1.5f, knockbackForce = 0 };
        attackCombo[4] = new AttackData { damage = 20, stunValue = 3, energyGain = 15, energyConsumption = 0, attackRange = 2.0f, knockbackForce = 5f };
        attackCombo[5] = new AttackData { damage = 0, stunValue = 0, energyGain = 0, energyConsumption = 0, attackRange = 0f, knockbackForce = 0 };
    }
    
    public void TakeDamage(int damage)
    {
        PlayerStatsManager.Instance.TakeDamage(damage);
    }
    
    private void PlayerDeath()
    {
        Debug.Log("Player has died!");
    }
    
    public int GetCurrentHP()
    {
        return currentPlayerHP;
    }

    public void SetHP(int hp)
{
    currentPlayerHP = Mathf.Clamp(hp, 0, playerMaxHP);
}
    
    private void OnDrawGizmosSelected()
    {
        if (attackCombo != null && attackCombo.Length > 0)
        {
            for (int i = 0; i < attackCombo.Length; i++)
            {
                if (attackCombo[i] != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(transform.position + transform.forward * attackCombo[i].attackRange * 0.5f, attackCombo[i].attackRange);
                }
            }
        }
    }
}

[System.Serializable]
public class AttackData
{
    public int damage = 10;
    public int stunValue = 1;
    public int energyGain = 5;
    public int energyConsumption = 0;
    public float attackRange = 1.5f;
    public float knockbackForce = 0f;
}

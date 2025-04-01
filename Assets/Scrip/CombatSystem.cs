using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("References")]
    private animationAttackStateController attackController;
    private EnergyManager energyManager;
    private managerUI managerUI;
    private PlayerHPUI playerHPUI;
    
    [Header("Attack Properties")]
    [SerializeField] private AttackData[] attackCombo;

    [Header("Combat Settings")]
    [SerializeField] private int playerMaxHP = 100;
    private int currentPlayerHP;
    public int damage = 10;

    public delegate void AttackEnergyUpdate(int energyRequired);
    public static event AttackEnergyUpdate OnAttackEnergyUpdate;

    void Start()
    {
        attackController = GameObject.FindGameObjectWithTag("CombatAnimationManager").GetComponent<animationAttackStateController>();
        energyManager = EnergyManager.Instance;
        playerHPUI = GetComponent<PlayerHPUI>();
        playerHPUI.playerMaxHP = playerMaxHP;

        if (managerUI == null)
        {
            managerUI = GameObject.FindGameObjectWithTag("ManagerUI").GetComponent<managerUI>();
        }

        if (managerUI.isSetHp1 == false || managerUI.isSetHp2 == false)
        {
            switch (gameObject.name)
            {
                case "Y Bot (Alador)(Clone)":
                    managerUI.characterHP1 = playerMaxHP;
                    Debug.Log(managerUI.characterHP1);
                    managerUI.isSetHp1 = true;
                    break;
                case "X Bot (Luna)(Clone)":
                    managerUI.characterHP2 = playerMaxHP;
                    Debug.Log(managerUI.characterHP2);
                    managerUI.isSetHp2 = true;
                    break;
            }
        }

        // ✅ โหลด HP ของตัวละครจาก managerUI
        switch (gameObject.name)
        {
            case "Y Bot (Alador)(Clone)":
                currentPlayerHP = managerUI.characterHP1;
                break;
            case "X Bot (Luna)(Clone)":
                currentPlayerHP = managerUI.characterHP2;
                break;
        }

        playerHPUI.playerCurrentHP = currentPlayerHP;

        if (attackCombo == null || attackCombo.Length == 0)
        {
            InitializeDefaultAttackCombo();
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            TakeDamage(damage);
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
                    // ดึงค่า mass ของศัตรู
                    float enemyMass = enemyRb.mass;
                    if (enemyRb != null)
                    {
                        Vector3 knockbackDirection = (hitCollider.transform.position - transform.position).normalized;
                        // คำนวณแรงกระแทกตามกฎ F = ma
                        float knockbackAcceleration = currentAttack.damage * 2f; // ค่าประมาณที่ปรับแต่งได้
                        Vector3 knockbackForce = knockbackDirection * enemyMass * knockbackAcceleration;

                        // ใช้ AddForce โดยใช้ ForceMode.Impulse เพื่อให้ส่งผลทันที
                        enemyRb.AddForce(knockbackForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentPlayerHP -= damage;

        // ✅ อัปเดตค่า HP ใน managerUI
        if (gameObject.name == "Y Bot (Alador)(Clone)")
        {
            managerUI.characterHP1 = currentPlayerHP;
        }
        else if (gameObject.name == "X Bot (Luna)(Clone)")
        {
            managerUI.characterHP2 = currentPlayerHP;
        }

        playerHPUI.playerCurrentHP = currentPlayerHP;

        if (currentPlayerHP <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        Debug.Log(gameObject.name + " has died!");
    }

    public int GetCurrentHP()
    {
        return currentPlayerHP;
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

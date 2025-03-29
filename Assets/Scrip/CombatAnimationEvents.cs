using UnityEngine;

public class CombatAnimationEvents : MonoBehaviour
{
    private CombatSystem combatSystem;

    void Start()
    {
         
    }

    void Update()
    {
        if (combatSystem == null)
        {
            // Find the combat system component
            combatSystem = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<CombatSystem>();
        }
    }

    // Call this from animation event at the moment the attack should deal damage
    public void OnAttackHit()
    {
        if (combatSystem != null)
        {
            combatSystem.ExecuteAttack();
        }
        else
        {
            Debug.Log("Null");
        }
    }
}

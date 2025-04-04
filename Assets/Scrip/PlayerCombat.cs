using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour 
{
    private animationAttackStateController attackStateController;
    void Start()
    {
        GameObject combatManager = GameObject.FindGameObjectWithTag("CombatAnimationManager");
        if (combatManager != null)
        {
            attackStateController = combatManager.GetComponent<animationAttackStateController>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            attackStateController.BtnAttack();
        }
    }
}
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
        else
        {
            Debug.LogError("CombatAnimationManager ไม่ถูกพบใน Scene!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Clicked!");
            attackStateController.BtnAttack();
        }
    }
}


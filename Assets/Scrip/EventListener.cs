using UnityEngine;

public class EventListener : MonoBehaviour
{
    private animationAttackStateController _mngrAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mngrAttack = GameObject.FindGameObjectWithTag("CombatAnimationManager").GetComponent<animationAttackStateController>();
    }

    public void CheckAnimEvent()
    {
        //Debug.Log("Animation End Check");
        _mngrAttack.CheckedAttackPhase();
    }
}

using UnityEngine;

public class enemyEventListener : MonoBehaviour
{
    private enemyAnimationStateController _emngrAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _emngrAttack = GetComponent<enemyAnimationStateController>();
    }

    public void CheckAttackEvent()
    {
        _emngrAttack.AttackEvent();
    }

    // Update is called once per frame
    public void CheckAnimEvent()
    {
        //Debug.Log("Animation End Check");
        _emngrAttack.AnimationAttackPhase();
    }
}

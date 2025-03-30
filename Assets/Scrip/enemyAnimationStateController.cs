using UnityEngine;

public class enemyAnimationStateController : MonoBehaviour
{
    Animator animator;
    public bool isNearHit;
    public bool isDirect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Is Near Hit" + isNearHit);
        Debug.Log("Is Direct" + isDirect);
    }
    public void AttackEvent()
    {

    }

    public void AnimationAttackPhase()
    {

    }
}


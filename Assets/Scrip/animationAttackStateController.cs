using UnityEngine;

public class animationAttackStateController : MonoBehaviour
{
    private Animator _animatorPlayer;
    public int CountAttackClick;
    public float attackResetTime = 1.0f;
    private float resetTime;
    void Start()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        _animatorPlayer = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Animator>();
        CountAttackClick = 0;
        
    }
    
    public void BtnAttack()
    {
        CountAttackClick++;

        _animatorPlayer.SetInteger("intAttackPhase", 1);

        if (CountAttackClick >= 16)
        {
            ResetAttackPhase();
        }
        
    }

        private void Update()
    {
        if (_animatorPlayer == null)
        {
            FindPlayer(); // I need it to find player mesh if at start dont
        }
    }

    public void CheckedAttackPhase()
    {
        Debug.Log("Check Attack Phase");
        if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Boxing"))
        {
            Debug.Log("Current State 1");
            if(CountAttackClick > 1)
            {
                _animatorPlayer.SetInteger("intAttackPhase", 2);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Elbow Punch"))
        {
            Debug.Log("Current State 2");
            if(CountAttackClick > 2)
            {
                _animatorPlayer.SetInteger("intAttackPhase", 3);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Hook Punch"))
        {
            Debug.Log("Current State 3");
            if(CountAttackClick > 3)
            {
                _animatorPlayer.SetInteger("intAttackPhase", 4);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Headbutt"))
        {
            Debug.Log("Current State 4");
            if(CountAttackClick > 4)
            {
                _animatorPlayer.SetInteger("intAttackPhase", 5);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Fist Fight A"))
        {
            Debug.Log("Current State 5");
            if (CountAttackClick >= 5)
            {
                ResetAttackPhase();
            }
        }

    }

    private void ResetAttackPhase()
    {
        CountAttackClick = 0;
        _animatorPlayer.SetInteger("intAttackPhase", 0);
    }
}

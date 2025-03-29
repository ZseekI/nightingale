using UnityEngine;

public class animationAttackStateController : MonoBehaviour
{
    private Animator _animatorPlayer;
    public int CountAttackClick;
    public float attackResetTime = 1.0f;
    public float idleResetTime = 1.0f;
    private float resetAttackTime;
    private float resetIdleTime;
    int isWalkingHash;
    int isRunHash;
    public int T = 5;
    public int intAttack;
    void Start()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        _animatorPlayer = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Animator>();
        CountAttackClick = 0;
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunHash = Animator.StringToHash("isRun");
        
    }
    
    public void BtnAttack()
    {
        CountAttackClick++;
        intAttack = 1;
        _animatorPlayer.SetInteger("intAttackPhase", 1);
        resetAttackTime = attackResetTime;
        
    }

        private void Update()
    {
        if (_animatorPlayer != null)
        {
            bool isWalking = _animatorPlayer.GetBool(isWalkingHash);
            bool isRun = _animatorPlayer.GetBool(isRunHash);
            resetIdleTime = Mathf.Max(resetIdleTime - Time.deltaTime, 0);

        if (resetIdleTime > 0 && !isWalking && !isRun) 
            {
                _animatorPlayer.SetBool("isIdle", true);
            }
        
        else
            { 
                    resetIdleTime = 0;
                    _animatorPlayer.SetBool("isIdle", false);
            }
        }

        if (_animatorPlayer == null)
        {
            FindPlayer(); // I need it to find player mesh if at start dont
        }

        if(resetAttackTime > 0)
        {
            resetAttackTime = Mathf.Max(resetAttackTime - Time.deltaTime, 0);
            if (resetAttackTime == 0 && CountAttackClick > T)
            {
                Debug.Log("resetAttackTime"+resetAttackTime);
                CheckedAttackPhase();
            }
        }
    }

    public void CheckedAttackPhase()
    {
        //Debug.Log("Check Attack Phase");
        if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Boxing"))
        {
            //Debug.Log("Current State 1");
            if(CountAttackClick > 1 && resetAttackTime > 0)
            {
                intAttack = 2;
                _animatorPlayer.SetInteger("intAttackPhase", 2);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Elbow Punch"))
        {
            //Debug.Log("Current State 2");
            if(CountAttackClick > 2 && resetAttackTime > 0)
            {
                intAttack = 3;
                _animatorPlayer.SetInteger("intAttackPhase", 3);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Hook Punch"))
        {
            //Debug.Log("Current State 3");
            if(CountAttackClick > 3 && resetAttackTime > 0)
            {
               intAttack = 4;
                _animatorPlayer.SetInteger("intAttackPhase", 4);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Headbutt"))
        {
            //Debug.Log("Current State 4");
            if(CountAttackClick > 4 && resetAttackTime > 0)
            {
                intAttack = 5;
                _animatorPlayer.SetInteger("intAttackPhase", 5);
            }
            else
            {
                ResetAttackPhase();
            }
        }
        else if (_animatorPlayer.GetCurrentAnimatorStateInfo(0).IsName("Fist Fight A"))
        {
            //Debug.Log("Current State 5");
            if (CountAttackClick >= 5 )
            {
                ResetAttackPhase();
            }
        }
    }

    private void ResetAttackPhase()
    {

        CountAttackClick = 0;
       intAttack = 0;
        resetIdleTime = idleResetTime;
        _animatorPlayer.SetInteger("intAttackPhase", 0);
        
    }
}

using UnityEngine;

public class animationAttackStateController : MonoBehaviour
{
    private Animator _animatorPlayer;
    private int CountAttackClick;
    void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (_animatorPlayer == null)
        {
            FindPlayer();
            Debug.Log(_animatorPlayer);
        }
    }

    void FindPlayer()
    {
        _animatorPlayer = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Animator>();
        CountAttackClick = 0;
        
    }
    
    public void BtnAttack()
    {
        CountAttackClick++;
        //Debug.Log("Attack Count"+ CountAttackClick);
        
        if (CountAttackClick == 1)
        {
            _animatorPlayer.SetInteger("intAttackPhase", 1);
        }
    }

    public void CheckedAttackPhase()
    {
        Debug.Log("Check Attack Phase");
    }
}

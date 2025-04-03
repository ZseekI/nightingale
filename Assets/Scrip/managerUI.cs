using UnityEngine;



public class managerUI : MonoBehaviour //ใน GameObject ชื่อ ManagerUI ใช้ Tag ManagerUI
{
    int currentCharacterIndex;
    public bool isAttack;
    public float resetTime = 3.0f;
    private animationAttackStateController _mngrAttack;
    float characterTimer = 0;
    [Header("ChangedCharacterDelayBar")]
    public Transform ccdUIBar;
    public int characterHP1;
    public int characterHP2;

    public bool isSetHp1;
    public bool isSetHp2;
    private managerCharacterChanged mngrCharacterChange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mngrCharacterChange = GameObject.FindGameObjectWithTag("Player").GetComponent<managerCharacterChanged>();
        _mngrAttack = GameObject.FindGameObjectWithTag("CombatAnimationManager").GetComponent<animationAttackStateController>();
    }

    void Update()
    {
        if (characterTimer > 0)
        {
            characterTimer = Mathf.Max(characterTimer - Time.deltaTime, 0);
        }

        if (_mngrAttack != null)
        {
            isAttack = _mngrAttack.isAttack;
        }

        if (Input.GetKeyDown(KeyCode.Space) && characterTimer == 0 && !isAttack)
        {
            currentCharacterIndex = 1 - currentCharacterIndex; // สลับค่า 0 <-> 1
            characterTimer = resetTime;
            mngrCharacterChange.ChangeCharacter(currentCharacterIndex);
            Debug.Log(characterTimer);
        }
        else if(Input.GetKeyDown(KeyCode.Space) && characterTimer == 0 && isAttack)
        {
            currentCharacterIndex = 1 - currentCharacterIndex; // สลับค่า 0 <-> 1
            characterTimer = resetTime;
            _mngrAttack.ResetAttackPhase();
            mngrCharacterChange.ChangeCharacter(currentCharacterIndex);
        }

        if (characterTimer > 0)
        {
            //Debug.Log(characterTimer);
        }

        

        UpdateUIBar();
    }

        void UpdateUIBar()
    {
        if (ccdUIBar != null) 
        {
            float scaleX = 1 - (characterTimer / resetTime); 
            ccdUIBar.localScale = new Vector3(scaleX, ccdUIBar.localScale.y, ccdUIBar.localScale.z); 
        }
    }

    

}
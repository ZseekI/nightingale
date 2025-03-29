using UnityEngine;



public class managerUI : MonoBehaviour
{
    int currentCharacterIndex;
    public float resetTime = 3.0f;
    float characterTimer = 0;
    [Header("ChangedCharacterDelayBar")]
    public Transform ccdUIBar;
    private managerCharacterChanged mngrCharacterChange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mngrCharacterChange = GameObject.FindGameObjectWithTag("Player").GetComponent<managerCharacterChanged>();

    }

    void Update()
    {
        if (characterTimer > 0)
        {
            characterTimer = Mathf.Max(characterTimer - Time.deltaTime, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && characterTimer == 0 )
        {
            currentCharacterIndex = 1 - currentCharacterIndex; // สลับค่า 0 <-> 1
            characterTimer = resetTime;
            mngrCharacterChange.ChangeCharacter(currentCharacterIndex);
            Debug.Log(characterTimer);
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

using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    public Transform hpBar;
    public int playerMaxHP;
    public int playerCurrentHP;
    //private CombatSystem combatSystem;

    void Start()
    {
        hpBar = GameObject.FindGameObjectWithTag("EditorOnly").transform;
    }

    void Update()
    {


        if (hpBar != null)
        {
            // Update HP bar based on current HP percentage
            float hpPercentage = (float)playerCurrentHP / playerMaxHP; // Assuming max HP is 100
            hpBar.localScale = new Vector3(hpPercentage, hpBar.localScale.y, hpBar.localScale.z);
        }
    }
}

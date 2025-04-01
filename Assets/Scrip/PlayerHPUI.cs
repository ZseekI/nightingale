using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    public Transform hpBar;
    private CombatSystem combatSystem;

    void Start()
    {
        combatSystem = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<CombatSystem>();
        hpBar = GameObject.FindGameObjectWithTag("EditorOnly").transform;
    }

    void Update()
    {
        if (combatSystem == null)
        {
            combatSystem = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<CombatSystem>();
        }

        if (combatSystem != null && hpBar != null)
        {
            // Update HP bar based on current HP percentage
            float hpPercentage = (float)combatSystem.GetCurrentHP() / 100f; // Assuming max HP is 100
            hpBar.localScale = new Vector3(hpPercentage, hpBar.localScale.y, hpBar.localScale.z);
        }
    }
}

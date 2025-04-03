using UnityEngine;

public class PlayerHPUI : MonoBehaviour //ใน Prefab ชื่อ Y Bot (Alador) ใช้ Tag PlayerMesh และ ใน Prefab ชื่อ X Bot (Luna) ใช้ Tag PlayerMesh
{
    public Transform hpBar;
    public int playerMaxHP;
    public int playerCurrentHP;

    void Start()
    {
        if (hpBar == null)
        {
            hpBar = GameObject.FindGameObjectWithTag("PlayerHP").transform;
        }
    }

void Update()
{
    CombatSystem combat = GetComponent<CombatSystem>();
    if (combat != null)
    {
        playerCurrentHP = combat.GetCurrentHP(); // ดึงค่าปัจจุบันจาก CombatSystem
    }

    if (hpBar != null)
    {
        float hpPercentage = (float)playerCurrentHP / playerMaxHP;
        hpBar.localScale = new Vector3(hpPercentage, hpBar.localScale.y, hpBar.localScale.z);
    }
}

public void UpdateHP()
{
    if (hpBar != null)
    {
        float hpPercentage = (float)playerCurrentHP / playerMaxHP;
        hpBar.localScale = new Vector3(hpPercentage, hpBar.localScale.y, hpBar.localScale.z);
    }
}
}

using UnityEngine;

public class managerCharacterChanged : MonoBehaviour
{
    private managerCharacter mngrCharacter;
    public Transform PlayerMesh;
    private int indexPreviousCharacter;
    private Quaternion previousRotation;

    void Start()
    {
        mngrCharacter = GameObject.Find("ManagerCharacter").GetComponent<managerCharacter>(); 
        GameObject tempDefaultCharacter = Instantiate(mngrCharacter.character[0], PlayerMesh);
        tempDefaultCharacter.transform.localRotation = Quaternion.identity;

        indexPreviousCharacter = 0;
    }

    public void ChangeCharacter(int characterIndex)
    {
        if (characterIndex != indexPreviousCharacter)
        {
            // **1️⃣ ดึงค่า HP ก่อนเปลี่ยนตัวละคร**
            int currentHP = PlayerStatsManager.Instance.currentHP; 

            // **2️⃣ เก็บ Rotation ของตัวละครก่อนลบ**
            Transform currentCharacter = PlayerMesh.GetChild(0);
            previousRotation = currentCharacter.rotation;

            // **3️⃣ ลบตัวละครเดิม**
            Destroy(currentCharacter.gameObject);

            // **4️⃣ สร้างตัวละครใหม่**
            GameObject tempCharacter = Instantiate(mngrCharacter.character[characterIndex], PlayerMesh);
            tempCharacter.transform.rotation = previousRotation;

            // **5️⃣ กำหนดค่า HP ให้ตัวใหม่**
            CombatSystem combatSystem = tempCharacter.GetComponent<CombatSystem>();
            if (combatSystem != null)
            {
                combatSystem.SetHP(currentHP);
            }

            // **6️⃣ อัปเดตตัวละครปัจจุบัน**
            indexPreviousCharacter = characterIndex;
        }
    }
}

using UnityEngine;

public class managerCharacterChanged : MonoBehaviour
{
    private managerCharacter mngrCharacter;
    public Transform PlayerMesh;
    private int indexPreviousCharacter;
    private Quaternion previousRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mngrCharacter = GameObject.Find("ManagerCharacter").GetComponent<managerCharacter>(); 
        GameObject tempDefaultCharacter = mngrCharacter.character[0];
        tempDefaultCharacter.transform.localRotation = Quaternion.identity;
        
        Instantiate(tempDefaultCharacter, PlayerMesh);
        indexPreviousCharacter = 0;
        
    }

    public void ChangeCharacter(int characterIndex)
    {
        if (characterIndex != indexPreviousCharacter)
        {
            Transform currentCharacter = PlayerMesh.GetChild(0); // อ้างอิงตัวละครก่อนลบ
            previousRotation = currentCharacter.rotation;
            

            Destroy(PlayerMesh.GetChild(0).gameObject);

            GameObject tempCharacter = mngrCharacter.character[characterIndex];

            tempCharacter.transform.rotation = previousRotation;
            Instantiate(tempCharacter, PlayerMesh);

            
            
            indexPreviousCharacter = characterIndex;
            
        }

        
        
    }
}

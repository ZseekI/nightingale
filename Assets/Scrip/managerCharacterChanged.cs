using UnityEngine;

public class managerCharacterChanged : MonoBehaviour
{
    private managerCharacter mngrCharacter;
    public Transform PlayerMesh;
    private int indexPreviousCharacter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mngrCharacter = GameObject.Find("ManagerCharacter").GetComponent<managerCharacter>(); 
        GameObject tempDefaultCharacter = mngrCharacter.character[0];
        Instantiate(tempDefaultCharacter, PlayerMesh);
        indexPreviousCharacter = 0;
    }

    public void ChangeCharacter(int characterIndex)
    {
        if (characterIndex != indexPreviousCharacter)
        {
            Destroy(PlayerMesh.GetChild(0).gameObject);
            GameObject tempCharacter = mngrCharacter.character[characterIndex];
            Instantiate(tempCharacter, PlayerMesh);

            indexPreviousCharacter = characterIndex;
        }

        
        
    }
}

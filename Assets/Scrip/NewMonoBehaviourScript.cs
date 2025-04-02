using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit");
        }
    }

    void OriggerExit(Collider other)
    {
     if (other.CompareTag("PLayer"))
     {
        //Debug.Log("PlayerOut");
     }   
    }

}

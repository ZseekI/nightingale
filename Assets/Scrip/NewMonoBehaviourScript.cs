using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float forceAmount = 500f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Rigidbody rb = transform.parent.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.right * forceAmount, ForceMode.Impulse);
            }
        }
    }
}

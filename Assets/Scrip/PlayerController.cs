using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 10.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Speed * Time.deltaTime * Vector3.forward);
        }
    }
}

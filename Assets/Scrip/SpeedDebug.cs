using UnityEngine;

public class SpeedDebug : MonoBehaviour
{
    public Rigidbody rb;
    [Header("Speed KM/H")]
    public float speedKMH;
    public bool isOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            speedKMH = rb.velocity.magnitude * 3.6f;
        }
    }
}

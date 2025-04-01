using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float accelerationForce = 20f; // กำหนดค่าความเร่ง a
    public float maxWalkSpeed = 5f;
    public float maxRunSpeed = 10f;
    bool isRunning;

    public float groundDrag = 5f;
    public float airMultiplier = 0.5f;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.mass = 70f; // ตั้งมวลของตัวละคร (kg)
        isRunning = false;
    }

    private void Update()
    {
        // ตรวจสอบว่าตัวละครอยู่บนพื้นหรือไม่
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // กำหนดแรงเสียดทานของอากาศ
        rb.drag = grounded ? groundDrag : 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    private void MovePlayer()
    {
        // คำนวณทิศทางของการเคลื่อนที่
        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

        float targetSpeed = isRunning ? maxRunSpeed : maxWalkSpeed;

        // ใช้สมการ F = ma → F = mass * acceleration
        Vector3 force = moveDirection * accelerationForce * rb.mass; 

        if (grounded)
            rb.AddForce(force, ForceMode.Force); // ใช้แรงแบบปกติ
        else
            rb.AddForce(force * airMultiplier, ForceMode.Force); // ลดแรงขณะลอย
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float targetSpeed = isRunning ? maxRunSpeed : maxWalkSpeed;

        // ถ้าความเร็วเกินที่กำหนด → ลดความเร็วลง
        if (flatVel.magnitude > targetSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * targetSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}

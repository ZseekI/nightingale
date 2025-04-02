using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    public float maxSpeedIn = 5f;
    public float maxWalkSpeedKMH = 18f;  // กำหนดค่าความเร็วเดินเป็น km/h
    public float maxRunSpeedKMH = 36f;   // กำหนดค่าความเร็ววิ่งเป็น km/h
    [Header("Max Speed (When Ground Drag 0)")]
    public float walkTurnSpeed = 10f;
    public float runTurnSpeed = 5f;
    private float maxWalkSpeed;
    private float maxRunSpeed;
    public float turnSpeed;

    public bool isRunning;

    public float groundDrag = 3f;
    public float airMultiplier = 0.3f; // ลดลงเพื่อให้กลางอากาศควบคุมได้ดีขึ้น
    //public float dampingFactor = 0.98f; // ปรับลดความเร็วแทนการใช้ drag
    private Vector3 smoothMoveDirection; // เก็บค่าทิศทางเคลื่อนที่ที่ลื่นไหล

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        SetMaxSpeed();
        isRunning = false;
    }

    private void Update()
    {
        // ตรวจสอบว่าตัวละครอยู่บนพื้นหรือไม่ (ใช้ SphereCast แทน Raycast)
        grounded = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out RaycastHit hit, playerHeight * 0.5f + 0.2f, whatIsGround);

        //float speedKMH = rb.velocity.magnitude * 3.6f;
        //Debug.Log("Speed: " + speedKMH.ToString("F2") + " km/h");

        MyInput();
        SpeedControl();

        // ถ้าตัวละครอยู่บนพื้น ใช้ drag
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
    // ความเร็วปัจจุบัน
    float currentSpeed = rb.velocity.magnitude;

    //  ความเร็วสูงสุดที่ใช้ในตอนนี้
    float targetMaxSpeed = isRunning ? maxRunSpeed : maxWalkSpeed;

    // เร่งที่ต้องการ
    float targetAcceleration = targetMaxSpeed / maxSpeedIn;

    float baseAcceleration = 0.7f * targetAcceleration; // ใช้ 70% ของแรงเร่งทั้งหมดเป็นแรงเริ่มต้น

    // คำนวณอัตราเร่งที่ต้องการ
    float accelerationRatio = Mathf.Clamp01((targetMaxSpeed - currentSpeed) / targetMaxSpeed);
    
    // คำนวณ dynamicAcceleration โดยปรับขึ้นอยู่กับอัตราการเร่ง
    float dynamicAcceleration = baseAcceleration + targetAcceleration * accelerationRatio;

    // ปรับ turnSpeed ตามความเร็วที่กำลังเคลื่อนที่
    float speedRatio = Mathf.Clamp(currentSpeed / targetMaxSpeed, 0f, 1f); // ให้ค่าอยู่ในช่วง 0 - 1
    turnSpeed = Mathf.Lerp(walkTurnSpeed, runTurnSpeed, speedRatio);

    // คำนวณทิศทางของการเคลื่อนที่
    Vector3 targetDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

    // ใช้ Slerp เพื่อทำให้การเปลี่ยนทิศทางลื่นไหลขึ้น
    smoothMoveDirection = Vector3.Slerp(smoothMoveDirection, targetDirection, Time.deltaTime * turnSpeed);

    // คำนวณแรงตามอัตราเร่ง
    Vector3 force = smoothMoveDirection * dynamicAcceleration * rb.mass; 

    // ถ้าตัวละครอยู่บนพื้น (grounded) ใช้แรงแบบปกติ
    if (grounded)
    {
        rb.AddForce(force, ForceMode.Force); 
    }
    else
    {
        // ถ้าตัวละครไม่อยู่บนพื้น (air) ใช้แรงที่ลดลง
        rb.AddForce(force * airMultiplier, ForceMode.Force); 

        // ลดความเร็วในแกน X และ Z อย่างค่อยเป็นค่อยไป (ลดเฉพาะ X, Z แต่ไม่ลด Y)
        Vector3 newVelocity = rb.velocity;
        newVelocity.x = Mathf.Lerp(newVelocity.x, 0, Time.deltaTime * 5f);
        newVelocity.z = Mathf.Lerp(newVelocity.z, 0, Time.deltaTime * 5f);
        rb.velocity = newVelocity;
    }
}

    public void SetMaxSpeed()
    {
        // แปลง km/h → m/s
        maxWalkSpeed = maxWalkSpeedKMH / 3.6f;
        maxRunSpeed = maxRunSpeedKMH / 3.6f;
        Debug.Log("Walk Speed" + maxWalkSpeed + " Run Speed" + maxRunSpeed);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float targetSpeed = isRunning ? maxRunSpeed : maxWalkSpeed;

        // ✅ เพิ่มตัวแปรลดความเร็วไวขึ้น
        float slowDownFactor = (horizontalInput == 0 && verticalInput == 0) ? 50f : 5f; 

        // ถ้าความเร็วต่ำมาก ให้เพิ่มค่าต่ำสุดเพื่อให้ยังเคลื่อนที่ได้
        if (flatVel.magnitude < 0.5f && (horizontalInput != 0 || verticalInput != 0))
        {
            rb.AddForce(smoothMoveDirection * 10f, ForceMode.Force);
        }

        // ถ้าความเร็วเกินที่กำหนด → ลดความเร็วลง
        if (flatVel.magnitude > targetSpeed)
        {
                Vector3 limitedVel = flatVel.normalized * targetSpeed;
                rb.velocity = new Vector3(
                Mathf.Lerp(flatVel.x, limitedVel.x, Time.deltaTime * slowDownFactor),
                rb.velocity.y,
                Mathf.Lerp(flatVel.z, limitedVel.z, Time.deltaTime * slowDownFactor));
        }
    }
}

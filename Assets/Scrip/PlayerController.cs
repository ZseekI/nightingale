using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ============================
    // Debug variables: ใช้สำหรับตรวจสอบค่าต่าง ๆ ที่คำนวณออกมา
    // ============================
    [Header("Debug")]
    public float dynamicAcceleration;  // แรงเร่งที่คำนวณจากสูตร F = ma (ตรงกับหัวข้อ C)
    public float baseAcceleration;
    public float targetMaxSpeed;              // ความเร็วสูงสุดที่ใช้งานในตอนนี้ (m/s)
    public float accelerationRatio;           // สัดส่วนความแตกต่างระหว่างความเร็วปัจจุบันกับ maxSpeed (0-1)
    public float targetAcceleration;                // ความเร่งที่คำนวณจาก targetMaxSpeed / maxSpeedIn (m/s²) ตามกฎของนิวตัน
    public float currentSpeed;                // ความเร็วปัจจุบันของตัวละคร (m/s)
    public float currentSpeedKMH;             // ความเร็วปัจจุบันแปลงเป็น km/h (เพื่อ Debug)
    public Vector3 force;                     // แรงที่คำนวณจาก F = m * a (ตรงกับหัวข้อ C)

    // ============================
    // Movement Settings
    // ============================
    [Header("Movement")]
    public float maxSpeedIn = 5f;             // เวลาที่ต้องการให้ตัวละครเร่งจาก 0 ไปยัง maxSpeed (วินาที)
    public float maxWalkSpeedKMH = 18f;       // ความเร็วเดินที่ต้องการ (km/h) → แปลงเป็น m/s ใน SetMaxSpeed()
    public float maxRunSpeedKMH = 36f;        // ความเร็ววิ่งที่ต้องการ (km/h)
    [Header("Max Speed (When Drag = 0)")]
    public float walkTurnSpeed = 10f;         // ความเร็วหมุนตอนเดิน (สำหรับ Slerp การเปลี่ยนทิศทาง)
    public float runTurnSpeed = 5f;           // ความเร็วหมุนตอนวิ่ง (ปรับให้ตอบสนองตามความเร็ว)
    private float maxWalkSpeedms;            // ความเร็วเดิน (m/s) หลังจากแปลงจาก km/h
    private float maxRunSpeedms;             // ความเร็ววิ่ง (m/s) หลังจากแปลงจาก km/h
    public float turnSpeed;                 // ความเร็วการหมุนจริง (ปรับตามความเร็วปัจจุบัน)

    public bool isRunning;                  // กำหนดว่าตอนนี้วิ่งหรือเดิน

    // ============================
    // Physics Settings (Unity Physics 3D)
    // ============================
    [Header("Physics Settings")]
    public float groundDrag = 3f;           // Drag ที่ใช้ในพื้น (ตัวแทนแรงเสียดทานในหัวข้อ A, E)
    public float airMultiplier = 0.3f;      // ลดแรงในอากาศเพื่อจำลองแรงต้านอากาศ (หัวข้อ E)
    private Vector3 smoothMoveDirection;    // ทิศทางการเคลื่อนที่ที่ผ่านการ Slerp เพื่อความลื่นไหล (การเปลี่ยนทิศทางโดยใช้เวกเตอร์ Slerp ถือเป็นการประยุกต์ Rotational Motion – หัวข้อ F)

    // ============================
    // Ground Check Settings
    // ============================
    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;                         // ตรวจสอบว่าตัวละครอยู่บนพื้นหรือไม่ (ใช้ SphereCast จาก Unity Physics 3D)

    public Transform orientation;          // ใช้ในการคำนวณทิศทาง (เชื่อมโยงกับกล้อง)

    float horizontalInput;
    float verticalInput;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // ควบคุม Rotation ผ่านโค้ด (ใช้คำนวณแรงเร่งตามแนวแกนที่กำหนดเอง)
        SetMaxSpeed();           // แปลงค่า km/h เป็น m/s (ตรงกับโจทย์การใช้หน่วยที่ถูกต้อง)
        isRunning = false;
    }

    private void Update()
    {
        // ตรวจสอบว่าตัวละครอยู่บนพื้นหรือไม่ โดยใช้ SphereCast (การใช้ Raycast/SphereCast เป็นส่วนหนึ่งของ Unity Physics 3D – หัวข้อ A)
        grounded = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out RaycastHit hit, playerHeight * 0.5f + 0.2f, whatIsGround);

        // รับค่าอินพุตจากผู้เล่น
        MyInput();
        SpeedControl();

        // กำหนด drag เมื่ออยู่บนพื้น (Drag นี้เป็นส่วนหนึ่งของการประยุกต์แรงเสียดทานใน Unity Physics 3D – หัวข้อ E)
        rb.drag = grounded ? groundDrag : 0;

        // คำนวณความเร็วปัจจุบันใน km/h สำหรับ Debug
        currentSpeedKMH = currentSpeed * 3.6f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // รับค่าอินพุตจากคีย์บอร์ด (Input.GetAxisRaw ให้ค่าตั้งแต่ -1 ถึง 1)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // กำหนดการวิ่งเมื่อกด Left Shift
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    private void MovePlayer()
    {
        // ============================
        // คำนวณแรงเร่ง (Dynamic Acceleration) โดยใช้หลัก F = ma (ตรงกับหัวข้อ C)
        // ============================
        // คำนวณความเร็วปัจจุบัน (m/s)
        currentSpeed = rb.velocity.magnitude;

        // กำหนด targetMaxSpeed จาก maxRunSpeed หรือ maxWalkSpeed (m/s)
        targetMaxSpeed = isRunning ? maxRunSpeedms : maxWalkSpeedms;

        // คำนวณ targetAcceleration โดยใช้สูตร: a = Vmax / t 
        // ซึ่งหมายความว่า ถ้าเราต้องการให้ตัวละครไปถึงความเร็วสูงสุด (targetMaxSpeed) ภายในเวลา (maxSpeedIn) วินาที
        targetAcceleration = targetMaxSpeed / maxSpeedIn;  // หน่วย m/s²

        // accelerationRatio คำนวณเพื่อดูว่าตัวละครห่างจากความเร็วสูงสุดมากแค่ไหน
        // เมื่อ currentSpeed ต่ำกว่าความเร็วสูงสุดมาก ค่า accelerationRatio จะใกล้ 1,
        // แต่เมื่อ currentSpeed ใกล้ targetMaxSpeed, accelerationRatio จะลดลง
        accelerationRatio = Mathf.Clamp01((targetMaxSpeed - currentSpeed) / targetMaxSpeed);

        // baseAcceleration ถูกตั้งไว้ที่ 70% ของ targetAcceleration
        // เหตุผล: ให้มีแรงเร่งขั้นต่ำตลอดเวลา เพื่อให้ตัวละครไม่สูญเสียแรงเร่งเมื่อ currentSpeed ใกล้ถึง targetMaxSpeed
        // นั่นคือ แม้ accelerationRatio จะต่ำลง (เมื่อ currentSpeed ใกล้ targetMaxSpeed) ตัวละครก็ยังมีแรงเร่งพื้นฐานที่ 70% ของ targetAcceleration
        baseAcceleration = 0.7f * targetAcceleration;

        // dynamicAcceleration จะรวมทั้งแรงพื้นฐาน (baseAcceleration) และส่วนที่เพิ่มขึ้นตาม accelerationRatio
        // เมื่อ currentSpeed ต่ำกว่า targetMaxSpeed dynamicAcceleration จะสูง (เพื่อเร่งได้เร็วขึ้น)
        // เมื่อ currentSpeed ใกล้ targetMaxSpeed dynamicAcceleration จะลดลง (เพื่อป้องกันการเกินความเร็ว)
        dynamicAcceleration = baseAcceleration + (targetAcceleration * accelerationRatio);

        // ============================
        // การประยุกต์ใช้ Rotational Motion (หัวข้อ F)
        // ============================
        // ปรับ turnSpeed ตามสัดส่วนความเร็ว (speedRatio) ให้การหมุนลื่นไหลขึ้น
        float speedRatio = Mathf.Clamp(currentSpeed / targetMaxSpeed, 0f, 1f);
        turnSpeed = Mathf.Lerp(walkTurnSpeed, runTurnSpeed, speedRatio);

        // คำนวณทิศทางที่ตัวละครควรเคลื่อนที่โดยใช้ orientation
        Vector3 targetDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;
        // ใช้ Slerp เพื่อปรับเปลี่ยนทิศทางให้ลื่นไหล (การประยุกต์เวกเตอร์สำหรับการหมุน)
        smoothMoveDirection = Vector3.Slerp(smoothMoveDirection, targetDirection, Time.deltaTime * turnSpeed);

        // ============================
        // คำนวณแรงที่จะส่งให้ Rigidbody ตามหลัก F = ma
        // ============================
        // dynamicAccelerationForce = dynamicAcceleration * rb.mass; 
        // force = smoothMoveDirection * dynamicAccelerationForce
        // นี่เป็นการประยุกต์ใช้สมการ F = ma (Newton’s Second Law)
        force = smoothMoveDirection * dynamicAcceleration * rb.mass; 

        // ============================
        // ใช้แรงที่คำนวณได้เพื่อเคลื่อนที่ตัวละคร
        // ============================
        if (grounded)
        {
            rb.AddForce(force, ForceMode.Force); // ใช้แรงเต็มที่เมื่ออยู่บนพื้น (Unity Physics 3D – Collision/Force)
        }
        else
        {
            // เมื่ออยู่ในอากาศ ลดแรงลงด้วย airMultiplier (ประยุกต์ Air Resistance – หัวข้อ E)
            rb.AddForce(force * airMultiplier, ForceMode.Force);

            // ลดความเร็วในแกน X และ Z แบบค่อยๆ (ไม่ลดแกน Y เพื่อให้แรงโน้มถ่วงทำงาน)
            Vector3 newVelocity = rb.velocity;
            newVelocity.x = Mathf.Lerp(newVelocity.x, 0, Time.deltaTime * 5f);
            newVelocity.z = Mathf.Lerp(newVelocity.z, 0, Time.deltaTime * 5f);
            rb.velocity = newVelocity;
        }
    }

    // ============================
    // แปลงค่าความเร็วจาก km/h เป็น m/s (ตรงตามโจทย์การใช้งานหน่วยที่ถูกต้อง)
    // ============================
    public void SetMaxSpeed()
    {
        // แปลง km/h → m/s โดยหารด้วย 3.6
        maxWalkSpeedms = maxWalkSpeedKMH / 3.6f;
        maxRunSpeedms = maxRunSpeedKMH / 3.6f;
        Debug.Log("Walk Speed: " + maxWalkSpeedms + " m/s, Run Speed: " + maxRunSpeedms + " m/s");
    }

    // ============================
    // ควบคุมความเร็วให้ตัวละครไม่เกินความเร็วสูงสุด (Dynamic Speed Control)
    // ============================
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float targetSpeed = isRunning ? maxRunSpeedms : maxWalkSpeedms;

        // กำหนด slowDownFactor เพื่อให้ตัวละครลดความเร็วได้เร็วขึ้นเมื่อไม่มีอินพุต (Air Resistance / Friction effect)
        float slowDownFactor = (horizontalInput == 0 && verticalInput == 0) ? 50f : 5f;

        // ถ้าความเร็วต่ำมากและมีการกดอินพุต → เพิ่มแรงให้เริ่มเคลื่อนที่ (Feedback จาก Input)
        if (flatVel.magnitude < 0.5f && (horizontalInput != 0 || verticalInput != 0))
        {
            rb.AddForce(smoothMoveDirection * 10f, ForceMode.Force);
        }

        // ถ้าความเร็วเกินที่กำหนด → ลดความเร็วลงแบบค่อยๆ (การประยุกต์ใช้ Lerp เพื่อจำลอง drag/air resistance)
        if (flatVel.magnitude > targetSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * targetSpeed;
            rb.velocity = new Vector3(
                Mathf.Lerp(flatVel.x, limitedVel.x, Time.deltaTime * slowDownFactor),
                rb.velocity.y,
                Mathf.Lerp(flatVel.z, limitedVel.z, Time.deltaTime * slowDownFactor)
            );
        }
    }
}

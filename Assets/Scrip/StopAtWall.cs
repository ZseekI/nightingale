using UnityEngine;

public class StopAtWall : MonoBehaviour
{
    public Animator animator;
    public EnemyAnimationStateController _mngrEAnimation;
    public float checkDistance = 0.5f;
    public float delayTime;
    public bool canCheck = true;  // ตั้งค่าเริ่มต้นเป็น true
    public LayerMask wallLayer;

    void Update()
    {
        canCheck = _mngrEAnimation.canCheck;
        Vector3 direction = -transform.right; // ทิศทาง Raycast
        float distance = checkDistance;
        Color rayColor = Color.red;

        // วาด Raycast ให้เห็นใน Scene
        Debug.DrawRay(transform.position, direction * distance, rayColor);

        if (Physics.Raycast(transform.position, direction, checkDistance, wallLayer) && canCheck)
        {
            canCheck = false; // ปิดการตรวจสอบทันที
            _mngrEAnimation.SkipAnimation();
            Debug.Log("Stop");
        }
        else
        {
            _mngrEAnimation.wall = false;
        }
    }

}

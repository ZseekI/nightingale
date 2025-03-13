using UnityEngine;

public class LockOnCamera : MonoBehaviour
{
    public Transform target; // เป้าหมายที่ต้องการล็อก
    public Transform player; // ตัวละครของเรา
    public float smoothSpeed = 5f; // ความลื่นไหลของกล้อง

    private bool isLocked = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // กด L เพื่อล็อกเป้า
        {
            isLocked = !isLocked;
            print (("IsLock") + isLocked);
        }

        if (isLocked && target != null)
        {
            Vector3 direction = (target.position - player.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * smoothSpeed);
        }
    }
}
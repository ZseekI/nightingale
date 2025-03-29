using UnityEditor;
using UnityEngine;

public class RotateEnemy : MonoBehaviour
{
    public float rotationSpeed = 0f; // ปรับความเร็วหมุน

    void Update()
    {
        if (enemyLocation.Instance != null)
        {
            Vector3 pivotPoint = enemyLocation.Instance.GetBonePosition(); // ดึงตำแหน่งของกระดูก
            RotateAroundPoint(pivotPoint);
        }
    }

    void RotateAroundPoint(Vector3 pivot)
    {
        // คำนวณระยะห่างจากจุดศูนย์กลางที่ต้องการหมุน
        Vector3 direction = transform.position - pivot;

        // หมุนรอบแกน Y โดยให้ยังคงรักษาระยะห่างจากจุดศูนย์กลาง
        direction = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0) * direction;

        // อัปเดตตำแหน่งของวัตถุหลังหมุน
        transform.position = pivot + direction;

        // หมุนตัววัตถุให้คงทิศทางที่ถูกต้อง
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
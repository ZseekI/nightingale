using UnityEngine;

public class enemyLocation : MonoBehaviour
{
    public static enemyLocation Instance { get; private set; }
    public Transform boneTransform;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // เก็บ Instance ของ Script นี้
        }
        else
        {
            Destroy(gameObject); // ป้องกันการมีหลาย Instance
        }
    }

    void Update()
    {
        if (boneTransform != null)
        {
            transform.position = boneTransform.position; // อัปเดตตำแหน่งให้ตรงกับ Bone
        }
    }

    public Vector3 GetBonePosition()
    {
        return transform.position; // คืนค่าตำแหน่งของ Bone
    }
}

using UnityEngine;

public class Trigger : MonoBehaviour
{
    public float fLeftSpeed = -1f;
    public float bLeftSpeed = -2f;
    public float fRightSpeed = 1f;
    public float bRightSpeed = 2f;
    RotateEnemy rotateEnemy;
    private EnemyAnimationStateController _emngrAttack;
    private int triggerCount;
    void Start()
    {
        rotateEnemy = GameObject.FindGameObjectWithTag("A").GetComponent<RotateEnemy>();
        _emngrAttack = GameObject.FindGameObjectWithTag ("A").GetComponent<EnemyAnimationStateController>();
    }

    void Update()
    {
        

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerCount++; // เพิ่มจำนวน Trigger ที่ Player อยู่ในนั้น
        }
    }

    void OnTriggerStay (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (gameObject.name)
            {
                case "FLeft":
                    rotateEnemy.rotationSpeed = fLeftSpeed;
                    break;
                case "BLeft":
                    rotateEnemy.rotationSpeed = bLeftSpeed;
                    break;
                case "FRight":
                    rotateEnemy.rotationSpeed = fRightSpeed;
                    break;
                case "BRight":
                    rotateEnemy.rotationSpeed = bRightSpeed;
                    break;
                case "NearHit":
                    _emngrAttack.isNearHit = true;
                    break;
                case "Direct":
                    _emngrAttack.isDirect = true;
                    rotateEnemy.rotationSpeed = 0; // หยุดหมุน
                    break;
                
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerCount--; // ลดจำนวน Trigger ที่ Player อยู่ในนั้น

            if (triggerCount <= 0) // ถ้าไม่มี Trigger ไหนที่ Player อยู่แล้ว
            {
                ResetValues();
            }

            switch (gameObject.name)
            {
                case "NearHit":
               _emngrAttack.isNearHit = false;
                break;
                case "Direct":
               _emngrAttack.isDirect = false;
                break;
            }
        }
    }

        void ResetValues()
    {
        rotateEnemy.rotationSpeed = 0f; // ค่าเริ่มต้น
    }

}



using UnityEngine;

public class Trigger : MonoBehaviour
{
    public float fLeftSpeed = 150f;
    public float bLeftSpeed = 120f;
    public float fRightSpeed = 150f;
    public float bRightSpeed = 120f;
    Animator _enemyAnimator;
    RotateEnemy rotateEnemy;
    private bool isNearHit = false;
    private bool isDirect = false;
    void Start()
    {
        rotateEnemy = GameObject.FindGameObjectWithTag("A").GetComponent<RotateEnemy>();
        _enemyAnimator = GameObject.FindGameObjectWithTag ("A").GetComponent<Animator>();
    }

    void Update()
    {
     if (_enemyAnimator != null)
        {
            //_enemyAnimator.SetBool("isNearHit", isNearHit && isDirect);
            //_enemyAnimator.SetBool("isFarHit", !(isNearHit && isDirect));
        }   
    }

    void OnTriggerStay (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (gameObject.tag)
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
                    isNearHit = true;
                    break;
                case "Direct":
                    isDirect = true;
                    rotateEnemy.rotationSpeed = 0; // หยุดหมุน
                    break;
            }
        }
    }
}

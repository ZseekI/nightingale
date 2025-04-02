using UnityEngine;

public class enemyEventListener : MonoBehaviour
{
    [Header("Triggers to Control")]
    public Collider triggerNearAttack;
    public Collider triggerHitL;

    private bool triggerNearWasDisabled = false;
    private bool triggerHitLWasDisabled = false;

    public void HandleHitTriggerEvent(int eventID)
    {
        Debug.Log("Received Animator Event ID: " + eventID); // ✅ ตรวจสอบค่าที่ถูกส่งมา
        switch (eventID)
        {
            case 1: // ปิด triggerNearAttack
                if (triggerNearAttack != null)
                {
                    triggerNearAttack.enabled = false;
                    triggerNearWasDisabled = true;
                }
                break;

            case 2: // ปิด triggerHitL
                if (triggerHitL != null)
                {
                    triggerHitL.enabled = false;
                    triggerHitLWasDisabled = true;
                }
                break;

            case 4: // เปิด triggerNearAttack อีกครั้ง
                if (triggerNearWasDisabled && triggerNearAttack != null)
                {
                    triggerNearAttack.enabled = true;
                    triggerNearWasDisabled = false;
                }
                break;
            

            case 5: // เปิด triggerNearAttack อีกครั้ง
                if (triggerNearWasDisabled && triggerNearAttack != null)
                {
                    triggerNearAttack.enabled = true;
                    triggerNearWasDisabled = false;
                }
                break;

            case 6: // เปิด triggerHitL อีกครั้ง
                if (triggerHitLWasDisabled && triggerHitL != null)
                {
                    triggerHitL.enabled = true;
                    triggerHitLWasDisabled = false;
                }
                break;

            default:
                Debug.LogWarning("Unknown event ID: " + eventID);
                break;
        }
    }
}
using UnityEngine;

public class enemyEventListener : MonoBehaviour
{
    [Header("Triggers to Control")]
    public Collider triggerNearAttack;
    public Collider triggerHitL;
    public Collider triggerHitR;

    public void HandleHitTriggerEvent(int eventID)
    {
        Debug.Log($"[Enemy Event] Received Animator Event ID: {eventID}"); // ✅ ตรวจสอบค่าที่ถูกส่งมา

        switch (eventID)
        {
            case 1: // ปิด triggerNearAttack
                if (triggerNearAttack != null)
                {
                    triggerNearAttack.enabled = false;
                    Debug.Log("[Enemy Event] ❌ Disabled: triggerNearAttack");
                }
                break;

            case 2: // ปิด triggerHitL
                if (triggerHitL != null)
                {
                    triggerHitL.enabled = false;
                    Debug.Log("[Enemy Event] ❌ Disabled: triggerHitL");
                }
                break;

            case 3: // ปิด triggerHitR
                if (triggerHitR != null)
                {
                    triggerHitR.enabled = false;
                    Debug.Log("[Enemy Event] ❌ Disabled: triggerHitR");
                }
                break;

            case 4: // เปิด triggerNearAttack
                if (triggerNearAttack != null)
                {
                    triggerNearAttack.enabled = true;
                    Debug.Log("[Enemy Event] ✅ Enabled: triggerNearAttack");
                }
                break;

            case 5: // เปิด triggerHitL
                if (triggerHitL != null)
                {
                    triggerHitL.enabled = true;
                    Debug.Log("[Enemy Event] ✅ Enabled: triggerHitL");
                }
                break;

            case 6: // เปิด triggerHitR
                if (triggerHitR != null)
                {
                    triggerHitR.enabled = true;
                    Debug.Log("[Enemy Event] ✅ Enabled: triggerHitR");
                }
                break;

            case 7: // ปิด triggerHitL & triggerHitR พร้อมกัน
                if (triggerHitL != null && triggerHitR != null)
                {
                    triggerHitL.enabled = false;
                    triggerHitR.enabled = false;
                    Debug.Log("[Enemy Event] ❌ Disabled: triggerHitL & triggerHitR");
                }
                break;

            case 8: // เปิด triggerHitL & triggerHitR พร้อมกัน
                if (triggerHitL != null && triggerHitR != null)
                {
                    triggerHitL.enabled = true;
                    triggerHitR.enabled = true;
                    Debug.Log("[Enemy Event] ✅ Enabled: triggerHitL & triggerHitR");
                }
                break;

            default:
                Debug.LogWarning($"[Enemy Event] ⚠️ Unknown event ID: {eventID}");
                break;
        }
    }
}
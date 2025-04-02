using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerMesh;
    public Rigidbody rb;
    public float rotationSpeed;
    PlayerController playerController;

    private void Start()
    {
        FindPlayerMesh();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        rotationSpeed = playerController.isRunning ? playerController.runTurnSpeed : playerController.walkTurnSpeed;

        if (playerMesh == null)
        {
            FindPlayerMesh(); // ถ้าหาไม่เจอ ให้ลองหาใหม่
        }

        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(inputDir != Vector3.zero)
        {
            playerMesh.forward = Vector3.Slerp(playerMesh.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        else
        {
        playerMesh.forward = Vector3.Slerp(playerMesh.forward, orientation.forward, Time.deltaTime * (rotationSpeed / 2f));
        }
    }

    private void FindPlayerMesh()
    {
        GameObject meshObject = GameObject.FindGameObjectWithTag("PlayerMesh");

        if (meshObject != null)
        {
            playerMesh = meshObject.transform;
        }
        else
        {
            //Debug.LogWarning("PlayerMesh ไม่ถูกพบ! ตรวจสอบว่า Player Mesh มี Tag: PlayerMesh หรือไม่");
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
public class animationStateController : MonoBehaviour
{
    Animator animator;
    InputAction moveAction;
    InputAction runAction;
    int isWalkingHash;
    int isRunHash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunHash = Animator.StringToHash("isRun");
        
        moveAction = InputSystem.actions.FindAction("Move");
        runAction = InputSystem.actions.FindAction("Run");
    }

    // Update is called once per frame
    void Update()
    { 
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRun = animator.GetBool(isRunHash);

        bool moveActionPressed = moveAction.IsPressed();
        bool runPressed = runAction.IsPressed();

        if (!isWalking && moveActionPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (isWalking && !moveActionPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (!isRun && runPressed)
        {
            animator.SetBool(isRunHash, true);
        }

        if (isRun && !runPressed)
        {
            animator.SetBool(isRunHash, false);
        }

    }   
}

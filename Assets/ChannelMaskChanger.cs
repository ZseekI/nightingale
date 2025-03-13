using UnityEngine;
using Unity.Cinemachine;

public class ChannelMaskChanger : MonoBehaviour
{
    public CinemachineBrain brain;
    public OutputChannels Output1;
    public OutputChannels Output2;
    // Set your new desired channel mask in the inspector, or modify this value in code.
    bool IsFirstChannel = true; 
    

    void Start()
    {
        // Make sure your main camera has the CinemachineBrain component.
        brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            Debug.LogError("No CinemachineBrain found on the main camera GG.");
        }
    }

    void Update()
    {
        // Listen for the "T" key press
        if (Input.GetKeyDown(KeyCode.T) && IsFirstChannel)
        {
            // Change the channel mask to the new value
            IsFirstChannel = !IsFirstChannel;
            brain.ChannelMask = Output2;
            print(IsFirstChannel);
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.T) && !IsFirstChannel)
            {
                IsFirstChannel = !IsFirstChannel;
                brain.ChannelMask = Output1;
                print(IsFirstChannel);
            }
        }
        
        
        
        
    }
}

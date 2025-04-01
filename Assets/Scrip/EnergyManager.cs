using UnityEngine;

public class EnergyManager : MonoBehaviour 
{
    public static EnergyManager Instance;
    animationAttackStateController _mngrAttackState;
    
    public int maxEnergy = 100;
    public int currentEnergy = 0;

    [Header("EnergyUi")]
    public Transform energyBar;

    void Start()
    {
        _mngrAttackState = GameObject.FindGameObjectWithTag("CombatAnimationManager").GetComponent<animationAttackStateController>();
    }

    void Update()
    {
        if (currentEnergy != maxEnergy)
        {
            _mngrAttackState.energyCurrent = currentEnergy;
        }
    }

    void Awake() 
    {
        UpdateEnergyUI();
        if(Instance == null) 
        {
            Instance = this;
        } else 
        {
            ///Destroy(gameObject);
        }
    }
    
    public void AddEnergy(int amount) 
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        UpdateEnergyUI();
    }
    
    public bool ConsumeEnergy(int amount) {
        if(currentEnergy >= amount) {
            currentEnergy -= amount;
            UpdateEnergyUI();
            return true;
        }
        return false;
    }

    void UpdateEnergyUI() 
    {
        if (energyBar != null) 
        {
            float energyScaleX = Mathf.Clamp((float)currentEnergy / maxEnergy, 0, 1);
            energyBar.localScale = new Vector3(energyScaleX, energyBar.localScale.y, energyBar.localScale.z);
        }
    }
}


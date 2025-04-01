using System;
using Unity.VisualScripting;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    public Transform object1; // วัตถุที่ 1
    public Transform object2; // วัตถุที่ 2
    enemyAnimationStateController _mngrEnemyAnimation;
    private float distance;
    public bool check;

    void Start()
    {
        _mngrEnemyAnimation = GetComponent<enemyAnimationStateController>();
    }
    void Update()
    { 
        if (check)
        {
            Debug.Log("Distance between objects: " + distance);
        }
    }
    
    public void GetDistance()
    {
        distance = Vector3.Distance(object1.position, object2.position);
        _mngrEnemyAnimation.playerDistance = distance;
    }
}

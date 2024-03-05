using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] float pointsPerKill = 100f;
    [SerializeField] float killStreakMultiplier = 1.25f;
    [SerializeField] float killStreakTime = 2f;
    private bool isOnKillStreak = false;
    private float killTimer;
    private float currentPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyKilled()
    {

    }
}

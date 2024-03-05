using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] float pointsPerKill = 100f;
    [SerializeField] float addToMultiplierForEachKill = 0.25f;
    [SerializeField] float multiplierStart = 1f;
    [SerializeField] float killStreakTime = 2f;
    float killStreakMultiplier;
    private bool isOnKillStreak = false;
    private float killTimer;
    private float currentPoints;

    // Start is called before the first frame update
    void Start()
    {
        killStreakMultiplier = multiplierStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnKillStreak)
        {
            killTimer += Time.deltaTime;
            if (killTimer >= killStreakTime)
            {
                killTimer = 0;
                isOnKillStreak = false;
                killStreakMultiplier = multiplierStart;
            }
        }
    }

    public void EnemyKilled()
    {
        isOnKillStreak = true;
        killTimer = 0;
        currentPoints += pointsPerKill * killStreakMultiplier;
        killStreakMultiplier += addToMultiplierForEachKill;
    }
}

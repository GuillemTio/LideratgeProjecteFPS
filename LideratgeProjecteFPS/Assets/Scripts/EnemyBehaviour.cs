using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    private GameObject player;
    public TypesOfEnemyBehaviours thisEnemyBehaviour;
    public Animator animator;
    private NavMeshAgent navMeshAgent;
    public float maxDistanceFromPlayer = 1.3f;
    public bool stopped;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (thisEnemyBehaviour)
        {
            case TypesOfEnemyBehaviours.CHASINGPLAYER:
                ChasingPlayerBehaviour();
                break;
            case TypesOfEnemyBehaviours.WANDERING:
                WanderingBehaviour();
                break;
            case TypesOfEnemyBehaviours.STANDINGSTILL:
                StandingStillBehaviour();
                break;
            default:
                break;
        }
    }

    private void ChasingPlayerBehaviour()
    {
        if (!stopped)
        {

            if (!(DistanceToPlayer() < maxDistanceFromPlayer))
            {
                navMeshAgent.isStopped = false;
                animator.SetBool("IsMoving", true);
                navMeshAgent.SetDestination(player.transform.position);
            }
            else
            {
                navMeshAgent.isStopped = true;
                animator.SetBool("IsMoving", false);
            }
        }
        else
        {
            navMeshAgent.isStopped = true;
        }
    }

    private void WanderingBehaviour()
    {

    }

    private void StandingStillBehaviour()
    {

    }

    private float DistanceToPlayer()
    {
        Vector3 l_PlayerPosition = player.transform.position;
        Vector3 l_EnemyPosition = transform.position;
        Vector3 l_EnemyToPlayer = l_PlayerPosition - l_EnemyPosition;
        return l_EnemyToPlayer.magnitude;
    }


}

using UnityEngine;
using UnityEngine.AI;
public class MovementController : MonoBehaviour
{
    [SerializeField] private Transform target;
    /*private BaseUnitView parentUnit;*/
    private NavMeshAgent agent;
    private float distanceRange;
    
    /*
    public bool canAttack => agent.destination.magnitude <= distanceRange;
    */

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void Init(float speed, float attackRange)
    {
        agent.speed = speed;
        distanceRange = attackRange;
    }

    public void SetTarget(BaseUnitController targetUnit)
    {
        target = targetUnit.transform;
    }

    public void Stop()
    {
        if (agent.isActiveAndEnabled)
            agent.isStopped = true;
        
    }

    public void Resume()
    {
        if (agent.isActiveAndEnabled)
            agent.isStopped = false;

    }

    void Update()
    {
        if (target == null)
            return;
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(target.position);
        }
    }
}
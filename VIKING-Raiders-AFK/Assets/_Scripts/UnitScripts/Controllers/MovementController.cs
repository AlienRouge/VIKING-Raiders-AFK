using UnityEngine;
using UnityEngine.AI;
public class MovementController : MonoBehaviour
{
    [SerializeField] private Transform target;
    /*private BaseUnitView parentUnit;*/
    private NavMeshAgent _navMeshAgent;
    /*private float distanceRange;*/
    
    /*
    public bool canAttack => agent.destination.magnitude <= distanceRange;
    */

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }

    public void Init(float speed/*, float attackRange*/)
    {
        _navMeshAgent.speed = speed;
        /*distanceRange = attackRange;*/
    }

    public void SetTarget(BaseUnitController targetUnit)
    {
        target = targetUnit.transform;
    }

    public void Stop()
    {
        if (_navMeshAgent.isActiveAndEnabled)
            _navMeshAgent.isStopped = true;
        
    }

    public void Resume()
    {
        if (_navMeshAgent.isActiveAndEnabled)
            _navMeshAgent.isStopped = false;

    }

    void Update()
    {
        if (target == null)
            return;
        if (_navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.SetDestination(target.position);
        }
    }
}
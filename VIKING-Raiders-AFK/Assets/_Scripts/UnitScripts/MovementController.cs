using _Scripts.UnitScripts;
using _Scripts.UnitScripts.Views;
using UnityEngine;
using UnityEngine.AI;
public class MovementController : MonoBehaviour
{
    [SerializeField] private Transform target;
    /*private BaseUnitView parentUnit;*/
    private NavMeshAgent agent;
    private bool isMoving;
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

    public void SetTarget(BaseUnitView targetUnit)
    {
        target = targetUnit.transform;
    }

    public void Stop()
    {
        Debug.Log("stop");
        agent.isStopped = true;
    }

    public void Resume()
    {
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
using _Scripts.UnitScripts;
using UnityEngine;
using UnityEngine.AI;
public class MovementController : MonoBehaviour
{
    [SerializeField] private Transform target;
    private BaseUnit parentUnit;
    private NavMeshAgent agent;
    private bool isMoving;

    private void Start()
    {
        parentUnit = GetComponent<BaseUnit>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = parentUnit.HeroStats.MoveSpeed;
    }

    public void SetTarget(BaseUnit targetUnit)
    {
        target = targetUnit.transform;
    }

    public void Stop()
    {
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
        agent.SetDestination(target.position);
    }
}
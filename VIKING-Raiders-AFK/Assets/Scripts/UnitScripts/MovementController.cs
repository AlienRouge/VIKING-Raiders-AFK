using UnitScripts;
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
        agent.speed = parentUnit.heroStats.moveSpeed;
    }

    public void SetTarget(BaseUnit targetUnit)
    {
        this.target = targetUnit.transform;
    }

    public void Stop()
    {
        agent.Stop();
    }

    public void Resume()
    {
        agent.Resume();
    }

    void Update()
    {
        if (target == null)
            return;
        agent.SetDestination(target.position);
    }
}
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }

    public void Init(float speed)
    {
        _navMeshAgent.speed = speed;
    }

    public void SetTarget(BaseUnitController targetUnit)
    {
        target = targetUnit.transform;
    }

    public float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (_navMeshAgent.enabled)
            _navMeshAgent.CalculatePath(targetPosition, path);

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        // The points in between are the corners of the path.
        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0;
        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }

    public void IsStopped(bool flag)
    {
        if (_navMeshAgent.isActiveAndEnabled)
            _navMeshAgent.isStopped = flag;
    }

    public void SetMoveSpeed(float speed)
    {
        _navMeshAgent.speed = speed;
    }
    
    public void Disable()
    {
        enabled = false;
        _navMeshAgent.enabled = false;
    }

    public void Enable()
    {
        enabled = true;
        _navMeshAgent.enabled = true;
    }

    private void Update()
    {
        if (target == null)
            return;
        if (_navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.SetDestination(target.position);
        }
    }
}
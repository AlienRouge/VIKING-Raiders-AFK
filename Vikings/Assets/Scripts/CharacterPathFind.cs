using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterPathFind : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        var playerList = GameObject.FindGameObjectsWithTag("Player");
        targetTransform = playerList[Random.Range(0, playerList.Length)].transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(targetTransform.position);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [SerializeField] private Hero heroStats;
    private CharacterPathFind characterPathFind;

    private SpriteRenderer spriteRenderer;
    [SerializeField] protected GameManager.Team myTeam;
    [SerializeField] protected Transform spawnPoint;


    private void Awake()
    {
        characterPathFind = GetComponent<CharacterPathFind>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Init(GameManager.Team team, Transform spawnPos)
    {
        myTeam = team;
        spriteRenderer.color = (myTeam == GameManager.Team.Team1) ? Color.blue : Color.red;
        spawnPoint = spawnPos;
        transform.position = spawnPos.position;
    }

    public void FindTarget()
    {
        
    }
}

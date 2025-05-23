using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    [Header("Patrol Settings")]
    public EPATROLAXIS patrolAxis = EPATROLAXIS.ALL;
    public float patrolDistance = 3f;
    public List<Vector2> PatrolPositions { get; private set; } = new();

    public AudioSource AudioSource { get; private set; }

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();  
    }
    private void OnEnable()
    {
        CreatePatrolPositions();
    }

    private void CreatePatrolPositions()
    {
        PatrolPositions.Clear();
        Vector2 origin = transform.position;
        List<Vector2> dirs = new();

        switch (patrolAxis)
        {
            case EPATROLAXIS.HORIZONTAL:
                dirs.Add(Vector2.left);
                dirs.Add(Vector2.right);
                break;

            case EPATROLAXIS.VERTICAL:
                dirs.Add(Vector2.up);
                dirs.Add(Vector2.down);
                break;

            case EPATROLAXIS.ALL:
                dirs.Add(Vector2.up);
                dirs.Add(Vector2.down);
                dirs.Add(Vector2.left);
                dirs.Add(Vector2.right);
                break;
        }

        foreach (var dir in dirs)
        {
            PatrolPositions.Add(origin + dir * patrolDistance);
        }
    }

 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float attackdmg = GetComponent<EnemyStat>().Atk;
            collision.gameObject.GetComponent<BaseStat>().Damaged(attackdmg);
        }
    }


}

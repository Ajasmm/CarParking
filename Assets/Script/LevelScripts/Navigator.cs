using Ajas.FrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Navigator : MonoBehaviour
{
    [SerializeField] Transform target;

    NavMeshAgent agent;
    Transform player;

    float resetTime = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GetPlayer();
    }

    private void Update()
    {
        if(target == null || player == null) return;

        resetTime += Time.deltaTime;
        if(resetTime > 1)
        {
            resetTime = 0;
            agent.ResetPath();
        }

        agent.nextPosition = player.TransformPoint(Vector3.forward);
        agent.SetDestination(target.position);
    }

    private async void GetPlayer()
    {
        await GameManager.Instance.WaitForPlayer();
        player = GameManager.Instance.player.GetComponent<Transform>();

    }
}

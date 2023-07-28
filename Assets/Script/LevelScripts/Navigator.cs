using Ajas.FrameWork;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Navigator : MonoBehaviour
{
    [SerializeField] Transform target;

    NavMeshAgent agent;
    Transform player;

    float resetTime = 0;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(GetPlayer());
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

        agent.SetDestination(target.position);
        agent.nextPosition = player.TransformPoint(Vector3.forward);
    }

    private IEnumerator GetPlayer()
    {
        yield return GameManager.Instance.WaitForPlayerEnumerator();
        player = GameManager.Instance.player.GetComponent<Transform>();
        agent.nextPosition = player.TransformPoint(Vector3.forward);
    }
}

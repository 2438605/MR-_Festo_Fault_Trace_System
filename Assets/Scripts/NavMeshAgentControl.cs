using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentControl : MonoBehaviour
{
    private Transform player;          // Assign the player GameObject in the Inspector
    private NavMeshAgent navMeshAgent;

    public void MoveToPlayer(float waitTime)
    {
        StartCoroutine(PauseToMove(waitTime));
    }

    public IEnumerator PauseToMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        navMeshAgent = GameObject.FindGameObjectWithTag("NavMeshAgent").GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null && navMeshAgent != null)
        {
            // Update the agent's destination to the player's position
            navMeshAgent.SetDestination(player.position);
        }
        else if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing from this GameObject.");
        }
        else
        {
            Debug.LogError("Player could not be found");
        }
    }
}

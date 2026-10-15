using UnityEngine;
using UnityEngine.AI;
using UnityEngine.iOS;

public class WanderingTraderNav
{
    public NavigationNode currentNode; // 현재 위치해 있는 노드

    public NavigationNode nextNode; // 다음으로 갈 노드

    private NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public WanderingTraderNav(NavigationNode _currentNode, NavMeshAgent _agent)
    {
        currentNode = _currentNode;
        agent = _agent;
    }

    public void MoveNextNode()
    {
        nextNode = DecideNextNode();
        agent.SetDestination(nextNode.transform.position);
    }

    private NavigationNode DecideNextNode()
    {
        if (currentNode.Edges.Count == 0)
        {
            return null;
        }

        //선택확률조정
        int total_weight = 0;

        foreach (var edge in currentNode.Edges)
        {
            total_weight += edge.weight;
        }

        int randomNum = Random.Range(0, total_weight);

        foreach (var edge in currentNode.Edges)
        {
            if (randomNum < edge.weight)
                return edge.node;

            randomNum -= edge.weight;
        }

        return currentNode.Edges[currentNode.Edges.Count - 1].node;
    }

    public bool IsArrived()
    {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if(!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
            {
                return true;
            }
        }

        return false;
    }

}

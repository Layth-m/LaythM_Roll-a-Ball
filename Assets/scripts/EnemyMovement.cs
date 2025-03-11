using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{

    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;
    private NavMeshAgent navMeshAgent;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //Get the animator component
        anim = GetComponentInChildren<Animator>();
        //Set the value of speed_f
        if (anim)
        {
            anim.SetFloat("speed_f", navMeshAgent.speed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }
}

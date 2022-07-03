using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class StateMachines : MonoBehaviour
{
    public enum AISTATE { PATROL, CHASE, ATTACK };
    public GameObject[] bridge;
    private NavMeshAgent ThisAgent;
    private GameObject Player;
   public GameObject[] tanks;

    // State prop!!!
    public AISTATE CurrentState
    {
        get { return _CurrentState; }
        set
        {
            StopAllCoroutines();
            _CurrentState = value;

            switch (CurrentState)
            {
                case AISTATE.PATROL:
                    StartCoroutine(StatePatrol());
                    break;

                case AISTATE.CHASE:
                    StartCoroutine(StateChase());
                    break;

                case AISTATE.ATTACK:
                    StartCoroutine(StateAttack());
                    break;
            }
        }
    }

    // field
    [SerializeField]
    private AISTATE _CurrentState = AISTATE.PATROL;

    private void Awake()
    {
        ThisAgent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        CurrentState = AISTATE.PATROL;
      
       
    }

    public IEnumerator StateChase()
    {

       
        float AttackDistance = 2f;
       

        while (CurrentState == AISTATE.CHASE)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) < AttackDistance)
            {
                CurrentState = AISTATE.ATTACK;
                yield break;
            }
            foreach (GameObject tank in tanks)
            {
                    tank.GetComponent<StateMachinesTank>().isOkay = true;
            }

            ThisAgent.SetDestination(Player.transform.position);
            yield return null;
        }
    }

    public IEnumerator StateAttack()
    {
        float AttackDistance = 2f;
        float attackTime = 0;

        if (Player!=null)
        {
            while (CurrentState == AISTATE.ATTACK)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) > AttackDistance)
                {
                    CurrentState = AISTATE.CHASE;
                    yield break;
                }

                print("Attack!");
                ThisAgent.SetDestination(Player.transform.position);
                yield return new WaitForSeconds(1f);
                attackTime++;
                if (attackTime == 2)
                {
                    bridge[0].SetActive(true);
                    bridge[1].SetActive(true);
                }
                if (attackTime==4)
                {
                    Destroy(Player);
                    CurrentState = AISTATE.PATROL;
                   
                }
                Debug.Log(attackTime);
            }
        }
        else
        {

        }
    }

    public IEnumerator StatePatrol()
    {
        GameObject[] Waypoints = GameObject.FindGameObjectsWithTag(gameObject.tag+"Waypoint");
        GameObject CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
        float TargetDistance = 2f;

        while (CurrentState == AISTATE.PATROL)
        {
            ThisAgent.SetDestination(CurrentWaypoint.transform.position);


            if (Vector3.Distance(transform.position, CurrentWaypoint.transform.position) < TargetDistance)
            {
                CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CurrentState = AISTATE.CHASE;
        }     
    }

}

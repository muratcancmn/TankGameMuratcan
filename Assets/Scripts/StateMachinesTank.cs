using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class StateMachinesTank : MonoBehaviour
{

    NavMeshAgent thisAgent;
    GameObject tower;
    public GameObject[] bridges;
    public bool isOkay=false;
    
    
    private void Awake()
    {

        thisAgent = GetComponent<NavMeshAgent>();
        tower = GameObject.FindGameObjectWithTag(gameObject.name+"Tower");

    }
    // Update is called once per frame
    void Update()
    {
        TankThings();
    }
    private void TankThings()
    {
        if (isOkay==false)
        {
            StartCoroutine(Patrol());
           
        }
        else
        {
            StopAllCoroutines();
    
               thisAgent.SetDestination(tower.transform.position);
        }


    }
    public IEnumerator Patrol()
    {
        GameObject[] Waypoints = GameObject.FindGameObjectsWithTag(gameObject.tag + "Waypoint");
        GameObject CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
        float TargetDistance = 2f;
        while (bridges[0].active == false)
        {
            thisAgent.SetDestination(CurrentWaypoint.transform.position);


            if (Vector3.Distance(transform.position, CurrentWaypoint.transform.position) < TargetDistance)
            {
                CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
            }

            yield return null;
        }

       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(gameObject.name+"Tower"))
        {
            Destroy(other.gameObject);
            isOkay = false;
         
        }
    }
}

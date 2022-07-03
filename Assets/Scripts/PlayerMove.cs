using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float force = 10;
   public bool isAlive = true;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Moves();
        
    }
    private void Moves()
    {
        if (gameObject!=null)
        {
            float horizVel = Input.GetAxis("Horizontal") * force;
            float vertVel = Input.GetAxis("Vertical") * force;
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(horizVel, 0, vertVel);
        }
        else
        {

        }

    }
}

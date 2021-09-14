using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_In_Aggression_Area : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player enters aggression area !!!");
            GetComponentInParent<Enemy_Cyclops>().CheckSight();
        }
    }
}

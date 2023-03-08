using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBathroomTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameStateManager.instance.NextState();
        }
    }
}

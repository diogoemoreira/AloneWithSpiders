using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkCupEnterTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cup")
        {
            GameStateManager.instance.NextState();
        }
    }
}

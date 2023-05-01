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

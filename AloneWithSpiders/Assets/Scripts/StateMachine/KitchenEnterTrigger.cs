using UnityEngine;

public class KitchenEnterTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameStateManager.instance.NextState();
        }
    }
}

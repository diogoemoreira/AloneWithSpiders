using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

public class TerrariumSpiderEnterTrigger : MonoBehaviour
{
    private List<GameObject> spiders;
    private bool singleSpider;
    private void Start()
    {
        spiders = new List<GameObject>();
        singleSpider = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Spider")
        {
            if (singleSpider)
            {
                singleSpider = false;
                other.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                other.gameObject.GetComponent<NavMeshAgent>().areaMask = 1 << NavMesh.GetAreaFromName("Terrarium");
                other.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                GameStateManager.instance.NextState();
                spiders.Add(other.gameObject);
            }
            else
            {
                if (!spiders.Contains(other.gameObject))
                {
                    spiders.Add(other.gameObject);
                    other.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                    other.gameObject.GetComponent<NavMeshAgent>().areaMask = 1 << NavMesh.GetAreaFromName("Terrarium");
                    other.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                }
                if (spiders.Count == 6)
                {
                    GameStateManager.instance.NextState();
                }
            }
        }
    }
}

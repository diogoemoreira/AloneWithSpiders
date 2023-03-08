using UnityEngine;

public class InteractorManager : MonoBehaviour
{
   public static InteractorManager instance;
   public GameObject[] RayInteractors;
   public GameObject[] DirectInteractor;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void changeInteractors(bool activateRay){
        //change which interactors are active
        foreach(var item in RayInteractors){
            item.SetActive(activateRay);
        }
        foreach(var item in DirectInteractor){
            item.SetActive(!activateRay);
        }
    }
}

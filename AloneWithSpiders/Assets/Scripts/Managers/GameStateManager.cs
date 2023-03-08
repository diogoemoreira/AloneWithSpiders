using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public Animator stateMachine;

    public bool forceNextState;

    public GameObject spiderPrefab;

    ////
    public Transform spawnPosition;
    public GameObject player;
    public Collider kitchenCollider;
    public GameObject cupToClean;
    public Collider bathroomCollider;
    public Transform bathroomSpiderSpawnPos;
    public Collider terrariumCollider;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if (forceNextState)
        {
            NextState();
            forceNextState = false;
        }
    }

    public void NextState()
    {
        stateMachine.SetTrigger("NextState");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchSpidersBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameStateManager.instance.terrariumCollider.enabled = true;
        SpiderSpawner.instance.SpawnSpiders(5);
        UITaskManager.instance.SetCurrentTask("- Put in the terrarium all the spiders");

        SubtitlesManager.instance.DisplaySubtitles("Can you make sure all the spiders are still inside?");
        SubtitlesManager.instance.DisplaySubtitles("If not there should be a total of 6 spiders. Since you already got one there are only 5 more.");
        SubtitlesManager.instance.DisplaySubtitles("After you've caught them all feel free to leave.");
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameStateManager.instance.terrariumCollider.enabled = false;
        UITaskManager.instance.SetCurrentTask("");
        SubtitlesManager.instance.DisplaySubtitles("Thank you very much. Hope to see you tomorrow again. ^^");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSpiderBehaviour : StateMachineBehaviour
{
    GameStateManager gsm;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gsm = animator.GetComponent<GameStateManager>();
        Instantiate(gsm.spiderPrefab, gsm.bathroomSpiderSpawnPos);
        gsm.terrariumCollider.enabled = true;
        UITaskManager.instance.SetCurrentTask("- Grab the spider and place it in the terrarium");

        //should only trigger when entering the bathroom or picking up the first spider
        SubtitlesManager.instance.DisplaySubtitles("You found a spider?");
        SubtitlesManager.instance.DisplaySubtitles("I must have forgotten to close the spiders' terrarium!");
        SubtitlesManager.instance.DisplaySubtitles("They are harmless so just grab them and leave them in the terrarium placed in the bedroom");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //gsm.terrariumCollider.enabled = false;
    }
}

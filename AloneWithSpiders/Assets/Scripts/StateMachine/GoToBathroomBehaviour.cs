using UnityEngine;

public class GoToBathroomBehaviour : StateMachineBehaviour
{
    GameStateManager gsm;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gsm = animator.GetComponent<GameStateManager>();
        gsm.bathroomCollider.enabled = true;
        UITaskManager.instance.SetCurrentTask("- Go to the bathroom");

        SubtitlesManager.instance.DisplaySubtitles("Now, next to the kitchen, you have the bedroom and the bathroom.");
        SubtitlesManager.instance.DisplaySubtitles("Go check out the bathroom first.");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gsm.bathroomCollider.enabled = false;
    }

}

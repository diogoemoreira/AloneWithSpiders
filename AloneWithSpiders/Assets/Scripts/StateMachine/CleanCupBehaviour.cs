using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CleanCupBehaviour : StateMachineBehaviour
{
    GameStateManager gsm;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gsm = animator.GetComponent<GameStateManager>();
        gsm.cupToClean.GetComponent<XRGrabInteractable>().enabled = true;
        UITaskManager.instance.SetCurrentTask("- Grab the cup and place it in the sink");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gsm.cupToClean.GetComponent<XRGrabInteractable>().enabled = false;
    }
}

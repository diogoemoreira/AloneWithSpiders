using UnityEngine;

public class InitialSpawnBehaviour : StateMachineBehaviour
{
    GameStateManager gsm;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gsm = GameStateManager.instance;
        gsm.player.transform.position = gsm.spawnPosition.position;
        gsm.player.transform.rotation = gsm.spawnPosition.rotation;
        gsm.kitchenCollider.enabled = true;
        UITaskManager.instance.SetCurrentTask("- Go to the kitchen");

        SubtitlesManager.instance.DisplaySubtitles("Hey, are you the new housekeeper?");
        SubtitlesManager.instance.DisplaySubtitles("Nice, your first day at the job is really easy! Go to the kitchen for now.");
        SubtitlesManager.instance.DisplaySubtitles("The house should be mostly clean but I think I left a cup on the kitchen's table.");
        SubtitlesManager.instance.DisplaySubtitles("Just put it in the sink for now.");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gsm.kitchenCollider.enabled = false;
    }
}

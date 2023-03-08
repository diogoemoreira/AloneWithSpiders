using UnityEngine;

public class UIManager : MonoBehaviour
{
    //singleton can be read from other scripts but only set within its own class
    public static UIManager instance {get; private set; }
    //
    
    public UIPause pauseUI = null;
    bool paused=false;

    //control variables
    bool justChanged = false;
    //

    UIInterface ui = null;

    private void Start() {
        if(instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }

    // Update is called once per frame
    void Update() {
        //confirm if the interface isn't locked and the cancel button was pressed
        if(Input.GetButtonDown("Cancel")) {
            //confirm if control variables werent just changed
            if (!justChanged) {
                if (paused) {
                    if (ui != null){
                        ActivateInterface();
                        Reset();
                    }
                }
                else {
                    PauseKeyDown();
                }
            }
            else {
                justChanged = false;
            }
        }
    }

    void PauseKeyDown(){
        //activate pause menu
        pauseUI.Activate();
        paused= true;
        ui = (UIInterface) pauseUI;
    }

    public void uiKeyDown(UIInterface ui_if){
        //used by any interface element that pauses the game
        ui_if.Activate();
        paused= true;
        ui = ui_if;
    }

    public bool IsPaused(){
        return instance.paused;
    }

    public void ActivateInterface(){
        //activate interface (either pauses or resumes the game)
        ui.Activate();
    }

    public void Reset(){
        ui = null;
        paused = false;
    }
}

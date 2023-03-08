using UnityEngine;

public static class CameraLockData {
    private static bool locked = false;

    public static void setLock(bool status){
        locked=status;
        if(locked){
            Cursor.lockState = CursorLockMode.Locked;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public static bool getLock(){
        return locked;
    }
}
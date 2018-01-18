using UnityEngine;
using System.Collections;

public class tempScript : MonoBehaviour {

	void Update () {
        if(Screen.sleepTimeout != SleepTimeout.NeverSleep) {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
	    if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
	}
}

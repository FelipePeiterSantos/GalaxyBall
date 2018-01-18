using UnityEngine;
using System.Collections;

public class sceneManager : MonoBehaviour {

    public bool isRightSide;
    public GameObject[] scenes;
    public Canvas passwordCanvas;
    public UnityEngine.UI.InputField inputPass;
    public string password;

    void OnEnable() {
        passwordCanvas.enabled = false;
        for (int i = 0; i < scenes.Length; i++)        {
            if(i == 0) {
                scenes[i].SetActive(true);
            }
            else {
                scenes[i].SetActive(false);
            }
        }
    }

    public void LoadScene(int aux) {
        for (int i = 0; i < scenes.Length; i++)        {
            if(i == aux) {
                scenes[i].SetActive(true);
            }
            else {
                scenes[i].SetActive(false);
            }
        }
        if(passwordCanvas.enabled) {
            passwordCanvas.enabled = false;
        }
        SFXManager.PlaySFX(4,isRightSide);
    }

    public void Password() {
        if(passwordCanvas.enabled) {
            passwordCanvas.enabled = false;
            CancelInvoke("InvokeInputField");
        }
        else {
            passwordCanvas.enabled = true;
            InvokeRepeating("InvokeInputField",.5f,.5f);
        }
    }

    void InvokeInputField() {
        if(inputPass.text == password) {
            LoadScene(4);
            passwordCanvas.enabled = false;
            CancelInvoke("InvokeInputField");
            inputPass.text = "";
        }
    }
}

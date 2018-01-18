using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class keyboardManager : MonoBehaviour {

    public InputField inputName;
    [System.Serializable]public struct KboardBtns {
        public string name;
        public Text btnText;
    }
    public KboardBtns[] keysTxt;
    public Image shiftKey;

    bool uppercase;

    void OnEnable() {
        uppercase = true;
        shiftKey.color = uppercase ? Color.cyan: Color.white;
    }

    public void OnShiftKey() {
        uppercase = !uppercase;
        shiftKey.color = uppercase ? Color.cyan: Color.white;
        if(uppercase) {
            for (int i = 0; i < keysTxt.Length; i++){
                keysTxt[i].btnText.text = keysTxt[i].name.ToUpper();
            }
        }
        else {
            for (int i = 0; i < keysTxt.Length; i++){
                keysTxt[i].btnText.text = keysTxt[i].name.ToLower();
            }
        }
    }

    public void OnKeyDonw(Text keyPressed) {
        inputName.text += keyPressed.text;
        uppercase = true;
        OnShiftKey();
    }

    public void OnSpaceDown() {
        inputName.text += " ";
        uppercase = false;
        OnShiftKey();
    }

    public void OnBackspaceDown() {
        if(inputName.text.Length > 0) {
            inputName.text = inputName.text.Substring(0,inputName.text.Length-1);
            if(inputName.text.Length <= 0 && !uppercase) {
                OnShiftKey();
            }
        }
        else if(!uppercase) {
            OnShiftKey();
        }
    }

    public void OpenKeyboard() {
        this.gameObject.SetActive(true);
    }

    public void ExitKeyboard() {
        this.gameObject.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class config : MonoBehaviour {

    public Dropdown removeDropdown;
    public Text dropdownText;
    public InputField addField;
    public Toggle[] toggles;

    void OnEnable() {
        LoadList();
        RefreshToggles();
    }

    void LoadList() {
        removeDropdown.ClearOptions();
        List<Dropdown.OptionData> optionDataList = new List<Dropdown.OptionData>();
        List<string> aux = Settings.LoadList();
        for (int i = 0; i < aux.Count; i++){
            optionDataList.Add(new Dropdown.OptionData() { text = aux[i]});
        }
        removeDropdown.AddOptions(optionDataList);
    }

    void RefreshList() {
        removeDropdown.ClearOptions();
        List<Dropdown.OptionData> optionDataList = new List<Dropdown.OptionData>();
        List<string> aux = Settings.listSchools;
        for (int i = 0; i < aux.Count; i++){
            optionDataList.Add(new Dropdown.OptionData() { text = aux[i]});
        }
        removeDropdown.AddOptions(optionDataList);
    }

    void RefreshToggles() {
        bool[] toggleStat = Settings.LoadToggles();
        for (int i = 0; i < toggles.Length; i++){
            toggles[i].isOn = toggleStat[i];
        }
    }

    public void AddButton() {
        if(addField.text != "") {
            Settings.AddToList(addField.text);
            addField.text = "";
        }
        RefreshList();
    }

    public void RemoveButton() {
        Settings.RemoveToList(dropdownText.text);
        RefreshList();
    }

    public void WipeButton() {
        Settings.WipeList();
        RefreshList();
    }

    public void MainMenuButton() {
        Settings.SaveList();
        Settings.SaveToggles();
    }

    public void ChangeValueToggle() {
        if (toggles[0].IsActive()) {
            for (int i = 0; i < toggles.Length; i++){
                Settings.activePlanets[i] = toggles[i].isOn.ToString();
            }
        }
    }
}

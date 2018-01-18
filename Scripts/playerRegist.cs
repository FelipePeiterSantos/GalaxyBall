using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class playerRegist : MonoBehaviour {

    public Dropdown schoolDropdown;
    public Text ptsTxt;
    public InputField playerName;
    public Text[] schoolAndCountry;
    public ranking finalPts;
    public int whatPlayer;
    public GameObject keyboard;

    void OnEnable() {
        playerName.text = "";
        schoolDropdown.ClearOptions();
        List<Dropdown.OptionData> optionDataList = new List<Dropdown.OptionData>();
        List<string> aux = Settings.LoadList();
        for (int i = 0; i < aux.Count; i++){
            optionDataList.Add(new Dropdown.OptionData() { text = aux[i]});
        }
        schoolDropdown.AddOptions(optionDataList);
        ptsTxt.text = finalPts.GetPointOfPlayer(whatPlayer).ToString("0000");
        keyboard.SetActive(false);
    }

    public void Regist() {
        List<Settings.RankingList> list = Settings.LoadRanking();
        Settings.RankingList addTo = new Settings.RankingList();
        addTo.points =  finalPts.GetPointOfPlayer(whatPlayer);
        addTo.text = playerName.text + ", " + schoolAndCountry[0].text + ", " + schoolAndCountry[1].text;
        list.Add(addTo);
        Settings.SaveRanking(list);
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class globalRanking : MonoBehaviour {

    public UnityEngine.UI.Text[] textPlacement,ptsPlacement;
	
    void OnEnable() {
        for (int i = 0; i < textPlacement.Length; i++){
            textPlacement[i].text = "";
            ptsPlacement[i].text = "";
        }
        List<Settings.RankingList> list = Settings.LoadRanking();
        list.Sort((s2, s1) => s1.points.CompareTo(s2.points));
        for (int i = 0; i <= textPlacement.Length; i++){
            if(list.Count > i) {
                if(!(list[i].points + list[i].text).Contains("999IGNOREME")) {
                    ptsPlacement[i-1].text = list[i].points.ToString();
                    textPlacement[i-1].text = list[i].text;
                }
            }
        }
    }
}

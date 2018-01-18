using UnityEngine;
using System.Collections;

public class ranking : MonoBehaviour {

    public bool isRightSide;
    [System.Serializable] public struct TextPlacement {
        public UnityEngine.UI.Text planetP1,planetP2, player1, player2;
    }
    public TextPlacement[] placeText;
    public TextPlacement finalScore;
    public GameObject rankingPanel;
    public UnityEngine.UI.Button btn;

    int placeCount;
    int ptsP1, ptsP2;

    void OnEnable() {
        rankingPanel.SetActive(false);
        ptsP1 = 0;
        ptsP2 = 0;
        placeCount = 0;
        for (int i = 0; i < placeText.Length; i++){
            placeText[i].planetP1.text = "";
            placeText[i].planetP2.text = "";
            placeText[i].player1.text = "";
            placeText[i].player2.text = "";
        }
        finalScore.player1.text = "";
        finalScore.player2.text = "";
    }

    public void hideScoreboard() {
        rankingPanel.SetActive(false);
    }

    public void btnScoreboard(bool aux) {
        if(!rankingPanel.activeSelf) {
            rankingPanel.SetActive(true);
            SFXManager.PlaySFX(4,isRightSide);
            finalScore.player1.text = ptsP1.ToString("00");
            finalScore.player2.text = ptsP2.ToString("00");
            if(ptsP1 > ptsP2) {
                finalScore.player2.color = Color.gray;
            }
            else if(ptsP1 < ptsP2) {
                finalScore.player1.color = Color.gray;
            }
        }
        if (btn != null) {
            btn.interactable = aux;
        }
    }

    public void AddToRank(string planet, int p1, int p2) {
        placeText[placeCount].planetP1.text = planet;
        placeText[placeCount].planetP2.text = planet;
        placeText[placeCount].player1.text = p1.ToString("00");
        placeText[placeCount].player2.text = p2.ToString("00");
        placeText[placeCount].player1.color = Color.white;
        placeText[placeCount].player2.color = Color.white;
        if(p1 > p2) {
            placeText[placeCount].player2.color = Color.gray;
        }
        else if(p1 < p2) {
            placeText[placeCount].player1.color = Color.gray;
        }
        ptsP1 += p1;
        ptsP2 += p2;
        placeCount++;
    }

    public int GetPointOfPlayer(int aux) {
        if (aux == 0) {
            return ptsP1;
        }
        else {
            return ptsP2;
        }
    } 
}

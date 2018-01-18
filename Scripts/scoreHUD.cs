using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class scoreHUD : MonoBehaviour {

    public Transform cameraRotation;
    public RectTransform canvasRotation;
    public Text[] scorePointTxt;
    public ranking[] rankingScript;

    int[] scorePlayer;

    string[] planetName = new string[] { "Terra", "Lua", "Marte", "Saturno", "Netuno" };
    
    int allPts;
    bool player1Turn;
    int playerTurn;

	void OnEnable () {
        allPts = 0;
        TurnFeed(1);
        player1Turn = true;
        scorePointTxt[0].color = Color.white;
        scorePointTxt[1].color = Color.clear;
        if(scorePlayer == null) {
            scorePlayer = new int[scorePointTxt.Length];
        }
        for (int i = 0; i < scorePointTxt.Length; i++)        {
            scorePlayer[i] = 0;
            scorePointTxt[i].text = scorePlayer[i].ToString("0000");
        }
	}
	
	void TurnFeed (int aux) {
        playerTurn = aux;
        if (aux == 1) {
            cameraRotation.rotation = Quaternion.Euler(0, 0, 0);
            canvasRotation.localRotation = Quaternion.Euler(0, 0, 0);
            scorePointTxt[0].color = Color.white;
            scorePointTxt[1].color = Color.clear;
        }
        else if(aux == 2) {
            cameraRotation.rotation = Quaternion.Euler(0, 0, 180);
            canvasRotation.localRotation = Quaternion.Euler(0, 0, 180);
            scorePointTxt[0].color = Color.clear;
            scorePointTxt[1].color = Color.white;
        }
	}

    public void AddPointToPlayer() {
        bool aux = player1Turn;
        if (aux) {
            scorePlayer[0]++;
            scorePointTxt[0].text = scorePlayer[0].ToString("0000");
        }
        else {
            scorePlayer[1]++;
            scorePointTxt[1].text = scorePlayer[1].ToString("0000");
        }
    }

    public void SwitchTurn() {
        player1Turn = !player1Turn;
        TurnFeed(player1Turn ? 1: 2);
    }

    public void ResetPoints(int aux) {
        allPts += scorePlayer[0] + scorePlayer[1];
        rankingScript[0].AddToRank(planetName[aux],scorePlayer[0],scorePlayer[1]);
        rankingScript[1].AddToRank(planetName[aux],scorePlayer[0],scorePlayer[1]);
        scorePlayer[0]=0;
        scorePointTxt[0].text = scorePlayer[0].ToString("0000");
        scorePlayer[1]=0;
        scorePointTxt[1].text = scorePlayer[1].ToString("0000");
    }

    public int PlayerTurn() {
        return playerTurn;
    }
}

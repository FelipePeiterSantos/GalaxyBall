using UnityEngine;
using System.Collections;

public class timer : MonoBehaviour {

    [System.Serializable]public struct Gravity {
        public string name;
        public float gravity;
        public GameObject planet;
    }

    public UnityEngine.UI.Text timerTxt;
    public UnityEngine.UI.Text planetName;
    public Gravity[] planets;
    public ballthrow ballScript;
    public scoreHUD scoreScript;
    public ranking[] finalScoreboard;

    int planet;
    float timerCount;
    int[] minuteSecond = new int[2];
    bool[] planetActive;
    bool roundEnded;
    bool gameEnded;

	void OnEnable () {
        timerCount = 60f;
        gameEnded = false;
        roundEnded = false;
        minuteSecond[0] = Mathf.FloorToInt(timerCount/60f);
        minuteSecond[1] = Mathf.RoundToInt(timerCount%60f);
        planetActive = Settings.LoadToggles();
        planet = 0;
        while (!planetActive[planet]) {
            planet++;
            if(planet > planets.Length-1) {
                planet = 0;
            }
        }
        planetName.text = planets[planet].name;
        for (int i = 0; i < planets.Length; i++){
            if(i == planet) {
                planets[planet].planet.SetActive(true);
            }
            else {
                planets[i].planet.SetActive(false);
            }
        }
        ballScript.SetGravity(planets[planet].gravity);
	}
	
	void FixedUpdate () {
        timerCount -= Time.deltaTime;
        minuteSecond[0] = Mathf.FloorToInt(timerCount/60f);
        minuteSecond[1] = Mathf.FloorToInt(timerCount % 60f);

        if(timerCount <= 0f && ballScript.BallOnAir()) {
            timerCount = 0f;
        }

        if(timerCount < -5f && !gameEnded) {
            if(NextPlanet()) {
                finalScoreboard[0].hideScoreboard();
                finalScoreboard[1].hideScoreboard();
                ballScript.StartBall();
                roundEnded = false;
                timerCount = 60f;
                ballScript.SetGravity(planets[planet].gravity);
                planetName.text = planets[planet].name;
                for (int i = 0; i < planets.Length; i++){
                    if(i == planet) {
                        planets[planet].planet.SetActive(true);
                    }
                    else {
                        planets[i].planet.SetActive(false);
                    }
                }
            }
        }
        else if(timerCount < 0f && !gameEnded && !roundEnded) {
            roundEnded = true;
            ballScript.StopBall();
            ballScript.ResetThrow();
            scoreScript.ResetPoints(planet);
            ShowScore();
        }


        if(timerTxt.text != minuteSecond[0].ToString("0")+minuteSecond[1].ToString(":00") && timerCount > 0) {
            timerTxt.text = minuteSecond[0].ToString("0")+minuteSecond[1].ToString(":00");
        }
	}

    bool NextPlanet() {
        planet++;
        if(planet >= planets.Length) {
            EndGame();
            return false;
        }
        else {
            while (!planetActive[planet]) {
                planet++;
                if(planetActive.Length <= planet) {
                    break;
                }
            }
        }
        if(planet >= planetActive.Length) {
            EndGame();
            return false;
        }
        return true;
    }

    void ShowScore() {
        finalScoreboard[0].btnScoreboard(false);
        finalScoreboard[1].btnScoreboard(false);
    }

    void EndGame() {
        finalScoreboard[0].btnScoreboard(true);
        finalScoreboard[1].btnScoreboard(true);
        gameEnded = true;
    }
}

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Settings : MonoBehaviour {

    public static List<string> listSchools = new List<string>();
    public static string[] activePlanets = new string[5];
    public struct RankingList {
        public int points;
        public string text;
    }

    static string schoolListPath;
    static string toggleListPath;
    static string rankListPath;

    public static void AddToList(string aux) {
        listSchools.Add(aux);
    }
    public static void RemoveToList(string aux) {
        listSchools.Remove(aux);
    }
    public static void WipeList() {
        listSchools = new List<string>();
    }

    public static List<string> LoadList() {
        schoolListPath = Application.persistentDataPath+"/schoolList.txt";
        listSchools = new List<string>();
        if(File.Exists(schoolListPath)) {
            string[] aux = File.ReadAllText(schoolListPath).Split('\n');
            for (int i = 0; i < aux.Length-1; i++){
                listSchools.Add(aux[i]);
            }
        }
        else {
            File.Create(schoolListPath).Close();
            listSchools = new List<string>();
            SaveList();
        }

        return listSchools;
    }

    public static void SaveList() {
        if(!File.Exists(schoolListPath)) {
            File.Create(schoolListPath).Close();
        }
        string[] aux = new string[listSchools.Count];
        for (int i = 0; i < listSchools.Count; i++){
            aux[i] = listSchools[i];
        }
        File.WriteAllLines(schoolListPath,aux);
    }

    public static bool[] LoadToggles(){
        toggleListPath = Application.persistentDataPath+"/planetToggles.txt";
        activePlanets = new string[5];
        if(File.Exists(toggleListPath)) {
            activePlanets = File.ReadAllText(toggleListPath).Split('\n');
        }
        else {
            File.Create(toggleListPath).Close();
            activePlanets = new string[5];
            for (int i = 0; i < activePlanets.Length; i++){
                activePlanets[i] = "True";
            }
            SaveToggles();
        }

        bool[] aux = new bool[5];
        for (int i = 0; i < aux.Length; i++){
            aux[i] = activePlanets[i].Contains("True");
        }

        return aux;
    }

    public static void SaveToggles() {
        if(!File.Exists(toggleListPath)) {
            File.Create(toggleListPath).Close();
        }
        File.WriteAllLines(toggleListPath,activePlanets);
    }

    public static List<RankingList> LoadRanking() {
        rankListPath = Application.persistentDataPath+"/globalRank.txt";
        List<RankingList> rankReturn = new List<RankingList>();
        if(!File.Exists(rankListPath)) {
            SaveRanking(new List<RankingList>());
        }
        else {
            RankingList aux1 = new RankingList();
            string[] aux = File.ReadAllText(rankListPath).Split('\n');
            string[] split = new string[2];
            for (int i = 0; i < aux.Length-1; i++){
                split = aux[i].Split( '|');
                aux1.points = int.Parse(split[0]);
                aux1.text = split[1];
                rankReturn.Add(aux1);
            }
        }
        
        return rankReturn;
    }

    public static void SaveRanking(List<RankingList> saveList) {
        if (!File.Exists(rankListPath)) {
            File.Create(rankListPath).Close();
            string[] aux = new string[] { "999|IGNOREME"};
            File.WriteAllLines(rankListPath,aux);
        }
        else {
            string[] aux = new string[saveList.Count];
            for (int i = 0; i < saveList.Count; i++){
                aux[i] = saveList[i].points + "|" + saveList[i].text;
            }
            File.WriteAllLines(rankListPath,aux);
        }
    }
}

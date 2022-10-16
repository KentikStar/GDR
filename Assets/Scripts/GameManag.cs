using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManag : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textCoin;
    [SerializeField]
    TextMeshProUGUI textLevel;
    [SerializeField]
    GameObject startUI;
    [SerializeField]
    GameObject restartUI;

    [SerializeField]
    GameObject coinPrefab;

    [SerializeField]
    GameObject thornPrefab;

    [SerializeField]
    private int countObjects = 1;

    private float xRange = 2f, yRange = 4.5f;

    Transform parentObjects;

    private int coinCount;

    public int CoinCount{
        get{
            return coinCount;
        }
        set{
            coinCount = value;
            ChangeTextCoin(CoinCount);
        }
    }

    private int level;

    public int Level{
        get{
            return level;
        }
        set{
            level = value;
            ChangeTextLevel(Level);
        }
    }

    private static bool isAlive;

    public static bool IsAlive{
        get{
            return isAlive;
        }
        set{
            isAlive = value;            
        }
    }

    [SerializeField]
    private CharacterCtrl player;
    private Vector3 startPlayerPos;

    void Start()
    {        
        startPlayerPos = player.transform.position;
    }

    public void StartGame(){
        CoinCount = 0;
        Level = 1;
        IsAlive = true;

        startUI.SetActive(false);

        StartLevel();
    }

    private void StartLevel(){
        parentObjects = new GameObject().transform;

        Spawn(coinPrefab);
        Spawn(thornPrefab);
    }

    private void NextLevel(){
        Level++;
        if(level % 3 == 0)
            countObjects++;

        player.ClearQueue();
        player.transform.position = startPlayerPos;

        Destroy(parentObjects.gameObject);

        StartLevel();
    }

    public void RestartGame(){
        CoinCount = 0;
        level = 0;
        countObjects = 1;

        restartUI.SetActive(false);
        IsAlive = true;

        NextLevel();
    }

    private void ChangeTextCoin(int value){
        textCoin.text = $"COIN: {value}";
    }

    private void ChangeTextLevel(int value){
        textLevel.text = $"LVL: {value}";
    }

    public void EndGame(){
        IsAlive = false;
        restartUI.SetActive(true);
        
    }

    public void ExitGame(){
        Application.Quit();
    }

    private void Spawn(GameObject prefab){
        for(int i = 0; i < countObjects; i++){
            Vector2 prefabPosition;
            do{
                float xPoint = RandomPoint(xRange);
                float yPoint = RandomPoint(yRange);
                
                prefabPosition = new Vector2(xPoint,yPoint);
            }while(CheckPosition(prefabPosition));

            Instantiate(prefab,prefabPosition,Quaternion.identity,parentObjects);
        }
    }

    private bool CheckPosition(Vector3 position){
        for(int i = 0; i < parentObjects.childCount; i++){
            if(parentObjects.GetChild(i).position == position)
                return true;
        }

        return false;
    }
    
    private float RandomPoint(float value){
        double tmp, number = UnityEngine.Random.Range(-value,value);
        if ((tmp = number % 0.5) != 0) 
            number += number > -1 ? (0.5 - tmp) : -tmp;

        return Convert.ToSingle(number);
    }

    public void CheckCountCoin(){
        int countCoin = countObjects;

        for(int i = 0; i < parentObjects.childCount; i++){
            if(parentObjects.GetChild(i).tag == "Coin" && !parentObjects.GetChild(i).gameObject.activeSelf)
                countCoin--;
        }

        if(countCoin == 0)
            NextLevel();
    }

    
}

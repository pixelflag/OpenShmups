using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private StageManager stageManager = default;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        HadManager.instance.ResetAll();

        stageManager.Initialize();
        stageManager.OnGameOver = GameOver;
        stageManager.CreateStage(1);
        stageManager.GameStart();
    }

    private void GameOver()
    {
        stageManager.CreateStage(1);
        stageManager.GameStart();
    }

    private void FixedUpdate()
    {
        stageManager.Excute();
    }
}
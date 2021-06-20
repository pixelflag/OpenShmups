using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private MainCamera mainCamera = default;
    [SerializeField]
    private StageObject[] stages = default;

    private StageObject activeStage;

    private CollisionManager collision;
    private SoundManager sound;
    private ObjectManager objectManager;
    private ControllerInput input;
    private HadManager had;
    private SecretCommand secretCommand;

    private bool isPause;
    private int waitCount = 0;

    private State state;
    private enum State
    {
        Play,
        Dead,
        Clear,
        GameOver,
    }

    public void Initialize()
    {
        mainCamera.Initialize();
        had = HadManager.instance;

        input = ControllerInput.instance;
        collision = new CollisionManager();

        objectManager = ObjectManager.instance;
        objectManager.Initialize();
        objectManager.InitializeEnemyBullet(mainCamera.transform);

        had.ResetAll();
        had.UpdateGauge(player);

        sound = SoundManager.instance;

        secretCommand = new SecretCommand();
        secretCommand.OnSuccess = CommandSccess;

        player.OnFlash = FlashBom;
        player.OnPlayerDead = PlayerDead;
    }

    public void CreateStage(int stageNum)
    {
        FlushStage();
        activeStage = Instantiate(stages[stageNum].gameObject).GetComponent<StageObject>();
        activeStage.Initialize(this);

        collision.SetCollisionMap(activeStage.GetCollisionMap());
    }

    public void GameStart()
    {
        objectManager.Flush();

        player.ResetObject();

        had.ResetAll();
        had.UpdateGauge(player);

        StartStage();
    }

    private void StartStage()
    {
        mainCamera.ResetPosition(0);
        mainCamera.isStopScroll = false;

        activeStage.ReStart(mainCamera.screenFrame.right);
        sound.PlayBgm(activeStage.currentBgm);

        state = State.Play;
    }

    private void ReStart(int startPosition)
    {
        objectManager.Flush();

        mainCamera.ResetPosition(startPosition);
        mainCamera.isStopScroll = false;

        player.Restart();
        had.UpdateGauge(player);

        activeStage.ReStart(mainCamera.screenFrame.right);
        sound.PlayBgm(activeStage.currentBgm);
        
        state = State.Play;
    }

    private void NextStage()
    {
        CreateStage(1);
        StartStage();
    }

    private void FlushStage()
    {
        if (activeStage != null)
            activeStage.StageDestroy();
    }

    public void Excute()
    {
        if (input.GetKeyDown(ControllerButtonType.Start))
        {
            isPause = !isPause;
            sound.PlayOneShotOnChannel(2, SeType.Pause, 0.5f);

            if (isPause)
            {
                sound.PauseBgm();
            }
            else
            {
                sound.ResumeBgm();
                secretCommand.Clear();
            }
        }

        if (isPause)
        {
            secretCommand.Input(input);
            return;
        }

        // -----

        switch (state)
        {
            case State.Play:
                KeyUpdate();
                break;
            case State.Dead:
                waitCount++;
                if (waitCount > 120)
                {
                    waitCount = 0;
                    if (had.p1Left <= 0)
                    {
                        had.ShowGameOver();
                        sound.PlayJingle(BgmType.GameOver, 1);
                        state = State.GameOver;
                    }
                    else
                    {
                        had.MinusLeft();
                        ReStart(activeStage.GetCheckPoint());
                    }
                }
                break;
            case State.Clear:
                KeyUpdate();
                waitCount++;
                if (waitCount > 180)
                {
                    waitCount = 0;
                    NextStage();
                }
                break;
            case State.GameOver:
                waitCount++;
                if (waitCount > 300)
                {
                    waitCount = 0;
                    if (OnGameOver != null) OnGameOver();
                }
                break;
        }

        mainCamera.CameraUpdate();

        objectManager.Excute();
        objectManager.CheckScreenOut(mainCamera.screenFrame);

        collision.ObjectHitCheck(player);
        objectManager.CheckDestroy();

        activeStage.Excute(mainCamera.screenFrame);
    }

    private void KeyUpdate()
    {
        if (input.stickX != 0 || input.stickY != 0)
        {
            player.Move(input.stickX, input.stickY);
        }
        if (input.GetKeyDown(ControllerButtonType.A))
        {
            player.Shot();
        }
        if (input.GetKeyDown(ControllerButtonType.B))
        {
            player.PowerUp();
        }
    }

    private void FlashBom()
    {
        foreach(Enemy en in objectManager.enemys)
        {
            en.FlashDamage();
        }
        foreach (EBullet eb in objectManager.eBullets)
        {
            eb.DestroyObject();
        }
    }

    private void PlayerDead()
    {
        state = State.Dead;
        StopScroll();
        sound.StopBgm();
    }

    // Delegate Events

    public void StartScroll()
    {
        mainCamera.isStopScroll = false;
    }

    public void StopScroll()
    {
        mainCamera.isStopScroll = true;
    }

    public void StageEnd()
    {
        mainCamera.isStopScroll = false;
        sound.StopBgm();
        state = State.Clear;
    }

    private void CommandSccess()
    {
        player.FullPowerUp();
    }

    public delegate void GameOverDelegate();
    public GameOverDelegate OnGameOver;
}

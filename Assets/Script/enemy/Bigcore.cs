using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bigcore : Enemy
{
    private int shield = 30;
    private int moveY;
    private int move = 2;

    [SerializeField]
    private SpriteRenderer coreSprite;
    [SerializeField]
    private Sprite[] coreSprites;

    [SerializeField]
    private SpriteRenderer shieldSprite;
    [SerializeField]
    private Sprite[] shieldSprites;

    private int moveCount = 0;
    private int coreCount = 0;
    private int attackCount = 0;

    private Vector2 initPosition;
    private int hitAreaSize = 8;

    private int upLimit = 80;
    private int downLimit = -64;

    private State state = State.Entry;
    private enum State
    {
        Entry,
        Move,
    }

    private CoreState coreState = CoreState.Invincible;
    private enum CoreState
    {
        Invincible,
        Normal,
        PreDestruct,
        Destruct,
    }

    public override void Initialize(EnemySpawnData data)
    {
        base.Initialize(data);
        hitBox = new Box(position, 16, 24);

        isHardBody = true;
        initPosition = data.position;
        moveY = move;
        disableAutoShot = true;
        score = 10000;
    }

    public override void Execute()
    {
        base.Execute();

        switch (state)
        {
            case State.Entry:
                x -= 1;
                if(x < initPosition.x -80)
                {
                    state = State.Move;
                }
                break;
            case State.Move:
                y += moveY;
                y = y > upLimit ? upLimit : y;
                y = y < downLimit ? downLimit : y;

                moveCount++;
                if (60 < moveCount)
                {
                    moveY = playerPosition.y < y ? -move : move;
                    moveCount = 0;
                }
                break;
        }

        coreCount++;
        switch (coreState)
        {
            case CoreState.Invincible:
                if (coreCount == 200)
                {
                    coreSprite.sprite = coreSprites[1];
                    coreState = CoreState.Normal;
                }
                break;
            case CoreState.Normal:
                if (coreCount == 2500)
                {
                    coreSprite.sprite = coreSprites[0];
                    coreState = CoreState.PreDestruct;
                }
                break;
            case CoreState.PreDestruct:
                if (coreCount == 3000)
                {
                    Dead();
                    coreState = CoreState.Destruct;
                }
                break;
            case CoreState.Destruct:
                break;

        }

        attackCount++;
        if(80 < attackCount)
        {
            ShotMissule((Vector2)position + new Vector2(-0, 24));
            ShotMissule((Vector2)position + new Vector2(-16, 8));
            ShotMissule((Vector2)position + new Vector2(-16, -8));
            ShotMissule((Vector2)position + new Vector2(-0, -24));
            attackCount = 0;
        }
        FitToPixel();
    }

    private void ShotMissule(Vector2 pos)
    {
        EnemySpawnData d = new EnemySpawnData();
        d.enemyName = EnemyName.BigCoreMissle;
        d.position = pos;

        ObjectManager.instance.CreateEnemy(d);
    }


    public override void HitObject(SpriteBase obj)
    {
        if (coreState == CoreState.Invincible) return;

        int dist = (int)Mathf.Abs(obj.position.y - position.y);
        if(dist < hitAreaSize)
        {
            shield--;

            if (shield < 0)
                Dead();
            else if (shield == 12)
                ShieldBroke(5);
            else if (shield == 16)
                ShieldBroke(4);
            else if (shield == 20)
                ShieldBroke(3);
            else if (shield == 24)
                ShieldBroke(2);
            else if (shield == 28)
                ShieldBroke(1);
        }
    }

    private void Dead()
    {
        isDead = true;
        hm.AddScore(score);
        ObjectManager.instance.CreateEffect(EffectName.Explose2, position);
        SoundManager.instance.PlayOneShotOnChannel(2, SeType.ExplosionL, 0.8f);
    }

    private void ShieldBroke(int index)
    {
        SoundManager.instance.PlayOneShotOnChannel(2, SeType.ExplosionS2, 0.5f);

        shieldSprite.sprite = shieldSprites[index];
        hm.AddScore(500);
    }
}

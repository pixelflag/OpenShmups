using UnityEngine;

public class EnemyEvent : StageEvent
{
    public Sprite[] sprites;
    public EnemyName enemyName;
    public bool flipY;
    public CapsulType capsulType;
    public bool backStart;

    private Enemy targetEnemy;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[GetSpriteIndex()];
        transform.localScale = flipY ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);
    }

    private int GetSpriteIndex()
    {
        switch (enemyName)
        {
            case EnemyName.Whirl:
                return 0;
            case EnemyName.Rock:
                return 1;
            case EnemyName.Butterfly:
                if (!isRedEnemy)
                    return 2;
                else
                    return 3;
            case EnemyName.Sunfish:
                if (!isRedEnemy)
                    return 4;
                else
                    return 5;
            case EnemyName.Hopper:
                if (!isRedEnemy)
                    return 6;
                else
                    return 7;
            case EnemyName.Walker:
                if (!isRedEnemy)
                    return 8;
                else
                    return 9;
            case EnemyName.Canon:
                if (!isRedEnemy)
                    return 10;
                else
                    return 11;
            case EnemyName.Hexagon:
                return 12;
            case EnemyName.Base:
                return 13;
            case EnemyName.BigCore:
                return 14;
        }
        return 0;
    }

    private bool isRedEnemy
    {
        get
        {
            switch (capsulType)
            {
                case CapsulType.Power:
                case CapsulType.Flash:
                    return true;
            }
            return false;
        }
    }


    public override void Execute(ScreenFrame frame)
    {
        switch (state)
        {
            case State.Initialize:
                state = State.Ready;
                break;
            case State.Ready:
                if (backStart)
                {
                    if (x < frame.left - 32)
                    {
                        Spawn();
                        state = State.Action;
                    }
                }
                else
                {
                    if (x < frame.right + 32)
                    {
                        Spawn();
                        state = State.Action;
                    }
                }
                break;
            case State.Action:
                if(targetEnemy.isDead)
                {
                    state = State.Done;
                }
                break;
            case State.Done:
                break;
        }
    }

    private void Spawn()
    {
        EnemySpawnData d = new EnemySpawnData();
        d.enemyName = enemyName;
        d.flipY = flipY;
        d.capsulType = capsulType;
        d.backStart = backStart;
        d.position = position;

        targetEnemy = objectFactory.CreateEnemy(d);
    }
}

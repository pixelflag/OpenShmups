using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunfish : Enemy
{
    public override void Initialize(EnemySpawnData data)
    {
        base.Initialize(data);
        om = ObjectManager.instance;
        shotWait = 500;
        shotCount = shotWait;
    }

    public override void Execute()
    {
        if(Mathf.Abs(playerPosition.y - y) < 2)
        {
            x -= 2;
            SetSprite(0);
        }
        else if(playerPosition.y < y)
        {
            x -= 0.5f;
            y -= 0.5f;
            SetSprite(1);
        }
        else
        {
            x -= 0.5f;
            y += 0.5f;
            SetSprite(2);
        }

        FitToPixel();

        base.Execute();
    }
}

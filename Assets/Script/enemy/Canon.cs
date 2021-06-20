using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : Enemy
{
    public override void Initialize(EnemySpawnData data)
    {
        base.Initialize(data);
        explosionSeType = SeType.ExplosionS2;
    }

    public override void Execute()
    {
        float rad = Calculate.PositionToRadian(position, playerPosition);
        float a = Mathf.PI / 8;
        if( a*1 < rad && rad < a * 3)
        {
            SetSprite(1);
            render.flipX = true;
        }
        else if( a*3 < rad && rad < a * 5)
        {
            SetSprite(2);
            render.flipX = true;
        }
        else if ( a*5 < rad && rad < a * 7)
        {
            SetSprite(1);
            render.flipX = false;
        }
        else if ( a*7 < rad && rad < a * 9)
        {
            SetSprite(0);
            render.flipX = false;
        }
        else if (a * 9 < rad && rad < a * 11)
        {
            SetSprite(1);
            render.flipX = false;
        }
        else if ( a*11 < rad && rad < a * 13)
        {
            SetSprite(2);
            render.flipX = true;
        }
        else if ( a*13 < rad && rad < a * 15)
        {
            SetSprite(1);
            render.flipX = true;
        }
        else
        {
            SetSprite(0);
            render.flipX = true;
        }

        base.Execute();
        FitToPixel();
    }
}

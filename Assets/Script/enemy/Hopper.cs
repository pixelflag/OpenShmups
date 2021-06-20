using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hopper : Enemy
{
    private AnimCounter anim;

    private int groundLine = 0;
    private Vector2 vector = new Vector2(0,0);
    private float grabity = 0.2f;
    private float jumpPower = 4;
    private float speed = 2;
    private int hopCount = 0;

    public override void Initialize(EnemySpawnData data)
    {
        base.Initialize(data);

        groundLine = (int)data.position.y;
        anim = new AnimCounter(sprites.Length, 8);
        shotWait = 180;

        if (flipY)
            grabity = -grabity;
    }

    public override void Execute()
    {
        base.Execute();

        x += vector.x;
        y += vector.y;

        vector.y -= grabity;

        if (flipY)
        {
            if (groundLine < y)
            {
                if (hopCount == 2)
                    vector.x = speed+1;
                else
                    vector.x = -speed;
                vector.y = -jumpPower;
                hopCount++;
            }
        }
        else
        {
            if (groundLine > y)
            {
                if (hopCount == 2)
                    vector.x = speed + 1;
                else
                    vector.x = -speed;
                vector.y = jumpPower;
                hopCount++;
            }
        }

        anim.Execute();
        SetSprite(anim.frame);

        FitToPixel();
    }
}

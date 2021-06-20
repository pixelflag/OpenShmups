using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : Enemy
{
    private AnimCounter anim;
    private int count;

    private float amp = 1.2f;
    private float speedX = 1.8f;
    private float speedY = 12;

    public override void Initialize(EnemySpawnData data)
    {
        base.Initialize(data);
        anim = new AnimCounter(sprites.Length, 8);
        shotWait = 500;
        shotCount = shotWait;
    }

    public override void Execute()
    {
        base.Execute();
        count++;

        x -= speedX;
        y += Mathf.Cos(count/speedY) * amp;

        anim.Execute();
        SetSprite(anim.frame);

        FitToPixel();
    }
}

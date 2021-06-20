using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Enemy, IGroundWalker
{
    private int count;
    private int escapeCount =  360;

    private AnimCounter anim;

    private bool isGround = false;
    private bool isWall = false;

    private State state = State.Walk;

    public int groundOffset { get; private set; }
    public float speedX { get; private set; }
    public float speedY { get; private set; }

    private float walkTarget;

    private enum State
    {
        Walk,
        Fire,
        Escape,
    }

    public override void Initialize(EnemySpawnData data)
    {
        base.Initialize(data);

        anim = new AnimCounter(2, 8);
        speedX = 2;
        groundOffset = 16;
        shotWait = 180;
        disableAutoShot = true;

        walkTarget = playerPosition.x + 128;
    }

    public override void Execute()
    {
        base.Execute();
        anim.Execute();

        escapeCount--;
        count++;

        switch (state)
        {
            case State.Walk:
                x += speedX;
                render.flipX = speedX > 0;
                SetSprite(anim.frame);
                moveY();

                if (Mathf.Abs(walkTarget - x) < 5)
                {
                    count = 0;
                    state = State.Fire;
                }
                break;
            case State.Fire:
                if(count == 40)
                {
                    SetSprite(2);
                    render.flipX = playerPosition.x > x;
                }

                if (count == 60)
                {
                    Shot(position);
                }

                if (count > 90)
                {
                    count = 0;
                    if(escapeCount < 0)
                    {
                        state = State.Escape;
                        speedX = -2;
                    }
                    else
                    {
                        state = State.Walk;
                        walkTarget = playerPosition.x + 128;
                    }
                }
                break;
            case State.Escape:
                x += speedX;
                render.flipX = speedX > 0;
                SetSprite(anim.frame);
                moveY();
                break;
        }
        FitToPixel();
    }

    private void moveY()
    {
        if (isWall)
        {
            speedY = flipY ? -2 : 2;
            y += speedY;
        }
        else
        {
            if (!isGround)
            {
                speedY = flipY ? 2 : -2;
                y += speedY;
            }
        }
    }

    public void CheckGround(bool isGround, bool isWall)
    {
        this.isGround = isGround;
        this.isWall = isWall;
    }
}

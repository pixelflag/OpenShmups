using UnityEngine;

public class Hexagon : Enemy
{
    private AnimCounter anim;
    private int moveX = 1;

    private State state = State.Up;
    private enum State
    {
        Up,
        Straight,
    }

    public override void Initialize(EnemySpawnData data)
    {
        base.Initialize(data);
        anim = new AnimCounter(sprites.Length, 8);
    }

    public override void Execute()
    {
        base.Execute();
        anim.Execute();
        SetSprite(anim.frame);

        switch (state)
        {
            case State.Up:
                if(flipY)
                {
                    y -= 2;
                    if (y < playerPosition.y)
                    {
                        state = State.Straight;
                        moveX = playerPosition.x < x ? -2 : 2;
                    }
                }
                else
                {
                    y += 2;
                    if (y > playerPosition.y)
                    {
                        state = State.Straight;
                        moveX = playerPosition.x < x ? -2 : 2;
                    }
                }
                break;
            case State.Straight:
                x += moveX;
                break;
        }
        FitToPixel();
    }
}


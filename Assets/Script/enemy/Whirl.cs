using UnityEngine;

public class Whirl : Enemy
{
    private AnimCounter anim;
    private int moveY = 1;

    private State state = State.Straight;
    private enum State
    {
        Straight,
        Turn,
        Return,
    }

    private int turnPoint;
    public void SetTurnPoint(int turnPoint)
    {
        this.turnPoint = turnPoint;
    }

    public override void Initialize(EnemySpawnData data)
    {
        base.Initialize(data);
        anim = new AnimCounter(sprites.Length, 8);
        disableAutoShot = true;
    }

    public override void Execute()
    {
        base.Execute();

        switch (state)
        {
            case State.Straight:
                x -= 2;
                if (x < turnPoint)
                {
                    state = State.Turn;
                    moveY = y < 16 ? 2 : -2;
                }
                break;
            case State.Turn:
                x += 1.5f;
                y += moveY;
                if (moveY < 0)
                {
                    if (playerPosition.y > y)
                    {
                        state = State.Return;
                    }
                }
                else
                {
                    if (playerPosition.y < y)
                    {
                        state = State.Return;
                    }
                }
                break;
            case State.Return:
                x += 4;
                break;
        }

        anim.Execute();
        SetSprite(anim.frame);

        FitToPixel();
    }
}

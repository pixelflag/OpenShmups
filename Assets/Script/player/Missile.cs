using UnityEngine;
using UnityEngine.Tilemaps;

public class Missile : PoolSprite, IGroundWalker
{
    private bool isGround = false;
    public int groundOffset { get; private set; }
    public float speedX { get; private set; }
    public float speedY { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
        hitBox = new Box(position, 4, 4);

        groundOffset = 8;
        speedX = 1;
        speedY = -1;
    }

    public override void Execute()
    {
        base.Execute();
        if (isDead) return;

        if(isGround)
        {
            render.sprite = sprites[1];
            x += 3;
        }
        else
        {
            render.sprite = sprites[0];
            x += 1;
            y -= 2;
        }

        FitToPixel();
    }

    public void HitObject(SpriteBase obj)
    {
        DestroyObject();
    }

    public void CheckGround(bool isGround, bool isWall)
    {
        this.isGround = isGround;
        if (isGround && isWall)
        {
            DestroyObject();
        }
    }
}

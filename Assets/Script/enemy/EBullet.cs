using UnityEngine;

public class EBullet : PoolSprite
{
    private Vector3 speed;
    private AnimCounter anim;

    public override void Initialize()
    {
        base.Initialize();
        hitBox = new Box(position, 1, 1);
        anim = new AnimCounter(2, 2);
    }

    public override void Execute()
    {
        if (isDead) return;

        x += speed.x;
        y += speed.y;

        anim.Execute();
        render.sprite = sprites[anim.frame];
    }

    public void SetSpeed(Vector3 speed)
    {
        this.speed = speed;
    }

    public void HitObject()
    {
        DestroyObject();
    }
}

using UnityEngine;

public class Explosion : SpriteBase
{
    [SerializeField]
    private int animWait = 4;

    private AnimCounter anim;

    public override void Initialize()
    {
        base.Initialize();
        anim = new AnimCounter(sprites.Length, animWait);
    }

    public override void Execute()
    {
        base.Execute();
        anim.Execute();

        if (anim.isEnd)
            isDead = true;
        else
            render.sprite = sprites[anim.frame];

        FitToPixel();
    }
}

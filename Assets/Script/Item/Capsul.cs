using UnityEngine;

public class Capsul : Item
{
    private AnimCounter anim;

    public override void Initialize(ItemName itemName)
    {
        base.Initialize(itemName);
        anim = new AnimCounter(sprites.Length, 8);
    }

    public override void Execute()
    {
        if (isDead) return;
        anim.Execute();
        render.sprite = sprites[anim.frame];

        FitToPixel();
    }
}

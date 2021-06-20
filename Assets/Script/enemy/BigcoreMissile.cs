using UnityEngine;

public class BigcoreMissile : Enemy
{
    public override void Initialize(EnemySpawnData data)
    {
        base.Initialize(data);
        hitBox = new Box(position, 8, 1);
        disableAutoShot = true;
        isThroughBullet = true;
    }

    public override void Execute()
    {
        base.Execute();
        x -= 3;
        FitToPixel();
    }

    public override void HitObject(SpriteBase obj)
    {
        if(obj.GetComponent<Player>() != null)
        {
            isDead = true;
        }
    }
}

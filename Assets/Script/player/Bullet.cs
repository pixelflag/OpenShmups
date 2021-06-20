using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolSprite
{
    private BulletType type;
    private bool evenShot = false;

    public override void Initialize()
    {
        base.Initialize();
        hitBox = new Box(position, 2, 2);
    }

    public void Spawn(Vector3 position, BulletType type, bool evenShot)
    {
        base.Spawn(position+new Vector3(0,-2,0));
        this.type = type;
        this.evenShot = evenShot;

        switch (type)
        {
            case BulletType.Normal:
                hitBox.extendsX = 10;
                hitBox.extendsY = 4;
                render.sprite = sprites[0];
                break;
            case BulletType.Double:
                hitBox.extendsX = 6;
                hitBox.extendsY = 6;
                if (evenShot)
                    render.sprite = sprites[0];
                else
                    render.sprite = sprites[1];
                break;
            case BulletType.Laser:
                hitBox.extendsX = 16;
                hitBox.extendsY = 4;
                render.sprite = sprites[2];
                break;
        }
    }

    public override void Execute()
    {
        base.Execute();
        if (isDead) return;

        switch (type)
        {
            case BulletType.Normal:
                x += 10;
                break;
            case BulletType.Double:
                if (evenShot)
                {
                    x += 10;
                    y += 0;
                }
                else
                {
                    x += 6;
                    y += 6;
                }
                break;
            case BulletType.Laser:
                x += 16;
                break;
        }

        FitToPixel();
    }

    public void HitEnemy(Enemy enemy)
    {
        switch (type)
        {
            case BulletType.Normal:
            case BulletType.Double:
                DestroyObject();
                break;
            case BulletType.Laser:
                if(enemy.isHardBody)
                {
                    DestroyObject();
                }
                break;
        }
    }

    public void HitMap()
    {
        DestroyObject();
    }
}

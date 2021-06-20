using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : SpriteBase
{
    [SerializeField]
    private GameObject bulletPrefab = default;
    [SerializeField]
    private GameObject missilePrefab = default;

    public Bullet[] bullet { get; private set; }
    public Missile[] missile { get; private set; }

    private int maxBullet = 2;
    private int maxMissile = 1;

    public bool isPlayer { get; set; }

    private AnimCounter anim;

    public void Initialize(bool isPlayer)
    {
        this.isPlayer = isPlayer;
        base.Initialize();
        anim = new AnimCounter(sprites.Length, 8);

        bullet = new Bullet[maxBullet];
        for (int i = 0; i < bullet.Length; i++)
        {
            bullet[i] = Instantiate(bulletPrefab).GetComponent<Bullet>();
            bullet[i].Initialize();
            bullet[i].DestroyObject();
        }

        missile = new Missile[maxMissile];
        for (int i = 0; i < missile.Length; i++)
        {
            missile[i] = Instantiate(missilePrefab).GetComponent<Missile>();
            missile[i].Initialize();
            missile[i].DestroyObject();
        }

        if(isPlayer)
        {
            render.sprite = null;
        }
    }

    public override void ResetObject()
    {
        base.ResetObject();

        foreach (Bullet b in bullet)
        {
            b.DestroyObject();
        }
        foreach (Missile m in missile)
        {
            m.DestroyObject();
        }
    }

    public override void Execute()
    {
        base.Execute();

        if (isDead) return;

        foreach (Bullet b in bullet)
        {
            b.Execute();
        }
        foreach (Missile m in missile)
        {
            m.Execute();
        }

        if (!isPlayer)
        {
            anim.Execute();
            render.sprite = sprites[anim.frame];
        }

        FitToPixel();
    }

    public void EnableOption()
    {
        isDead = false;
    }

    public void DesableOption()
    {
        isDead = true;
        render.sprite = null;
    }

    public bool ShotBullet(BulletType bulletType)
    {
        if (isDead) return false;

        for (int i = 0; i < bullet.Length; i++)
        {
            Bullet b = bullet[i];

            switch (bulletType)
            {
                case BulletType.Normal:
                case BulletType.Laser:
                    if (b.isDead == true)
                    {
                        b.Spawn(position, bulletType, false);
                        b.z = 0;
                        return true;
                    }
                    break;
                case BulletType.Double:
                    if(i % 2 == 0)
                    {
                        if (b.isDead == true)
                        {
                            b.Spawn(position, bulletType, true);
                            b.z = 0;
                            return true;
                        }
                    }
                    else
                    {
                        if (b.isDead == true)
                        {
                            b.Spawn(position, bulletType, false);
                            b.z = 0;
                            return true;
                        }
                    }
                    break;
            }
        }

        return false;
    }

    public bool ShotMissile()
    {
        if (isDead) return false;

        foreach (Missile m in missile)
        {
            if (m.isDead == true)
            {
                m.Spawn(position);
                m.z = 0;
                return true;
            }
        }
        return false;
    }
}

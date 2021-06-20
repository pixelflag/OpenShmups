using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Enemy
{
    private int life = 5;
    private int spawnWait = 10;
    private int spawnNum = 4;

    private State state = State.Wait1;
    private enum State
    {
        Wait1,
        Spawn1,
        Wait2,
        Spawn2,
        End,
    }

    private int count = 0;

    public override void Initialize(EnemySpawnData data)
    {
        base.Initialize(data);

        hitBox = new Box(position, 16, 8);
        isHardBody = true;
        score = 1000;
        disableAutoShot = true;
    }

    public override void Execute()
    {
        base.Execute();
        count++;

        switch (state)
        {
            case State.Wait1:
                if(count > 80)
                {
                    state = State.Spawn1;
                    count = 0;
                }
                break;
            case State.Spawn1:
                if (count % spawnWait == 0) Spawn();
                if (count > spawnWait * spawnNum)
                {
                    state = State.Wait2;
                    count = 0;
                }
                break;
            case State.Wait2:
                if (count > 80)
                {
                    state = State.Spawn2;
                    count = 0;
                }
                break;
            case State.Spawn2:
                if (count % spawnWait == 0) Spawn();
                if (count > spawnWait * spawnNum)
                {
                    state = State.End;
                    count = 0;
                }
                break;
            case State.End:
                break;
        }
        FitToPixel();
    }

    private void Spawn()
    {
        EnemySpawnData d = new EnemySpawnData();
        d.enemyName = EnemyName.Hexagon;
        d.flipY = flipY;
        d.position = position;

        Enemy en = om.CreateEnemy(d);
        en.position = position;
    }

    public override void HitObject(SpriteBase obj)
    {
        SoundManager.instance.PlayOneShotOnChannel(2, SeType.Damage, 0.5f);

        life--;
        if(life <= 0)
        {
            isDead = true;
            hm.AddScore(score);
            ObjectManager.instance.CreateEffect(EffectName.Explose2, position);
            SoundManager.instance.PlayOneShotOnChannel(2, SeType.ExplosionM, 0.5f);
        }
        else if(life <= 2)
        {
            render.sprite = Psprites[0];
        }
    }
}

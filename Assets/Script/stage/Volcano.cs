using UnityEngine;

public class Volcano : PixelObject
{
    public bool isEruption;
    private int count;
    private int step = 10;
    private Vector2 maxPower = new Vector2(3,8);

    public override void Initialize()
    {
        base.Initialize();
        Destroy(GetComponent<SpriteRenderer>());
    }

    public override void ResetObject()
    {
        isEruption = false;
        count = 0;
    }

    public override void Execute()
    {
        if(isEruption)
        {
            count++;
            if (count % step == 0)
            {
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        EnemySpawnData d = new EnemySpawnData();
        d.enemyName = EnemyName.Rock;
        d.position = position;

        Rock rk = ObjectManager.instance.CreateEnemy(d).GetComponent<Rock>();
        float xx = Random.Range(-maxPower.x, maxPower.x);
        float yy = Random.Range(4,maxPower.y);
        rk.AddForce(new Vector2(xx,yy));
    }
}

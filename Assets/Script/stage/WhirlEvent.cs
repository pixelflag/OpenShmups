using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlEvent : StageEvent
{
    [SerializeField]
    private EnemyName enemyName;

    private int createCount = 4;
    private List<Whirl> enemys;
    private Vector2 lastPosition;

    private int count;

    public override void ResetObject()
    {
        base.ResetObject();
        count = 0;
        enemys.Clear();
    }

    public override void Execute(ScreenFrame frame)
    {
        switch (state)
        {
            case State.Initialize:
                enemys = new List<Whirl>();
                state = State.Ready;
                break;
            case State.Ready:
                if (x < frame.right)
                {
                    count++;
                    if (count % 10 == 0)
                    {
                        enemys.Add(Spawn(new Vector2(32, 0)));
                    }
                    if (enemys.Count == createCount)
                    {
                        state = State.Action;
                    }
                }
                break;
            case State.Action:
                int deadCount = 0;
                foreach(Enemy en in enemys)
                {
                    if (en == null)
                        deadCount++;
                    else
                        lastPosition = en.position;
                }
                if (deadCount == createCount)
                {
                    objectFactory.CreateItem(ItemName.PowerCapsul, lastPosition);
                    state = State.Done;
                }
                break;
            case State.Done:
                break;
        }
    }

    private Whirl Spawn(Vector2 offset)
    {
        EnemySpawnData d = new EnemySpawnData();
        d.enemyName = enemyName;
        d.position = (Vector2)position + offset;

        Whirl wl = objectFactory.CreateEnemy(d).GetComponent<Whirl>();
        wl.SetTurnPoint((int)x - 112);

        return wl;
    }
}

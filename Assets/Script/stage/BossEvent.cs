using UnityEngine;

public class BossEvent : StageEvent
{
    private Enemy target;

    public override void Execute(ScreenFrame frame)
    {
        switch (state)
        {
            case State.Initialize:
                state = State.Ready;
                break;
            case State.Ready:
                if (x < frame.right)
                {
                    if (OnEnter != null) OnEnter();
                    Spawn();
                    state = State.Action;
                }
                break;
            case State.Action:
                if (target.isDead)
                {
                    if (OnEnd != null) OnEnd();
                    state = State.Done;
                }
                break;
            case State.Done:
                break;
        }
    }

    private void Spawn()
    {
        EnemySpawnData d = new EnemySpawnData();
        d.enemyName = EnemyName.BigCore;
        d.position = position + new Vector3(32,0,0);

        target = objectFactory.CreateEnemy(d);
    }
}

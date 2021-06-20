using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoEvent : StageEvent
{
    [SerializeField]
    private Volcano volcano1;
    [SerializeField]
    private Volcano volcano2;
    [SerializeField]
    private int stopCount = 600;

    private int count;

    public override void ResetObject()
    {
        base.ResetObject();
        volcano1.ResetObject();
        volcano2.ResetObject();
    }

    public override void Execute(ScreenFrame frame)
    {
        switch (state)
        {
            case State.Initialize:
                state = State.Ready;
                volcano1.Initialize();
                volcano2.Initialize();
                break;
            case State.Ready:
                if (x < frame.right)
                {
                    volcano1.isEruption = true;
                    volcano2.isEruption = true;

                    if (OnEnter != null) OnEnter();
                    state = State.Action;
                    count = stopCount;
                }
                break;
            case State.Action:
                count--;

                volcano1.Execute();
                volcano2.Execute();

                if (count < 0)
                {
                    volcano1.isEruption = false;
                    volcano2.isEruption = false;

                    if (OnEnd != null) OnEnd();
                    state = State.Done;
                }
                break;
            case State.Done:
                break;
        }
    }
}
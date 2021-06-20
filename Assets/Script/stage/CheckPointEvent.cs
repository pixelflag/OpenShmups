using UnityEngine;

public class CheckPointEvent : StageEvent
{
    public override void Execute(ScreenFrame frame)
    {
        switch (state)
        {
            case State.Initialize:
                state = State.Ready;
                break;
            case State.Ready:
                if (x < frame.right)
                    state = State.Action;
                break;
            case State.Action:
                int halfScreenWidth = 128;
                stage.UpdateCheckPoint((int)position.x - halfScreenWidth);
                state = State.Done;
                break;
            case State.Done:
                break;
        }
    }
}

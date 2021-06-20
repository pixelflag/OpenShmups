using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvent : StageEvent
{
    public bool overrideCurrentBgm;
    public BgmType bgmType;
    public SoundEventType type;

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
                    switch (type)
                    {
                        case SoundEventType.Play:
                            sound.PlayBgm(bgmType);
                            if(overrideCurrentBgm)
                                stage.currentBgm = bgmType;
                            break;
                        case SoundEventType.FadeIn:
                            sound.BGMFadeIn(bgmType);
                            if (overrideCurrentBgm)
                                stage.currentBgm = bgmType;
                            break;
                        case SoundEventType.FadeInOut:
                            sound.BGMFadeOutAndPlayIn(bgmType);
                            if (overrideCurrentBgm)
                                stage.currentBgm = bgmType;
                            break;
                        case SoundEventType.FadeOut:
                            sound.BGMFadeOutAndStop();
                            break;
                        case SoundEventType.Stop:
                            sound.StopBgm();
                            break;
                    }
                    state = State.Done;
                }
                break;
            case State.Action:
                break;
            case State.Done:
                break;
        }
    }

}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create SoundResource", fileName = "SoundResource")]
public class SoundResource : ScriptableObject
{
    public AudioClip[] SE;
    public AudioClip[] BGM;

    public AudioClip GetEffect(SeType type)
    {
        switch(type)
        {
            case SeType.Shot: return SE[0];
            case SeType.Lazer: return SE[1];
            case SeType.PowerCapsul: return SE[2];
            case SeType.FlashCapsul: return SE[3];
            case SeType.PowerUp: return SE[4];
            case SeType.Damage: return SE[5];
            case SeType.ExplosionS1: return SE[6];
            case SeType.ExplosionS2: return SE[7];
            case SeType.ExplosionM: return SE[8];
            case SeType.ExplosionL: return SE[9];
            case SeType.Miss: return SE[10];
            case SeType.Bonus: return SE[11];
            case SeType.Pause: return SE[12];
        }
        return null;
    }

    public AudioClip GetBgm(BgmType type)
    {
        switch (type)
        {
            case BgmType.Space: return BGM[0];
            case BgmType.Land: return BGM[1];
            case BgmType.Boss: return BGM[2];
            case BgmType.GameOver: return BGM[3];
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGauge : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites = default;

    [SerializeField]
    private GameObject[] buttons = default;

    public void UpdateGauge(Player player)
    {
        for (int i=0; i < buttons.Length; i++ )
        {
            bool active = i+1 == player.capsulNum;
            int index = GetIndex(i, active, player);
            buttons[i].GetComponent<SpriteRenderer>().sprite = sprites[index];

        }
    }

    private int GetIndex(int index, bool active, Player player)
    {
        switch (index)
        {
            // SpeedUp
            case 0:
                if(player.isSpeedMax)
                    return active ? 0 : 1;
                else
                    return active ? 2 : 3;
            // Missule
            case 1:
                if (player.useMissile)
                    return active ? 0 : 1;
                else
                    return active ? 4 : 5;
            // Double
            case 2:
                if (player.bulletType == BulletType.Double)
                    return active ? 0 : 1;
                else
                    return active ? 6 : 7;
            // Laser
            case 3:
                if (player.bulletType == BulletType.Laser)
                    return active ? 0 : 1;
                else
                    return active ? 8 : 9;
            // Option
            case 4:
                if (player.isOptionMax)
                    return active ? 0 : 1;
                else
                    return active ? 10 : 11;
            // Shied
            case 5:
                if (player.useShield)
                    return active ? 0 : 1;
                else
                    return active ? 12 : 13;
            default:
                return active ? 0 : 1;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : PixelObject
{
    [SerializeField]
    private Sprite[] sprites = default;

    [SerializeField]
    private GameObject shield1 = default;
    [SerializeField]
    private GameObject shield2 = default;

    private SpriteRenderer render1;
    private SpriteRenderer render2;

    private int maxShield = 5;
    private int shieldLife = 0;

    public bool active { get; private set; }
    public bool isDamaged;

    private AnimCounter anim;

    public override void Initialize()
    {
        base.Initialize();
        render1 = shield1.GetComponent<SpriteRenderer>();
        render2 = shield2.GetComponent<SpriteRenderer>();
        anim = new AnimCounter(2,4);
    }

    public override void Execute()
    {
        if (!active) return;
        anim.Execute();
        render1.sprite = sprites[anim.frame + (isDamaged ? 2 : 0)];
        render2.sprite = sprites[anim.frame + (isDamaged ? 2 : 0)];
    }

    public void Damage()
    {
        shieldLife--;
        if (shieldLife == 1)
        {
            isDamaged = true;
        }

        if (shieldLife <= 0)
        {
            Hide();
        }
    }

    public void AddSield()
    {
        active = true;
        shieldLife = maxShield;

        render1.sprite = sprites[0];
        render2.sprite = sprites[0];
    }

    public void Hide()
    {
        active = false;
        isDamaged = false;
        render1.sprite = null;
        render2.sprite = null;
    }
}

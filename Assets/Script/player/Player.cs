using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : SpriteBase
{
    [SerializeField]
    private GameObject optionPrefab = default;

    [SerializeField]
    private Shield shield = default;

    public bool unDeadMode;

    // ----------

    private int maxOption = 2;
    private int maxSpeed = 4;

    private int optionNum = 0;
    private int speedNum = 0;

    public BulletType bulletType { get; private set; }
    public bool isSpeedMax { get { return maxSpeed <= speedNum; } }
    public bool isOptionMax { get { return maxOption <= optionNum; } }
    public bool useMissile { get; private set; }
    public bool useShield { get { return shield.active; } }

    public int capsulNum { get; private set; }
    private int maxCapsul = 6;

    public Attacker[] attaker { get; private set; }
    private Vector2[] moveLog;

    private float[] speedTable = { 1, 1.5f, 2, 2.5f, 3, 4 };

    private float tilt = 0;
    private float maxTilt = 5;

    public override void Initialize()
    {
        base.Initialize();
        hitBox = new Box(position, 6, 1);

        attaker = new Attacker[maxOption+1];
        for (int i = 0; i < attaker.Length; i++)
        {
            attaker[i] = Instantiate(optionPrefab,this.transform.parent).GetComponent<Attacker>();

            if(i == 0)
            {
                attaker[i].Initialize(true);
            }
            else
            {
                attaker[i].Initialize(false);
                attaker[i].DesableOption();
            }
        }

        shield.Initialize();
        shield.Hide();

        moveLog = new Vector2[64];

        for (int i = 0; i < moveLog.Length; i++)
        {
            moveLog[i] = new Vector2();
        }
    }

    public override void ResetObject()
    {
        Restart();
        capsulNum = 0;
    }

    public void Restart()
    {
        base.ResetObject();

        bulletType = BulletType.Normal;
        useMissile = false;
        shield.Hide();

        optionNum = 0;
        speedNum = 0;

        capsulNum = capsulNum > 0 ? 1:0;

        render.sprite = sprites[0];
        isDead = false;

        for (int i = 0; i < attaker.Length; i++)
        {
            if (i == 0)
            {
                attaker[i].ResetObject();
            }
            else
            {
                attaker[i].ResetObject();
                attaker[i].DesableOption();
            }
        }

        localPosition = new Vector3(-64, 8, 0);

        for (int i = 0; i < moveLog.Length; i++)
        {
            moveLog[i] = localPosition;
        }
    }

    public override void Execute()
    {
        base.Execute();

        for (int i = 0; i < attaker.Length; i++)
        {
            attaker[i].Execute();
            attaker[i].localPosition = moveLog[12 * i];
        }

        shield.Execute();

        if (!isDead)
        {
            if (tilt >= maxTilt)
                render.sprite = sprites[2];
            else if (tilt <= -maxTilt)
                render.sprite = sprites[1];
            else
                render.sprite = sprites[0];

            tilt *= 0.98f;
        }
        FitToPixel();
    }

    public void Move(float moveX, float moveY)
    {
        x += moveX * speedTable[speedNum];
        y += moveY * speedTable[speedNum];

        for (int i = moveLog.Length - 1; 0 < i; i--)
        {
            moveLog[i] = moveLog[i - 1];
        }
        moveLog[0] = localPosition;

        // tilt -----
        if (moveY == 1)
            tilt = tilt + 1 >  maxTilt ?  maxTilt : tilt + 1;
        else if (moveY == -1)
            tilt = tilt - 1 < -maxTilt ? -maxTilt : tilt - 1;
    }

    public void Shot()
    {
        foreach (Attacker at in attaker)
        {
            if (at.ShotBullet(bulletType))
            {
                switch (bulletType)
                {
                    case BulletType.Normal:
                    case BulletType.Double:
                        SoundManager.instance.PlayOneShotOnChannel(1, SeType.Shot, 0.2f);
                        break;
                    case BulletType.Laser:
                        SoundManager.instance.PlayOneShotOnChannel(1, SeType.Lazer, 0.2f);
                        break;
                }
            }
            if (useMissile)
                at.ShotMissile();
        }
    }

    public void PowerUp()
    {
        if (isDead) return;

        switch (capsulNum)
        {
            case 0:
                // 0 capsul
                break;
            case 1:
                // Speed UP
                if(!isSpeedMax)
                {
                    speedNum++;
                    SoundManager.instance.PlayOneShotOnChannel(2, SeType.PowerUp, 0.5f);
                    capsulNum = 0;
                }
                break;
            case 2:
                // Missile
                if(!useMissile)
                {
                    useMissile = true;
                    SoundManager.instance.PlayOneShotOnChannel(2, SeType.PowerUp, 0.5f);
                    capsulNum = 0;
                }
                break;
            case 3:
                // Double Shot
                if (bulletType != BulletType.Double)
                {
                    bulletType = BulletType.Double;
                    SoundManager.instance.PlayOneShotOnChannel(2, SeType.PowerUp, 0.5f);
                    capsulNum = 0;
                }
                break;
            case 4:
                // Laser
                if (bulletType != BulletType.Laser)
                {
                    bulletType = BulletType.Laser;
                    SoundManager.instance.PlayOneShotOnChannel(2, SeType.PowerUp, 0.5f);
                    capsulNum = 0;
                }
                break;
            case 5:
                // Opstion
                if(!isOptionMax)
                {
                    attaker[optionNum+1].EnableOption();
                    optionNum++;
                    SoundManager.instance.PlayOneShotOnChannel(2, SeType.PowerUp, 0.5f);
                    capsulNum = 0;
                }
                break;
            case 6:
                // Shield
                if (!useShield)
                {
                    shield.AddSield();
                    SoundManager.instance.PlayOneShotOnChannel(2, SeType.PowerUp, 0.5f);
                    capsulNum = 0;
                }
                break;
            default:
                throw new System.Exception("Illegal numbers : " + capsulNum);

        }
        HadManager.instance.UpdateGauge(this);
    }

    public void FullPowerUp()
    {
        if (isDead) return;

        speedNum = 1;
        useMissile = true;

        while(!isOptionMax)
        {
            attaker[optionNum + 1].EnableOption();
            optionNum++;
        }
        shield.AddSield();

        HadManager.instance.UpdateGauge(this);
    }

    public void HitObject(SpriteBase obj)
    {
        if (useShield)
            shield.Damage();
        else
            playerDead();
    }

    public void HitItem(ItemName itemName)
    {
        HadManager hm = HadManager.instance;

        switch (itemName)
        {
            case ItemName.PowerCapsul:
                capsulNum = maxCapsul < capsulNum + 1 ? 1 : capsulNum + 1;

                hm.UpdateGauge(this);
                hm.AddScore(500);
                SoundManager.instance.PlayOneShotOnChannel(2, SeType.PowerCapsul, 0.5f);

                break;
            case ItemName.FlashCapsul:
                if (OnFlash != null) OnFlash();
                SoundManager.instance.PlayOneShotOnChannel(2, SeType.FlashCapsul, 0.5f);
                break;
            case ItemName.Score5000:
                hm.AddScore(5000);
                SoundManager.instance.PlayOneShotOnChannel(2, SeType.Bonus, 0.5f);
                break;
            case ItemName.OneUp:
                hm.PlusLeft();
                SoundManager.instance.PlayOneShotOnChannel(2, SeType.Bonus, 0.5f);
                break;
        }
    }

    public void HitMap()
    {
        playerDead();
    }

    private void playerDead()
    {
        if (unDeadMode) return;

        isDead = true;
        render.sprite = null;
        SoundManager.instance.PlayOneShotOnChannel(3, SeType.Miss, 0.8f);
        ObjectManager.instance.CreateEffect(EffectName.Explose3, position);
        if (OnPlayerDead != null) OnPlayerDead();
    }

    public delegate void FlashCapsulDelegate();
    public FlashCapsulDelegate OnFlash;

    public delegate void PlayerDeadDelegate();
    public PlayerDeadDelegate OnPlayerDead;
}
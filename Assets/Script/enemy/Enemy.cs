using UnityEngine;

public class Enemy : SpriteBase
{
    public EnemyName enemyName { get; private set; }

    protected ObjectManager om;
    protected HadManager hm;

    protected Vector3 playerPosition
    {
        get { return om.GetPlayer().position; }
    }

    protected bool disableAutoShot = false;
    protected int shotWait = 180;
    protected int shotCount;
    protected int score = 100;

    [SerializeField]
    protected Sprite[] Psprites;

    public bool isHardBody { get; protected set; }
    public bool isThroughBullet { get; protected set; }

    private CapsulType dropCapsul = CapsulType.None;
    protected EffectName explosionName = EffectName.Explose1;
    protected SeType explosionSeType = SeType.ExplosionS1;

    protected bool flipY = false;
    private float bulletSpeed = 1.0f;

    public virtual void Initialize(EnemySpawnData data)
    {
        base.Initialize();

        om = ObjectManager.instance;
        hm = HadManager.instance;

        this.enemyName = data.enemyName;
        this.position = data.position;
        this.dropCapsul = data.capsulType;
        SetSprite(0);
        hitBox = new Box(position, 6, 6);

        flipY = data.flipY;
        transform.localScale = flipY ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);

        shotCount = shotWait;
    }

    public override void Execute()
    {
        shotCount--;
        if (shotCount < 0)
        {
            shotCount = shotWait;

            if (disableAutoShot) return;
            Shot(position);
        }
    }

    protected void SetSprite(int index)
    {
        switch (dropCapsul)
        {
            case CapsulType.Power:
            case CapsulType.Flash:
                render.sprite = Psprites[index];
                break;
            default:
                render.sprite = sprites[index];
                break;
        }
    }

    public virtual void HitObject(SpriteBase obj)
    {
        isDead = true;
        hm.AddScore(score);

        ItemDrop();
        om.CreateEffect(explosionName, position);
        SoundManager.instance.PlayOneShotOnChannel(2, explosionSeType, 0.5f);
    }

    public virtual void FlashDamage()
    {
        isDead = true;

        ItemDrop();
        om.CreateEffect(explosionName, position);
    }

    private void ItemDrop()
    {
        switch (dropCapsul)
        {
            case CapsulType.Power:
                om.CreateItem(ItemName.PowerCapsul, position);
                break;
            case CapsulType.Flash:
                om.CreateItem(ItemName.FlashCapsul, position);
                break;
        }
    }

    protected void Shot(Vector2 p1)
    {
        Vector2 p2 = om.GetPlayer().position;
        Vector2 speed = Calculate.PositionToNomaliseVector(p1, p2) * bulletSpeed;
        om.CreateEBullet(p1, speed);
    }
}
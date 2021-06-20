using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    [SerializeField]
    private Player player = default;
    public Player GetPlayer() { return player; }

    [SerializeField]
    private GameObject[] EnemysPrefab = default;
    [SerializeField]
    private GameObject[] EffectsPrefab = default;
    [SerializeField]
    private GameObject[] ItemPrefab = default;
    [SerializeField]
    private GameObject EBulletPrefab = default;

    public List<Enemy> tempEnemys { get; private set; }
    public List<Enemy> enemys { get; private set; }
    public List<SpriteBase> effects { get; private set; }
    public List<Item> tempItems { get; private set; }
    public List<Item> items { get; private set; }
    public EBullet[] eBullets { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        player.Initialize();

        tempEnemys = new List<Enemy>();
        enemys = new List<Enemy>();
        effects = new List<SpriteBase>();
        tempItems = new List<Item>();
        items = new List<Item>();
    }

    public void InitializeEnemyBullet(Transform parent)
    {
        eBullets = new EBullet[64];
        for (int i = 0; i < eBullets.Length; i++)
        {
            eBullets[i] = Instantiate(EBulletPrefab, parent).GetComponent<EBullet>();
            eBullets[i].Initialize();
            eBullets[i].DestroyObject();
        }
    }

    public void Flush()
    {
        foreach (EBullet eb in eBullets)
        {
            eb.DestroyObject();
        }

        foreach (SpriteBase ef in effects)
        {
            ef.isDead = true;
        }

        foreach (Item itm in items)
        {
            itm.isDead = true;
        }

        foreach (Enemy en in enemys)
        {
            en.isDead = true;
        }
    }

    public Enemy CreateEnemy(EnemySpawnData data)
    {
        switch (data.enemyName)
        {
            case EnemyName.Whirl: return SetUpEnemy(0, data);
            case EnemyName.Rock: return SetUpEnemy(1, data);
            case EnemyName.Butterfly: return SetUpEnemy(2, data);
            case EnemyName.Sunfish: return SetUpEnemy(3, data);
            case EnemyName.Hopper: return SetUpEnemy(4, data);
            case EnemyName.Walker: return SetUpEnemy(5, data);
            case EnemyName.Canon: return SetUpEnemy(6, data);
            case EnemyName.Hexagon: return SetUpEnemy(7, data);
            case EnemyName.Base: return SetUpEnemy(8, data);
            case EnemyName.BigCore: return SetUpEnemy(9, data);
            case EnemyName.BigCoreMissle: return SetUpEnemy(10, data);
            default: throw new System.Exception("enemy not found. : " + data.enemyName);
        }
    }

    private Enemy SetUpEnemy(int index, EnemySpawnData data)
    {
        Enemy en = Instantiate(EnemysPrefab[index]).GetComponent<Enemy>();
        en.Initialize(data);
        tempEnemys.Add(en);
        return en;
    }

    public SpriteBase CreateEffect(EffectName name, Vector3 position)
    {
        switch (name)
        {
            case EffectName.Explose1: return SetUpEffect(0, position);
            case EffectName.Explose2: return SetUpEffect(1, position);
            case EffectName.Explose3: return SetUpEffect(2, position);
            case EffectName.Score5000: return SetUpEffect(3, position);
            case EffectName.OneUp: return SetUpEffect(4, position);
            default: throw new System.Exception("effect not found. : " + name);
        }
    }

    private SpriteBase SetUpEffect(int index, Vector3 position)
    {
        SpriteBase ef = Instantiate(EffectsPrefab[index]).GetComponent<SpriteBase>();
        ef.Initialize();
        ef.position = position;
        effects.Add(ef);
        return ef;
    }

    public Item CreateItem(ItemName type, Vector3 position)
    {
        switch (type)
        {
            case ItemName.PowerCapsul: return SetUpItem(type, 0, position);
            case ItemName.FlashCapsul: return SetUpItem(type, 1, position);
            case ItemName.Score5000: return SetUpItem(type, 2, position);
            case ItemName.OneUp: return SetUpItem(type, 3, position);
            default: throw new System.Exception("item not found. : " + type);
        }
    }

    private Item SetUpItem(ItemName type, int index, Vector3 position)
    {
        Item it = Instantiate(ItemPrefab[index]).GetComponent<Item>();
        it.Initialize(type);
        it.position = position;
        tempItems.Add(it);
        return it;
    }

    public void CreateEBullet(Vector3 position, Vector3 speed)
    {
        foreach (EBullet eb in eBullets)
        {
            if (eb.isDead == true)
            {
                eb.Spawn(position);
                eb.SetSpeed(speed);
                return;
            }
        }
    }

    public void Excute()
    {
        player.Execute();

        foreach (EBullet eb in eBullets)
        {
            eb.Execute();
        }

        foreach (SpriteBase ef in effects)
        {
            ef.Execute();
        }

        items.AddRange(tempItems);
        tempItems.Clear();
        foreach (Item itm in items)
        {
            itm.Execute();
        }

        enemys.AddRange(tempEnemys);
        tempEnemys.Clear();
        foreach (Enemy en in enemys)
        {
            en.Execute();
        }
    }

    public void CheckDestroy()
    {
        for (int i = enemys.Count - 1; 0 <= i; i--)
        {
            if (enemys[i].isDead)
            {
                Destroy(enemys[i].gameObject);
                enemys.RemoveAt(i);
            }
        }
        for (int i = effects.Count - 1; 0 <= i; i--)
        {
            if (effects[i].isDead)
            {
                Destroy(effects[i].gameObject);
                effects.RemoveAt(i);
            }
        }
        for (int i = items.Count - 1; 0 <= i; i--)
        {
            if (items[i].isDead)
            {
                Destroy(items[i].gameObject);
                items.RemoveAt(i);
            }
        }
    }

    public void CheckScreenOut(ScreenFrame frame)
    {
        player.x = frame.right - 16 < player.x ? frame.right - 16 : player.x;
        player.x = player.x < frame.left + 16 ? frame.left + 16 : player.x;
        player.y = frame.top - 8 < player.y ? frame.top - 8 : player.y;
        player.y = player.y < frame.bottom + 32 ? frame.bottom + 8 : player.y;

        foreach (Item itm in items)
        {
            if (ScreenOut(frame, itm.position, 16))
                itm.isDead = true;
        }

        foreach (SpriteBase ef in effects)
        {
            if (ScreenOut(frame, ef.position, 16))
                ef.isDead = true;
        }

        foreach (Enemy en in enemys)
        {
            if (ScreenOut(frame, en.position, 64))
                en.isDead = true;
        }

        foreach (Attacker at in player.attaker)
        {
            foreach (Bullet b in at.bullet)
            {
                if (!b.isDead)
                    if (ScreenOut(frame, b.position, 0))
                        b.DestroyObject();
            }
            foreach (Missile m in at.missile)
            {
                if (!m.isDead)
                    if (ScreenOut(frame, m.position, 0))
                        m.DestroyObject();
            }
        }
        foreach (EBullet eb in eBullets)
        {
            if (!eb.isDead)
                if (ScreenOut(frame, eb.position, 0))
                    eb.DestroyObject();
        }
    }

    private bool ScreenOut(ScreenFrame frame, Vector3 pos, int offset)
    {
        if (frame.right + offset < pos.x)
            return true;
        if (frame.left - offset > pos.x)
            return true;
        if (frame.top + offset < pos.y)
            return true;
        if (frame.bottom - offset > pos.y)
            return true;
        return false;
    }

}

using UnityEngine.Tilemaps;
using UnityEngine;

public class CollisionManager
{
	private Tilemap collisionMap;
	private int cellSize;

	public void SetCollisionMap(Tilemap collisionMap)
	{
		this.collisionMap = collisionMap;
		cellSize = (int)collisionMap.cellSize.x;
	}

	public void ObjectHitCheck(Player player)
	{
		ObjectManager om = ObjectManager.instance;
		Box pb = player.GetHitBox();

		foreach (Enemy en in om.enemys)
		{
			// Player
			if(!player.isDead)
            {
				if (BoxHitCheck(pb, en.GetHitBox()))
				{
					player.HitObject(en);
					en.HitObject(player);
					continue;
				}
			}

			foreach (Attacker at in player.attaker)
			{
				// Player Bullet
				foreach (Bullet bl in at.bullet)
				{
					if (!bl.isDead)
						if(!en.isThroughBullet)
							if (BoxHitCheck(bl.GetHitBox(), en.GetHitBox()))
							{
								en.HitObject(bl);
								bl.HitEnemy(en);
								break;
							}
				}

				if (en.isDead) continue;

				// Player Missile
				foreach (Missile ms in at.missile)
				{
					if (!ms.isDead)
						if (!en.isThroughBullet)
							if (BoxHitCheck(ms.GetHitBox(), en.GetHitBox()))
							{
								en.HitObject(ms);
								ms.HitObject(en);
								break;
							}
				}
			}

			if(en.enemyName == EnemyName.Walker)
            {
				GroundHitCheck(en as Walker);
			}
		}

		if (!player.isDead)
		{
			foreach (EBullet eb in om.eBullets)
			{
				if (BoxHitCheck(pb, eb.GetHitBox()))
				{
					player.HitObject(eb);
					eb.HitObject();
				}

				if (MapPointHitCheck(eb.position))
				{
					eb.HitObject();
				}
			}

			foreach (Item itm in om.items)
			{
				if (BoxHitCheck(pb, itm.GetHitBox()))
				{
					player.HitItem(itm.itemName);
					itm.HitObject(player);
				}
			}
		}

		// map hit

		if (!player.isDead)
		{
			if (MapBoxHitCheck(pb))
			{
				player.HitMap();
			}
		}

		foreach (Attacker at in player.attaker)
		{
			foreach (Bullet bl in at.bullet)
			{
				if (!bl.isDead)
					if (MapPointHitCheck(bl.position))
					{
						bl.HitMap();
					}
			}
			foreach (Missile ms in at.missile)
			{
				if (!ms.isDead)
					GroundHitCheck(ms);
			}
		}
	}

	public bool BoxHitCheck(Box box1, Box box2)
	{
		return box1.top > box2.bottom && box1.bottom < box2.top &&
			   box1.right > box2.left && box1.left < box2.right;
	}

	private void GroundHitCheck(IGroundWalker gw)
    {
		float xx = gw.position.x;
		float yy = gw.position.y;

		int sy = gw.speedY < 0 ? -gw.groundOffset : gw.groundOffset;
		Vector3Int ground = new Vector3Int((int)(xx / cellSize), (int)((yy + sy) / cellSize), 0);
		bool isGround = collisionMap.GetColliderType(ground) == Tile.ColliderType.Sprite;

		int sx = gw.speedX < 0 ? -8 : 8;
		Vector3Int forward = new Vector3Int((int)((xx + sx) / cellSize), (int)(yy / cellSize), 0);
		bool isWall = collisionMap.GetColliderType(forward) == Tile.ColliderType.Sprite;

		gw.CheckGround(isGround, isWall);
	}

	private bool MapPointHitCheck(Vector2 pos)
	{
		Vector3Int p = new Vector3Int((int)Mathf.Floor(pos.x / 8), (int)Mathf.Floor(pos.y / 8), 0);
		return collisionMap.GetColliderType(p) == Tile.ColliderType.Sprite;
	}

	private bool MapBoxHitCheck(Box box)
	{
		if (MapPointHitCheck(box.topLeft))
			return true;
		if (MapPointHitCheck(box.topRight))
			return true;
		if (MapPointHitCheck(box.bottomLeft))
			return true;
		if (MapPointHitCheck(box.bottomRight))
			return true;

		return false;
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageObject : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Tilemap collision;
    public Tilemap GetCollisionMap() { return collision; }

    private StageEvent[] events;
    private int checkPoint;
    public int GetCheckPoint() { return checkPoint; }

    public BgmType currentBgm { get; set; }

    public void Initialize(StageManager stageManager)
    {
        List<StageEvent> list = new List<StageEvent>();

        Transform evt = transform.Find("Event");
        int c = evt.childCount;
        for(int i=0; i < c; i++)
        {
            GameObject go = evt.GetChild(i).gameObject;

            if(go.GetComponent<BossEvent>() != null)
            {
                BossEvent be = go.GetComponent<BossEvent>();
                be.Initialize(this);
                be.OnEnter = stageManager.StopScroll;
                be.OnEnd = stageManager.StageEnd;
                list.Add(be);
            }
            else if (go.GetComponent<StageEvent>() != null)
            {
                StageEvent se = go.GetComponent<StageEvent>();
                se.Initialize(this);
                se.OnEnter = stageManager.StopScroll;
                se.OnEnd = stageManager.StartScroll;
                list.Add(se);
            }
        }
        events = list.ToArray();
        collision.gameObject.SetActive(false);

        currentBgm = BgmType.Space;
    }

    public void ReStart(int activeLine)
    {
        foreach (StageEvent se in events)
        {
            se.ResetObject(activeLine);
        }
    }

    public void ResetStage()
    {
        checkPoint = 0;
    }

    public void Excute(ScreenFrame frame)
    {
        foreach (StageEvent se in events)
        {
            se.Execute(frame);
        }
    }

    public void StageDestroy()
    {
        Destroy(gameObject);
    }

    public void UpdateCheckPoint(int checkPoint)
    {
        this.checkPoint = checkPoint;
    }
}

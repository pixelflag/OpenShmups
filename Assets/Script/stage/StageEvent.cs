using UnityEngine;

public class StageEvent : PixelObject
{
    protected StageObject stage;
    protected SoundManager sound;
    protected ObjectManager objectFactory;

    protected State state = State.Initialize;
    protected enum State
    {
        Initialize,
        Ready,
        Action,
        Done,
    }  

    public virtual void Initialize(StageObject stage)
    {
        this.stage = stage;
        base.Initialize();
        Destroy(GetComponent<SpriteRenderer>());

        sound = SoundManager.instance;
        objectFactory = ObjectManager.instance;
    }

    public void ResetObject(int activeLine)
    {
        base.ResetObject();

        if (x < activeLine)
            state = State.Done;
        else
            state = State.Initialize;
    }

    public virtual void Execute(ScreenFrame frame)
    {

    }

    public delegate void StartScrollDeregate();
    public StartScrollDeregate OnEnter;

    public delegate void StopScrollDeregate();
    public StopScrollDeregate OnEnd;

}

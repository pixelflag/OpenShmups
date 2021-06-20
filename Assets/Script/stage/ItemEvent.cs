using UnityEngine;

public class ItemEvent : StageEvent
{
    public Sprite[] sprites;
    public ItemName itemName;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[GetSpriteIndex()];
    }

    private int GetSpriteIndex()
    {
        switch (itemName)
        {
            case ItemName.Score5000: return 0;
            case ItemName.OneUp: return 1;
        }
        return 0;
    }

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
                    objectFactory.CreateItem(itemName, position);
                    state = State.Done;
                }
                break;
            case State.Done:
                break;
        }
    }
}
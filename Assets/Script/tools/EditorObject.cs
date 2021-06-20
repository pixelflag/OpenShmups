using UnityEngine;

public class EditorObject : MonoBehaviour
{
    void Awake()
    {
        Destroy(this.gameObject);
    }
}

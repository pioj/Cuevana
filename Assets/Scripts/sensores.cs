using UnityEngine;

public class sensores : MonoBehaviour
{
    [Header("sensores")] 
    public SpriteRenderer head;
    public SpriteRenderer leftRCS;
    public SpriteRenderer rightRCS;

    private void Update()
    {
        //sensores
        if (head) head.enabled = Input.GetKey(KeyCode.UpArrow) || Input.GetButton("Thrust");
        if (leftRCS) leftRCS.enabled = Input.GetKey(KeyCode.Q) || Input.GetAxisRaw("RCS") <= -1;
        if (rightRCS) rightRCS.enabled = Input.GetKey(KeyCode.E) || Input.GetAxisRaw("RCS") >= 1;
    }
}

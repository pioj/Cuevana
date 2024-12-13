using UnityEngine;

public class minion_spawnpoint : MonoBehaviour
{
    public Sprite crashSprite;
    [Header("Spawn")] 
    public GameObject prefabPickables;
    public int amount = 3;

    public void Spawn()
    {
        if (crashSprite)
        {
            var crash = new GameObject("House_destroyed");
            var spr = crash.AddComponent<SpriteRenderer>();
            spr.sprite = crashSprite;
            spr.sortingOrder = -1;
            crash.transform.position = transform.position;
            crash.isStatic = true;
        }

        if (amount > 0 && prefabPickables)
        {
            for (int i = 0; i < amount; i++)
            {
                var clon = Instantiate(prefabPickables,transform.position, Quaternion.identity);
                var rnd = Random.Range(-0.5f, 0.5f);
                clon.transform.Translate(rnd,0,0);
            }
        }
    }
}

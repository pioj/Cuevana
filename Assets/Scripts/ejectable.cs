using System;
using UnityEngine;

public class ejectable : MonoBehaviour
{
    public GameObject prefabEjectable;
    public float selfDestruction = 3f;
    private controllable cComp;

    private void Awake()
    {
        cComp = GetComponent<controllable>();
        if (!cComp) throw new Exception("Error! Ejectable depende de vehículo controllable!");
        if (!prefabEjectable) throw new Exception("Error! Ejectable requiere de un prefab de escape!");
    }

    private void Start() { }

    // Update is called once per frame
    void Update()
    {
        //Tecla de eyección, de momento SpaceBar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameMode.Lives++;

            if (prefabEjectable)
            {
                var clon = Instantiate(prefabEjectable, transform.position, Quaternion.identity);
                var comp = clon.GetComponent<ejectNSpawn>();

                var lado = Mathf.CeilToInt(transform.localScale.x);
                comp.Init(lado);
            }
            
            //pierdo el control de la nave...
            Destroy(gameObject,selfDestruction);
            GameMode.PrisonersKilled += GameMode.PrisonersAboard;
            GameMode.PrisonersAboard = 0;
            cComp.enabled = false;
        }
    }

}

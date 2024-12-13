using UnityEngine;

public class shieldable : MonoBehaviour
{
    public GameObject prefabShield;
    public bool shieldOn=false;
    public float energy;
    
    private GameObject shield;
    private SpriteRenderer ShieldRend;
    Color modo1 = Color.cyan * new Color(0,1,1,0.25f);   //Barrier
    Color modo2 = Color.red * new Color(1,0,0,0.25f);    //Absorb
    Color modo3 = Color.green * new Color(0,1,0,0.25f);  //Reflect
    
    private int currentMode = 0;

    private void Start()
    {
        shield = Instantiate(prefabShield, transform.position, Quaternion.identity);
        ShieldRend = shield.GetComponent<SpriteRenderer>();
        shield.transform.SetParent(transform);
        SetMode(currentMode);
        energy = 100f; //100%
        ToggleShield(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Shield"))
        {
            if (shieldOn != true) ToggleShield(true);
        }else if (Input.GetButtonUp("Shield"))
        {
            if (shieldOn != false) ToggleShield(false);
        }

        if (Input.GetButtonDown("ShieldMode"))
        {
            currentMode++;
            SetMode(currentMode);
        }


    }

    private void SetMode(int mode)
    {
        mode = mode % 3;
        switch (mode)
        {
            case 0:
                ShieldRend.color = modo1;
                break;
            case 1:
                ShieldRend.color = modo2;
                break;
            case 2:
                ShieldRend.color = modo3;
                break;
        }
    }

    private void ToggleShield(bool onOff)
    {
        shieldOn = onOff;
        ShieldRend.enabled = onOff;
        var sComp = shield.GetComponent<CircleCollider2D>();
        sComp.enabled = onOff;
    }


    //veamos si soporta el trigger del children...
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (currentMode)
        {
            //Barrier
            case 0:
            {
                if (other.CompareTag("bullet"))
                {
                    energy -= 10f;
                    energy = Mathf.Clamp(energy, 0f, 100f);
                    var bala = other.GetComponent<bullet>();
                    bala.Reset();
                }
                break;
            }
            //Absorb
            case 1:
            {
                if (other.CompareTag("bullet"))
                {
                    energy += 5f;
                    energy = Mathf.Clamp(energy, 0f, 100f);
                    var bala = other.GetComponent<bullet>();
                    bala.Reset();
                }
                break;
            }
            //Reflect
            case 2:
            {
                if (other.CompareTag("bullet"))
                {
                    energy -= 25f;
                    energy = Mathf.Clamp(energy, 0f, 100f);
                    var bala = other.GetComponent<bullet>();
                    var ndir = bala.GetDir();
                    bala.SetDir(Vector2.Reflect(ndir,ndir.normalized));
                }
                break;
            }
        }
    }
}

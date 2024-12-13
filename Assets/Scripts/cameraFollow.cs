using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform Target;
    public float factor = 2f;
    private Vector3 _newPos;
    
    private points_of_interest _poi;
    private GameObject _fallbackPOI;
    
    

    private void Awake()
    {
        _poi = FindObjectOfType<points_of_interest>();
    }

    private void Start()
    {
        _fallbackPOI = new GameObject("fallback_PointOFInterest");
        //_fallbackPOI.transform.SetParent(transform);
    }

    private void Update()
    {
        if (Target)
        {
            var newpos = new Vector3(Target.position.x, Target.position.y, -10f);
            _newPos = Vector3.Lerp(transform.position, newpos, factor * Time.deltaTime);
            
            transform.position = _newPos;
            
            //guardo esta posicion en el fallbackPOI
            _fallbackPOI.transform.position = Target.position;
        }
        else
        {
            Target = _fallbackPOI.transform;
            return;
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            //guardo esta posicion en el fallbackPOI
            if (Target) _fallbackPOI.transform.position = Target.position;

            var tg = _poi.GiveNextPOI();
            SetTarget(tg ? tg : _fallbackPOI.transform);
        }
    }

    public void SetTarget(Transform targ)
    {
        Target = targ;
    }
}

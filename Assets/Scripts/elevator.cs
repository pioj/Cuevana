using System;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
    [Range(1f,20f)] public float height = 3f;
    public bool isMoving;
    private bool _locked;

    public float timerfreq = 2f;
    private float timerstart;
    
    private Vector2 _startPos;
    private Vector2 _endPos;
    private Vector2 _currentPos;

    [Header("Parts")] 
    public SpriteRenderer tower;
    public Transform top;
    public Transform cage;

    [Header("Cage")] 
    public Sprite CageFree;
    public Sprite CageLocked;
    private SpriteRenderer _cageSpr;
    private PolygonCollider2D _cageCol;
    private List<Vector2> points;
    private List<Vector2> simplifiedPoints;
    private bool _changed;
    
    private void Start()
    {
        if (tower) tower.size = new Vector2(tower.size.x, height);
        if (top) top.transform.localPosition = new Vector3(top.transform.localPosition.x, height, 0);
        
        _startPos = cage.transform.position;
        _endPos = new Vector2(cage.transform.position.x, cage.transform.position.y + height);

        if (cage) _cageSpr = cage.GetComponent<SpriteRenderer>();
        if (cage) _cageCol = cage.GetComponent<PolygonCollider2D>();
        
        points = new List<Vector2>();
        simplifiedPoints = new List<Vector2>();
    }

    private void Update()
    {
        if (timerstart > timerfreq && !isMoving)
        {
            timerstart = 0;
            _currentPos = (_currentPos == _endPos) ? _startPos : _endPos;
            isMoving = true;
        }else if (!isMoving)
        {
            timerstart += 1f * Time.deltaTime;
        }

        /*
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Thrust"))
        {
            _currentPos = (_currentPos == _endPos) ? _startPos : _endPos;
            isMoving = true;
        }
        */
        
        _locked = isMoving;

        if (!_changed)
        {
            _cageSpr.sprite = (!_locked) ? CageFree : CageLocked;
            UpdatePolygonCollider2D();
            _changed = true;
        }

        if (isMoving) cage.transform.position = Vector2.MoveTowards(cage.transform.position, _currentPos, Time.deltaTime);
        
        if ((Vector2) cage.transform.position == _currentPos)
        {
            isMoving = false;
            _changed = false;
        }
    }
    
    private void UpdatePolygonCollider2D(float tolerance = 0.01f)
    {
        _cageCol.pathCount = _cageSpr.sprite.GetPhysicsShapeCount();
        for(int i = 0; i < _cageCol.pathCount; i++)
        {
            _cageSpr.sprite.GetPhysicsShape(i, points);
            LineUtility.Simplify(points, tolerance, simplifiedPoints);
            _cageCol.SetPath(i, simplifiedPoints);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.up * height));
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(cage.position,0.2f);
    }
    
}

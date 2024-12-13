using System.Collections;
using UnityEngine;

public class info_lifebar : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _rend;
    private static readonly int Life = Animator.StringToHash("life");

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _rend.enabled = false;
    }

    public void Show(float amount, Vector2 pos = new Vector2(), float duration = 2f)
    {
        _rend.enabled = true;
        amount = Mathf.Clamp01(amount);
        _anim.SetFloat(Life,amount);

        transform.localPosition = pos;
        
        //fix para los sprites que están rotados 90º, etc...
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        StartCoroutine(nameof(_showLifeBar), duration);
    }

    private IEnumerator _showLifeBar(float secs = 1f)
    {
        yield return new WaitForSeconds(secs);
        _rend.enabled = false;
    }
}

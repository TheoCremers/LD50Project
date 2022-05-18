using UnityEngine;
using System.Collections;

public abstract class BaseUnitAI : MonoBehaviour 
{   
    public SpriteRenderer Sprite;

    public Rigidbody2D RigidBody;

    [SerializeField]
    protected float _agroRange;

    protected float _currentAgroRange;

    [SerializeField]
    protected float _moveSpeed;

    protected Vector2 _facingDirection;    

    protected Transform _target;


    protected virtual void Start()
    {       
        _target = null;
        _currentAgroRange = _agroRange;
    }

    protected virtual void Update() 
    { 
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (_facingDirection.x < 0) 
        {
            Sprite.flipX = true;
        } 
        else if (_facingDirection.x > 0) 
        {
            Sprite.flipX = false;
        }
    }

    protected virtual IEnumerator FadeOutAndDestroy ()
    {
        var fadeTime = 0.2f;
        Color initialColor = Sprite.color;
        float t = 0f;
        while (t < fadeTime * 0.5f)
        {
            t += Time.deltaTime;
            Sprite.color = Color.Lerp(initialColor, Color.black, t * 2f / fadeTime);
            yield return null;
        }
        t = 0f;
        while (t < fadeTime * 0.5f)
        {
            t += Time.deltaTime;
            Sprite.color = Color.Lerp(Color.black, Color.clear, t * 2f / fadeTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}



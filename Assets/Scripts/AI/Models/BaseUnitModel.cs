using UnityEngine;

public abstract class BaseUnitModel : MonoBehaviour
{   
    [SerializeField]
    protected float _agroRange;

    protected float _currentAgroRange;

    [SerializeField]
    protected float _moveSpeed;

    internal Vector2 _moveDirection;    

    public SpriteRenderer Sprite;
    
    public Rigidbody2D RigidBody;

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
        if (_moveDirection.x < 0) 
        {
            Sprite.flipX = true;
        } 
        else if (_moveDirection.x > 0) 
        {
            Sprite.flipX = false;
        }
    }
}



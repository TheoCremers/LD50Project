using UnityEngine;

namespace LD50.Scripts.AI 
{
    public abstract class BaseUnitAI : MonoBehaviour 
    {   
        [SerializeField]
        protected float _moveSpeed;

        protected Vector2 _moveDirection;    

        public SpriteRenderer Sprite;

        protected Transform _target;

        public Rigidbody2D RigidBody;

        protected virtual void Start()
        {       
            _target = null;
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
}


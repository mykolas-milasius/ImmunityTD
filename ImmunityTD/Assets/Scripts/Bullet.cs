using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour
    {
        private GameObject _target;
        public Tower Tower;
        public float Speed = 70f;
        public float Damage = 10f;
        public float LifeTime = 2f;

        private bool _isDestroyed = false;

        public void Seek(GameObject target)
        {
            this._target = target;
        }

        void Start()
        {
            Destroy(gameObject, LifeTime);
        }

        void Update()
        {
            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 direction = _target.transform.position - transform.position;
            float distanceThisFrame = Speed * Time.deltaTime;

            if (direction.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        }

        void HitTarget()
        {
            if (Tower != null && _target != null)
            {
                Tower.DealDamage(_target);
                DestroyBullet();
            }
        }

        void DestroyBullet()
        {
            if (!_isDestroyed)
            {
                Destroy(gameObject);
                _isDestroyed = true;
            }
        }
    }
}
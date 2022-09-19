using RPG.Attributes;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 20f;
        [SerializeField] float destroyAfterTime = 2f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] UnityEvent onHit;
        [SerializeField] bool isSpell = false;

        Health target = null;
        BaseStats baseStats;

        Vector3 targetPoint;
        GameObject instigator = null;
        float damage = 0f;

        private void Start()
        {
            transform.LookAt(GetAimLocation());

            baseStats = instigator.GetComponent<BaseStats>();
        }

        private void Update()
        {
            if (target != null && isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }

        public void SetTarget(GameObject instigator, float damage, Health target = null, Vector3 targetPoint = default)
        {
            this.target = target;
            this.targetPoint = targetPoint;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if (target == null) return targetPoint;

            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();

            if (targetCollider == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();

            if (target != null && health != target) return;
            if (health == null || health.IsDead()) return;
            if (other.gameObject == instigator) return;

            if (isSpell)
            {
                damage = damage + baseStats.GetStat(Stat.SpellDamage);
            }

            health.TakeDamage(instigator, damage);

            speed = 0;

            onHit.Invoke();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, destroyAfterTime);
        }
    }
}
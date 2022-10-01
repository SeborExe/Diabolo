using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using RPG.Utils;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Collections;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        AudioSource audioSource;

        [SerializeField] float regenerationPercentage = 100;
        [SerializeField] public TakeDamageEvent takeDamage;
        [SerializeField] InfoAboveHead infoAboveHead = null;

        [Header("Audio")]
        [SerializeField] AudioClip[] OnHitClips;
        [SerializeField] AudioClip[] OnDieClips;
        [SerializeField] Vector2 onHitsVolume = new Vector2(0.2f, 1f);
        [SerializeField] Vector2 onDieVolume = new Vector2(0.2f, 1f);

        [Header("Health Regeneration")]
        [SerializeField] float timeToRegeneration = 5f;
        [SerializeField] float maxRegenerationPercent = 60f;
        float regenerationTimer = 0;

        public event Action OnTakeDamage;
        public event Action OnDie;
        public UnityEvent OnDieEvent;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        Animator animator;
        ActionScheduler actionScheduler;
        BaseStats baseStats;

        LazyValue<float> healthPoints;

        bool isDead;

        public bool IsDead() => isDead;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            baseStats = GetComponent<BaseStats>();
            audioSource = GetComponent<AudioSource>();

            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private void Start()
        {
            healthPoints.ForceInit();

            if (infoAboveHead != null)
            {
                infoAboveHead.UpdateHealthBar(GetHealthPoints(), GetMaxHealthPoints());
            }
        }

        private void Update()
        {
            UpdateTimers();
            HealthRegeneration();
        }

        private float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            baseStats.OnLevelUp += RegenerateHealth;

            OnDie += PlayDieClip;
            OnTakeDamage += PlayTakeDamageClip;
        }

        private void OnDisable()
        {
            baseStats.OnLevelUp -= RegenerateHealth;

            OnDie -= PlayDieClip;
            OnTakeDamage -= PlayTakeDamageClip;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            CheckBlock(damage, out damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            regenerationTimer = timeToRegeneration;

            OnTakeDamage?.Invoke();

            if (infoAboveHead != null)
            {
                infoAboveHead.isDamaged = true;
                infoAboveHead.UpdateHealthBar(GetHealthPoints(), GetMaxHealthPoints());
            }

            if (healthPoints.value == 0)
            {
                OnDie?.Invoke();
                OnDieEvent?.Invoke();
                Die();
                AwardExperience(instigator);
            }

            takeDamage.Invoke(damage);
        }

        private void HealthRegeneration()
        {
            if (healthPoints.value < GetMaxHealthPoints() && regenerationTimer == 0)
            {
                if (healthPoints.value >= GetMaxHealthPoints() * (maxRegenerationPercent / 100f)) return;

                healthPoints.value += Time.deltaTime * GetRegenerate();

                if (healthPoints.value > GetMaxHealthPoints()) healthPoints.value = GetMaxHealthPoints();
            }
        }

        private void UpdateTimers()
        {
            if (regenerationTimer > 0)
            {
                regenerationTimer -= Time.deltaTime;

                if (regenerationTimer < 0) regenerationTimer = 0;
            } 
        }

        public void Heal(float amount)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + amount, GetMaxHealthPoints()); 
        }

        private float GetRegenerate()
        {
            return GetComponent<BaseStats>().GetStat(Stat.HealthRegeneration);
        }

        public void StartHealCoroutine(float amoutToHeal, int perioid, float timeBetweenHeal)
        {
            StartCoroutine(HealRoutine(amoutToHeal, perioid, timeBetweenHeal));
        }

        private IEnumerator HealRoutine(float amoutToHeal, int perioid, float timeBetweenHeal)
        {
            for (int i = 0; i < perioid; i++)
            {
                Heal(amoutToHeal);
                yield return new WaitForSeconds(timeBetweenHeal);
            }
        }

        private float CheckBlock(float damage, out float ChangedDamage)
        {
            float chanceToBlock = baseStats.GetStat(Stat.ChanceToBlock);
            float randomChance = UnityEngine.Random.Range(1.0001f, 2.01f);

            if (randomChance <= chanceToBlock)
            {
                return ChangedDamage = 0;
            }
            else
            {
                return ChangedDamage = damage;
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return baseStats.GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return (healthPoints.value / baseStats.GetStat(Stat.Health)) * 100;
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger("die");
            actionScheduler.CancelCurrentAction();
        }

        private void PlayDieClip()
        {
            audioSource.clip = GetRandomDieClip();
            audioSource.volume = GetRandomVolume(onDieVolume);
            audioSource.Play();
        }

        private void PlayTakeDamageClip()
        {
            audioSource.clip = GetRandomTakeDamageClip();
            audioSource.volume = GetRandomVolume(onHitsVolume);
            audioSource.Play();
        }

        private AudioClip GetRandomDieClip()
        {
            int randomIndex = UnityEngine.Random.Range(0, OnDieClips.Length);
            return OnDieClips[randomIndex];
        }

        private AudioClip GetRandomTakeDamageClip()
        {
            int randomIndex = UnityEngine.Random.Range(0, OnHitClips.Length);
            return OnHitClips[randomIndex];
        }

        private float GetRandomVolume(Vector2 volume)
        {
            float randomVolume = UnityEngine.Random.Range(volume.x, volume.y);
            return randomVolume;
        }

        private void RegenerateHealth()
        {
            float regenHealth = healthPoints.value = baseStats.GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealth);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) { return; }

            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value <= 0)
            {
                Die();
            }
        }
    }
}

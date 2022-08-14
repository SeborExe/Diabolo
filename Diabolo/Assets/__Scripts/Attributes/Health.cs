using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using RPG.Utils;
using UnityEngine.Events;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        AudioSource audioSource;

        [SerializeField] float regenerationPercentage = 100;
        [SerializeField] public TakeDamageEvent takeDamage;
        [SerializeField] InfoAboveHead infoAboveHead = null;
        [SerializeField] AudioClip[] OnHitClips;
        [SerializeField] AudioClip[] OnDieClips;

        public event Action OnTakeDamage;
        public event Action OnDie;

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

        private float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            baseStats.onLevelUp += RegenerateHealth;

            OnDie += PlayDieClip;
            OnTakeDamage += PlayTakeDamageClip;
        }

        private void OnDisable()
        {
            baseStats.onLevelUp -= RegenerateHealth;

            OnDie -= PlayDieClip;
            OnTakeDamage -= PlayTakeDamageClip;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            OnTakeDamage?.Invoke();

            if (infoAboveHead != null)
            {
                infoAboveHead.isDamaged = true;
                infoAboveHead.UpdateHealthBar(GetHealthPoints(), GetMaxHealthPoints());
            }

            if (healthPoints.value == 0)
            {
                OnDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public void Heal(float amount)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + amount, GetMaxHealthPoints()); 
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
            audioSource.Play();
        }

        private void PlayTakeDamageClip()
        {
            audioSource.clip = GetRandomTakeDamageClip();
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

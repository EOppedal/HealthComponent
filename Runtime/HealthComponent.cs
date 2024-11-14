using System;
using System.Collections.Generic;
using PlayerLoopTimer;
using UnityEngine;

namespace HealthComponent {
    public abstract class HealthComponent<T> : MonoBehaviour, IHealthComponent where T : IComparable<T> {
        [SerializeField] protected T health;
        public T maxHealth;

        [SerializeField] protected float invulnerabilityTime = 0.5f;
        [field: SerializeField] public bool HasInvincibilityFrames { get; protected set; }

        public event Action<T> OnHealthChanged = delegate { };
        public event Action OnHealingReceived = delegate { };
        public event Action OnTakingDamage = delegate { };
        public event Action OnDeath = delegate { };

        public readonly List<Timer> TimersStoppedWhenTakingDamage = new();

        public CountdownTimer InvulnerabilityTimer;

        public T Health {
            get => health;
            set {
                health = Clamp(value, default, maxHealth);
                InvokeHealthChangedEvent();
                if (health.CompareTo(ZeroValue) <= 0) {
                    OnDeath?.Invoke();
                }
            }
        }

        protected virtual void Awake() {
            InvulnerabilityTimer = new CountdownTimer(invulnerabilityTime);
            InvulnerabilityTimer.OnBegin += () => {
                InvulnerabilityTimer.Duration = invulnerabilityTime;
                HasInvincibilityFrames = true;
            };
            InvulnerabilityTimer.OnComplete += () => HasInvincibilityFrames = false;

            OnTakingDamage += () => {
                foreach (var timer in TimersStoppedWhenTakingDamage) {
                    timer.StopTimer();
                }
            };
        }

        public virtual void TriggerInvulnerability(float duration = default) {
            if (InvulnerabilityTimer.IsRunning) return;

            InvulnerabilityTimer.Duration = Mathf.Approximately(duration, default) ? invulnerabilityTime : duration;
            InvulnerabilityTimer.StartTimer();
        }

        public virtual void InvokeHealthChangedEvent() {
            OnHealthChanged?.Invoke(Health);
        }

        public virtual void TakeDamage(T amount) {
            if (HasInvincibilityFrames) return;

            Health = Subtract(Health, amount);

            OnTakingDamage?.Invoke();

            TriggerInvulnerability();
        }

        public virtual void Heal(T amount) {
            Health = Add(Health, amount);
            OnHealingReceived?.Invoke();
        }

        public static implicit operator T(HealthComponent<T> healthComponent) {
            return healthComponent.Health;
        }

        protected abstract T ZeroValue { get; }
        protected abstract T Clamp(T value, T min, T max);
        protected abstract T Subtract(T a, T b);
        protected abstract T Add(T a, T b);
    }
}
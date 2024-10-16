using System;
using System.Collections.Generic;
using PlayerLoopTimer;
using UnityEngine;

namespace HealthComponent {
    public abstract class HealthComponent<T> : MonoBehaviour, IHealthComponent where T : IComparable<T> {
        [SerializeField] protected T health;
        public T maxHealth;

        [SerializeField] private float invulnerabilityTime = 0.5f;
        private bool _hasInvincibilityFrames;

        public event Action<T> OnHealthChanged = delegate { };
        public event Action OnHealingReceived = delegate { };
        public event Action OnTakingDamage = delegate { };
        public event Action OnDeath = delegate { };

        public readonly List<Timer> TimersStoppedWhenTakingDamage = new();

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

        private CountdownTimer _invulnerabilityTimer;

        protected virtual void Awake() {
            _invulnerabilityTimer = new CountdownTimer(invulnerabilityTime);
            _invulnerabilityTimer.OnBegin += () => {
                _invulnerabilityTimer.Duration = invulnerabilityTime;
                _hasInvincibilityFrames = true;
            };
            _invulnerabilityTimer.OnComplete += () => _hasInvincibilityFrames = false;

            OnTakingDamage += () => {
                foreach (var timer in TimersStoppedWhenTakingDamage) {
                    timer.StopTimer();
                }
            };
        }

        public virtual void TriggerInvulnerability(float duration = default) {
            if (_invulnerabilityTimer.TimerRunning) return;

            _invulnerabilityTimer.Duration = Mathf.Approximately(duration, default) ? invulnerabilityTime : duration;
            _invulnerabilityTimer.StartTimer();
        }

        public virtual void InvokeHealthChangedEvent() {
            OnHealthChanged?.Invoke(Health);
        }

        public virtual void TakeDamage(T amount) {
            if (_hasInvincibilityFrames) return;

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
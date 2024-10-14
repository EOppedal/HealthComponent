using System;
using HealthComponent;

namespace RegenerationEffect {
    public abstract class RegenInstance<T> where T : IComparable<T> {
        protected readonly float HealingIntervalTime;
        protected readonly T HealingAmountPerInterval;
        protected readonly float Duration;

        protected RegenInstance(float healingIntervalTime, T healingAmountPerInterval, float duration) {
            HealingIntervalTime = healingIntervalTime;
            HealingAmountPerInterval = healingAmountPerInterval;
            Duration = duration;
        }

        public abstract void ApplyTo(IHealthComponent healthComponent);
    }
}
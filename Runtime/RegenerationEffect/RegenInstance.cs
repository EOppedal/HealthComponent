using System;
using HealthComponent;

namespace RegenerationEffect {
    public abstract class RegenInstance<T> where T : IComparable<T> {
        protected readonly float HealingInterval;
        protected readonly T HealingAmountPerInterval;
        protected readonly float Duration;
        protected readonly T HealingAmount;

        protected RegenInstance(float healingInterval, T healingAmountPerInterval, float duration, T healingAmount) {
            HealingInterval = healingInterval;
            HealingAmountPerInterval = healingAmountPerInterval;
            Duration = duration;
            HealingAmount = healingAmount;
        }

        public abstract void ApplyTo(IHealthComponent healthComponent);
    }
}
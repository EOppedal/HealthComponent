using HealthComponent;
using PlayerLoopTimer;
using UnityEngine;

namespace RegenerationEffect {
    public class RegenInstanceFloat : RegenInstance<float> {
        public RegenInstanceFloat(float healingAmountPerInterval, float healingInterval, float duration)
            : base(healingInterval, healingAmountPerInterval, duration) {
        }

        public RegenInstanceFloat(float totalHealingAmount, float healingInterval, float duration, float _ = 0)
            : base(healingInterval, totalHealingAmount / (duration / healingInterval), duration) {
        }

        private static Timer RegenerationTimerInterval(HealthComponentFloat healthComponent,
            float healingAmountPerInterval, float duration, float healingInterval) {
            var regenTimer = new IntervalTimer(duration, healingInterval);
            regenTimer.OnInterval += () => healthComponent.Heal(healingAmountPerInterval);
            regenTimer.StartTimer();

            return regenTimer;
        }

        public override void ApplyTo(IHealthComponent healthComponent) {
            if (healthComponent is HealthComponentFloat healthComponentFloat) {
                healthComponentFloat.TimersStoppedWhenTakingDamage.Add(
                    RegenerationTimerInterval(healthComponentFloat,
                        HealingAmountPerInterval, Duration, HealingInterval));
            }
            else {
                Debug.LogError("HealthComponent is not a HealthComponentFloat and healing could not be applied.");
            }
        }
    }
}
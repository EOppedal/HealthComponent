using HealthComponent;
using PlayerLoopTimer;
using UnityEngine;

namespace RegenerationEffect {
    public class RegenInstanceFloat : RegenInstance<float> {
        public RegenInstanceFloat(float healingAmountPerInterval, float healingIntervalTime, float duration)
            : base(healingIntervalTime, healingAmountPerInterval, duration) {
        }

        public RegenInstanceFloat(float totalHealingAmount, float healingIntervalTime, float duration, float _ = 0)
            : base(healingIntervalTime, totalHealingAmount / (duration / healingIntervalTime), duration) {
        }
        
        public RegenInstanceFloat(float totalHealingAmount, int healingIntervals, float duration)
            : base(duration / healingIntervals, totalHealingAmount / (duration / (duration / healingIntervals)), duration) {
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
                        HealingAmountPerInterval, Duration, HealingIntervalTime));
            }
            else {
                Debug.LogError("HealthComponent is not a HealthComponentFloat and healing could not be applied.");
            }
        }
    }
}
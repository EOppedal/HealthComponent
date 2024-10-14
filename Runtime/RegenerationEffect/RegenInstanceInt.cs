using HealthComponent;
using PlayerLoopTimer;
using UnityEngine;

namespace RegenerationEffect {
    public class RegenInstanceInt : RegenInstance<int> {
        public RegenInstanceInt(float healingInterval, int healingAmountPerInterval, float duration, int healingAmount)
            : base(healingInterval, healingAmountPerInterval, duration, healingAmount) {
        }

        private static Timer RegenerationTimerInterval(HealthComponentInt healthComponent,
            int healingAmountPerInterval, float duration, float healingInterval) {
            var regenTimer = new IntervalTimer(duration, healingInterval);
            regenTimer.OnInterval += () => healthComponent.Heal(healingAmountPerInterval);
            regenTimer.StartTimer();

            return regenTimer;
        }

        public override void ApplyTo(IHealthComponent healthComponent) {
            if (healthComponent is HealthComponentInt healthComponentFloat) {
                healthComponentFloat.TimersStoppedWhenTakingDamage.Add(
                    RegenerationTimerInterval(healthComponentFloat,
                        HealingAmountPerInterval, Duration, HealingInterval));
            }
            else {
                Debug.LogError("HealthComponent is not a HealthComponentInt and healing could not be applied.");
            }
        }
    }
}
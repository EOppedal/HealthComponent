using HealthComponent;
using PlayerLoopTimer;
using UnityEngine;

namespace RegenerationEffect {
    /// <summary>
    /// TotalHealingAmount may give wrong totals if the increment calculated is not an int
    /// </summary>
    public class RegenInstanceInt : RegenInstance<int> {
        public RegenInstanceInt(int healingAmountPerInterval, float healingIntervalTime, float duration)
            : base(healingIntervalTime, healingAmountPerInterval, duration) {
        }

        public RegenInstanceInt(int totalHealingAmount, float healingIntervalTime, float duration, int _ = 0)
            : base(healingIntervalTime, (int)(totalHealingAmount / (duration / healingIntervalTime)), duration) {
        }
        
        public RegenInstanceInt(int totalHealingAmount, int healingIntervals, float duration)
            : base(duration / healingIntervals, (int)(totalHealingAmount / (duration / (duration / healingIntervals))), duration) {
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
                        HealingAmountPerInterval, Duration, HealingIntervalTime));
            }
            else {
                Debug.LogError("HealthComponent is not a HealthComponentInt and healing could not be applied.");
            }
        }
    }
}
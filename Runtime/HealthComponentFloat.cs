using UnityEngine;

namespace HealthComponent {
    public class HealthComponentFloat : HealthComponent<float> {
        protected override float ZeroValue => 0f;

        protected override float Clamp(float value, float min, float max) {
            return Mathf.Clamp(value, min, max);
        }

        protected override float Subtract(float a, float b) {
            return a - b;
        }

        protected override float Add(float a, float b) {
            return a + b;
        }
    }
}
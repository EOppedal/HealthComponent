using System;

namespace HealthComponent {
    public class HealthComponentInt : HealthComponent<int> {
        protected override int ZeroValue => 0;

        protected override int Clamp(int value, int min, int max) {
            return Math.Clamp(value, min, max);
        }

        protected override int Subtract(int a, int b) {
            return a - b;
        }

        protected override int Add(int a, int b) {
            return a + b;
        }
    }
}
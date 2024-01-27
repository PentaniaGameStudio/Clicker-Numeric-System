using NymphoMini.Clicker;
using System.Collections;
using UnityEngine;

namespace NymphoMini
{
    /// <summary>
    /// Number System with an integred Temporary system to create a fever effect or something like this
    /// </summary>
    public class ComplexNumberSystem : NumberSystem
    {
        // Current value of the number.
        public override double Number
        {
            get
            {
                return TempBoost ? tempNumber : number;
            }
            set
            {
                number = number + value;
                tempNumber = number * TempMultiplicator;
            }
        }

        [field: SerializeField]
        protected double tempNumber = 0;

        // Temporary multiplicator, used for scaling the number during arithmetic operations when a temporary boost is made.
        [field: SerializeField]
        public double TempMultiplicator { get; protected set; } = 1;
        [field: SerializeField]
        public bool TempBoost { get; protected set; } = false;


        /// <summary>
        /// Start a temporary boost.
        /// </summary>
        /// <param name="boostValue">The boost to add. By default TempBoost is set to 1, so if you add 2 that would mean x3 and not x2</param>
        /// <param name="duration">The duration of the boost.</param>
        public void AddTemporaryBoost(double boostValue = 2f, float duration = 5f)
        {
            TempBoost = true;
            TempMultiplicator += boostValue;
            tempNumber = number * TempMultiplicator;
            GameEngine.instance.StartCoroutine(RemoveTemporaryBoost(boostValue, duration));
        }

        protected IEnumerator RemoveTemporaryBoost(double boostValue, float duration)
        {
            yield return new WaitForSeconds(duration);
            TempMultiplicator -= boostValue;
            if (TempMultiplicator == 1)
                TempBoost = false;
        }

    }
}

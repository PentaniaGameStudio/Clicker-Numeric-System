using UnityEngine;

namespace NymphoMini.Clicker
{
    /// <summary>
    /// Represents a number with a magnitude (scale), supporting basic arithmetic operations like addition and multiplication.
    /// </summary>
    [System.Serializable]
    public class NumberSystem
    {
        #region Const

        // Base value used for scaling the magnitude.
        private const double BaseMagnitude = 1000;

        // Minimum value used for adjustments in magnitude scaling.
        private const double MinMagnitudeValue = 0.000001f;

        #endregion

        // Current value of the number.
        [field: SerializeField]
        public double Number { get; private set; } = 0;

        // Current multiplicator, used for scaling the number during arithmetic operations.
        [field: SerializeField]
        public double Multiplicator { get; private set; } = 1;

        // Current magnitude (scale) of the number.
        [field: SerializeField]
        public QuantityMagnitude Magnitude { get; private set; } = QuantityMagnitude.Unit;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the NumberSystem class with default values.
        /// </summary>
        public NumberSystem()
        {
            Number = 0;
            Magnitude = QuantityMagnitude.Unit;
        }

        /// <summary>
        /// Initializes a new instance of the NumberSystem class with a specific number and magnitude.
        /// </summary>
        public NumberSystem(double number, QuantityMagnitude magnitude)
        {
            Number = number;
            Magnitude = magnitude;
        }

        /// <summary>
        /// Initializes a new instance of the NumberSystem class with a specific number.
        /// </summary>
        public NumberSystem(double number)
        {
            Number = number;
            Magnitude = QuantityMagnitude.Unit;
        }

        #endregion

        #region Public Function

        /// <summary>
        /// Adds another NumberSystem instance to this instance.
        /// </summary>
        public void Add(NumberSystem number) => Add(number.Number, number.Magnitude);

        /// <summary>
        /// Adds a value with a specified magnitude to the current number, adjusting the magnitude if necessary.
        /// </summary>
        public void Add(double value, QuantityMagnitude valueMagnitude)
        {
            // Check of the difference in magnitude.
            int magnitudeDifference = Magnitude - valueMagnitude;

            // Multiplicate the added value by the current multiplicator.
            value *= Multiplicator;

            // Depending on the difference, adjust Number.
            switch (magnitudeDifference)
            {
                case 0:
                    Number += value;
                    break;

                case > 0:
                    Number += AdjustForSmallerMagnitude(value, magnitudeDifference);
                    break;

                default:
                    AdjustForBiggerMagnitude(value, valueMagnitude, -magnitudeDifference);
                    break;
            }

            NormalizeMagnitude();
        }

        /// <summary>
        /// Multiplies the current number by a specified value.
        /// </summary>
        public void Multiply(double value = 2f)
        {
            Number *= value;
            NormalizeMagnitude();
        }

        #endregion

        #region Private Function

        // Adjusts the value for smaller magnitude differences.
        private double AdjustForSmallerMagnitude(double value, int magnitudeDifference)
        {
            // If the difference is below 2, round to the min magnitude value.
            return magnitudeDifference <= 2 ? value / Mathf.Pow((float)BaseMagnitude, magnitudeDifference) : MinMagnitudeValue;
        }

        // Adjusts the current number for bigger magnitude differences.
        private void AdjustForBiggerMagnitude(double value, QuantityMagnitude valueMagnitude, int magnitudeDifference)
        {
            Number = (Number / Mathf.Pow((float)BaseMagnitude, magnitudeDifference)) + value;
            Magnitude = valueMagnitude;
        }

        // Normalizes the current number to ensure it stays within the appropriate magnitude.
        private void NormalizeMagnitude()
        {
            while (Number >= BaseMagnitude)
            {
                Number /= BaseMagnitude;
                Magnitude++;
            }
        }

        #endregion

        #region ToString override

        /// <summary>
        /// Returns a string that represents the current NumberSystem object.
        /// </summary>
        public override string ToString()
        {
            return Magnitude > QuantityMagnitude.Unit ? $"{(int)Number}{GetSuffix(Magnitude)} {(int)((Number - (int)Number) * BaseMagnitude)}{GetSuffix(Magnitude - 1)}" : ((int)Number).ToString();
        }

        // Returns the suffix associated with a given magnitude.
        private static string GetSuffix(QuantityMagnitude magnitude)
        {
            return magnitude == QuantityMagnitude

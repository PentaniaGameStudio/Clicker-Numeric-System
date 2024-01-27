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
        protected const double BaseMagnitude = 1000;

        // Minimum value used for adjustments in magnitude scaling.
        protected const double MinMagnitudeValue = 0.000001f;

        #endregion

        // Current value of the number.
        public virtual double Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

        [field: SerializeField]
        protected double number = 0;

        // Current multiplicator, used for scaling the number during arithmetic operations.
        [field: SerializeField]
        public double Multiplicator { get; protected set; } = 1;


        // Current magnitude (scale) of the number.
        [field: SerializeField]
        public QuantityMagnitude Magnitude { get; protected set; } = QuantityMagnitude.Unit;

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
        /// Multiplies the current number by a specified value. 1 mean x2, while 0.25 mean x1.25
        /// </summary>
        public void Multiply(double value = 1f)
        {
            Number += Number * value;
            Multiplicator += value;
            NormalizeMagnitude();
        }
        #endregion

        #region Private Function

        // Adjusts the value for smaller magnitude differences.
        protected double AdjustForSmallerMagnitude(double value, int magnitudeDifference)
        {
            // If the difference is below 2, round to the min magnitude value.
            return magnitudeDifference <= 2 ? value / Mathf.Pow((float)BaseMagnitude, magnitudeDifference) : MinMagnitudeValue;
        }

        // Adjusts the current number for bigger magnitude differences.
        protected void AdjustForBiggerMagnitude(double value, QuantityMagnitude valueMagnitude, int magnitudeDifference)
        {
            Number = (Number / Mathf.Pow((float)BaseMagnitude, magnitudeDifference)) + value;
            Magnitude = valueMagnitude;
        }

        // Normalizes the current number to ensure it stays within the appropriate magnitude.
        protected void NormalizeMagnitude()
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
        protected static string GetSuffix(QuantityMagnitude magnitude)
        {
            return magnitude == QuantityMagnitude.Unit ? "" : magnitude.ToString();
        }

        #endregion

    }
}

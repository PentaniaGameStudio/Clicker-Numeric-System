using UnityEngine;

namespace NymphoMini.Clicker
{
    [System.Serializable]
    public class NumberSystem
    {
        #region Const

        private const double BaseMagnitude = 1000;
        private const double MinMagnitudeValue = 0.000001f;

        #endregion

        [field: SerializeField]
        public double Number { get; private set; } = 0;

        [field: SerializeField]
        public QuantityMagnitude Magnitude { get; private set; } = QuantityMagnitude.Unit;

        #region Constructor

        public NumberSystem()
        {
            Number = 0;
            Magnitude = QuantityMagnitude.Unit;
        }

        public NumberSystem(double number, QuantityMagnitude magnitude)
        {
            Number = number;
            Magnitude = magnitude;
        }

        public NumberSystem(double number)
        {
            Number = number;
            Magnitude = QuantityMagnitude.Unit;
        }

        #endregion

        #region Public Function

        public void Add(NumberSystem number) => Add(number.Number, number.Magnitude);

        public void Add(double value, QuantityMagnitude valueMagnitude)
        {
            int magnitudeDifference = Magnitude - valueMagnitude;

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

        public void Multiply(double value = 2f)
        {
            Number *= value;
            NormalizeMagnitude();
        }

        #endregion

        #region Private Function

        private double AdjustForSmallerMagnitude(double value, int magnitudeDifference)
        {
            return magnitudeDifference <= 2 ? value / Mathf.Pow((float)BaseMagnitude, magnitudeDifference) : MinMagnitudeValue;
        }

        private void AdjustForBiggerMagnitude(double value, QuantityMagnitude valueMagnitude, int magnitudeDifference)
        {
            Number = (Number / Mathf.Pow((float)BaseMagnitude, magnitudeDifference)) + value;
            Magnitude = valueMagnitude;
        }

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

        public override string ToString()
        {
            return Magnitude > QuantityMagnitude.Unit ? $"{(int)Number}{GetSuffix(Magnitude)} {(int)((Number - (int)Number) * BaseMagnitude)}{GetSuffix(Magnitude - 1)}" : ((int)Number).ToString();
        }

        private static string GetSuffix(QuantityMagnitude magnitude)
        {
            return magnitude == QuantityMagnitude.Unit ? "" : magnitude.ToString();
        }

        #endregion

    }
}

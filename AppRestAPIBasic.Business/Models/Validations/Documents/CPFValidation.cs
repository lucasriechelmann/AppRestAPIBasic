namespace AppRestAPIBasic.Business.Models.Validations.Documents
{
    public class CPFValidation
    {
        public const int CPF_LENGTH = 11;

        public static bool Validate(string cpf)
        {
            var cpfNumbers = Utils.OnlyNumber(cpf);

            if (!IsLengthValid(cpfNumbers)) return false;
            return !AreThereRepeatedDigits(cpfNumbers) && AreThereValidDigits(cpfNumbers);
        }

        private static bool IsLengthValid(string value)
        {
            return value.Length == CPF_LENGTH;
        }

        private static bool AreThereRepeatedDigits(string value)
        {
            string[] invalidNumbers =
            {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999"
            };
            return invalidNumbers.Contains(value);
        }

        private static bool AreThereValidDigits(string value)
        {
            var number = value.Substring(0, CPF_LENGTH - 2);
            var verifyingDigit = new VerifyingDigit(number)
                .WithMultipliersOfUpTo(2, 11)
                .Replacing("0", 10, 11);
            var firstDigit = verifyingDigit.CalculateDigit();
            verifyingDigit.AddDigit(firstDigit);
            var secondDigit = verifyingDigit.CalculateDigit();

            return string.Concat(firstDigit, secondDigit) == value.Substring(CPF_LENGTH - 2, 2);
        }
    }
}

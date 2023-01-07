namespace AppRestAPIBasic.Business.Models.Validations.Documents
{
    public class CNPJValidation
    {
        public const int CNPJ_LENGTH = 14;

        public static bool Validate(string cpnj)
        {
            var cnpjNumbers = Utils.OnlyNumber(cpnj);

            if (!IsLengthValid(cnpjNumbers)) return false;
            return !AreThereRepeatedDigits(cnpjNumbers) && AreThereValidDigits(cnpjNumbers);
        }

        private static bool IsLengthValid(string value)
        {
            return value.Length == CNPJ_LENGTH;
        }

        private static bool AreThereRepeatedDigits(string value)
        {
            string[] invalidNumbers =
            {
                "00000000000000",
                "11111111111111",
                "22222222222222",
                "33333333333333",
                "44444444444444",
                "55555555555555",
                "66666666666666",
                "77777777777777",
                "88888888888888",
                "99999999999999"
            };
            return invalidNumbers.Contains(value);
        }

        private static bool AreThereValidDigits(string value)
        {
            var number = value.Substring(0, CNPJ_LENGTH - 2);

            var verifyingDigit = new VerifyingDigit(number)
                .WithMultipliersOfUpTo(2, 9)
                .Replacing("0", 10, 11);
            var firstDigit = verifyingDigit.CalculateDigit();
            verifyingDigit.AddDigit(firstDigit);
            var secondDigit = verifyingDigit.CalculateDigit();

            return string.Concat(firstDigit, secondDigit) == value.Substring(CNPJ_LENGTH - 2, 2);
        }
    }
}

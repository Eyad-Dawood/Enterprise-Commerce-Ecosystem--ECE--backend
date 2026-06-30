namespace ECE.Domain.Common.Utilities.Normalization;

public static class ArabicNormalizer
{
    public static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return value
            .Trim()

            
            .Replace('أ', 'ا')
            .Replace('إ', 'ا')
            .Replace('آ', 'ا')

            .Replace('ى', 'ي')
            .Replace('ئ', 'ي')

            .Replace('ؤ', 'و')

            .Replace('ة', 'ه')

            .Replace("ـ", "")

            .Replace("\u064B", "") // Fathatan
            .Replace("\u064C", "") // Dammatan
            .Replace("\u064D", "") // Kasratan
            .Replace("\u064E", "") // Fatha
            .Replace("\u064F", "") // Damma
            .Replace("\u0650", "") // Kasra
            .Replace("\u0651", "") // Shadda
            .Replace("\u0652", "") // Sukun
            .Replace("\u0670", ""); // Superscript Alef
    }
}
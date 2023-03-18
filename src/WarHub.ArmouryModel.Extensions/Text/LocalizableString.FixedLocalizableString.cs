namespace WarHub.ArmouryModel.Source;

public abstract partial class LocalizableString
{
    private sealed class FixedLocalizableString : LocalizableString
    {
        /// <summary>
        /// FixedLocalizableString representing an empty string.
        /// </summary>
        private static readonly FixedLocalizableString empty = new(string.Empty);

        private readonly string fixedString;

        public static FixedLocalizableString Create(string? fixedResource)
        {
            if (string.IsNullOrEmpty(fixedResource))
            {
                return empty;
            }

            return new FixedLocalizableString(fixedResource);
        }

        private FixedLocalizableString(string fixedResource)
        {
            fixedString = fixedResource;
        }

        protected override string GetText(IFormatProvider? formatProvider)
        {
            return fixedString;
        }

        protected override bool AreEqual(object? other)
        {
            return other is FixedLocalizableString fixedStr
                && string.Equals(fixedString, fixedStr.fixedString, StringComparison.Ordinal);
        }

        protected override int GetHash()
        {
            return fixedString?.GetHashCode(StringComparison.Ordinal) ?? 0;
        }

        internal override bool CanThrowExceptions => false;
    }
}

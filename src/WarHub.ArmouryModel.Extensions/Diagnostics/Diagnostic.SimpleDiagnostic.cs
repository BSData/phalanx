namespace WarHub.ArmouryModel.Source;

public abstract partial class Diagnostic
{
    internal sealed class SimpleDiagnostic : Diagnostic
    {
        private readonly object?[] messageArgs;

        private SimpleDiagnostic(
            DiagnosticDescriptor descriptor,
            DiagnosticSeverity severity,
            int warningLevel,
            Location location,
            IEnumerable<Location>? additionalLocations,
            object?[]? messageArgs,
            ImmutableDictionary<string, string?>? properties,
            bool isSuppressed)
        {
            if ((warningLevel == 0 && severity != DiagnosticSeverity.Error) ||
                (warningLevel != 0 && severity == DiagnosticSeverity.Error))
            {
                throw new ArgumentException($"{nameof(warningLevel)} ({warningLevel}) and {nameof(severity)} ({severity}) are not compatible.", nameof(warningLevel));
            }

            Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
            Severity = severity;
            WarningLevel = warningLevel;
            Location = location ?? Location.None;
            AdditionalLocations = additionalLocations is null ? Array.Empty<Location>() : additionalLocations.ToImmutableArray<Location>();
            this.messageArgs = messageArgs ?? Array.Empty<object?>();
            Properties = properties ?? ImmutableDictionary<string, string?>.Empty;
            IsSuppressed = isSuppressed;
        }

        internal static SimpleDiagnostic Create(
            DiagnosticDescriptor descriptor,
            DiagnosticSeverity severity,
            int warningLevel,
            Location location,
            IEnumerable<Location>? additionalLocations,
            object?[]? messageArgs,
            ImmutableDictionary<string, string?>? properties,
            bool isSuppressed = false)
        {
            return new SimpleDiagnostic(descriptor, severity, warningLevel, location, additionalLocations, messageArgs, properties, isSuppressed);
        }

        internal static SimpleDiagnostic Create(
            string id,
            LocalizableString title,
            string category,
            LocalizableString message,
            LocalizableString description,
            string helpLink,
            DiagnosticSeverity severity,
            DiagnosticSeverity defaultSeverity,
            bool isEnabledByDefault,
            int warningLevel,
            Location location,
            IEnumerable<Location>? additionalLocations,
            IEnumerable<string>? customTags,
            ImmutableDictionary<string, string?>? properties,
            bool isSuppressed = false)
        {
            var descriptor = new DiagnosticDescriptor(id, title, message,
                 category, defaultSeverity, isEnabledByDefault, description, helpLink, customTags?.ToImmutableArray() ?? ImmutableArray<string>.Empty);
            return new SimpleDiagnostic(descriptor, severity, warningLevel, location, additionalLocations, messageArgs: null, properties: properties, isSuppressed: isSuppressed);
        }

        public override DiagnosticDescriptor Descriptor { get; }

        public override string Id => Descriptor.Id;

        public override string GetMessage(IFormatProvider? formatProvider = null)
        {
            if (messageArgs.Length == 0)
            {
                return Descriptor.MessageFormat.ToString(formatProvider);
            }

            var localizedMessageFormat = Descriptor.MessageFormat.ToString(formatProvider);

            try
            {
                return string.Format(formatProvider, localizedMessageFormat, messageArgs);
            }
            catch (Exception)
            {
                // Analyzer reported diagnostic with invalid format arguments, so just return the unformatted message.
                return localizedMessageFormat;
            }
        }

        public override DiagnosticSeverity Severity { get; }

        public override bool IsSuppressed { get; }

        public override int WarningLevel { get; }

        public override Location Location { get; }

        public override IReadOnlyList<Location> AdditionalLocations { get; }

        public override ImmutableDictionary<string, string?> Properties { get; }

        public override bool Equals(Diagnostic? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not SimpleDiagnostic other)
            {
                return false;
            }

            return Descriptor.Equals(other.Descriptor)
                && messageArgs.SequenceEqual(other.messageArgs, ReferenceEqualityComparer.Instance)
                && Location == other.Location
                && Severity == other.Severity
                && WarningLevel == other.WarningLevel;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Diagnostic);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Descriptor,
                messageArgs.Aggregate(
                    new HashCode(),
                    (acc, item) => { acc.Add(item); return acc; },
                    x => x.ToHashCode()),
                WarningLevel,
                Location,
                Severity);
        }

        internal override Diagnostic WithLocation(Location location)
        {
            ArgumentNullException.ThrowIfNull(location);

            if (Location != location)
            {
                return new SimpleDiagnostic(Descriptor, Severity, WarningLevel, location, AdditionalLocations, messageArgs, Properties, IsSuppressed);
            }

            return this;
        }

        internal override Diagnostic WithSeverity(DiagnosticSeverity severity)
        {
            if (Severity != severity)
            {
                var warningLevel = GetDefaultWarningLevel(severity);
                return new SimpleDiagnostic(Descriptor, severity, warningLevel, Location, AdditionalLocations, messageArgs, Properties, IsSuppressed);
            }

            return this;
        }

        internal override Diagnostic WithIsSuppressed(bool isSuppressed)
        {
            if (IsSuppressed != isSuppressed)
            {
                return new SimpleDiagnostic(Descriptor, Severity, WarningLevel, Location, AdditionalLocations, messageArgs, Properties, isSuppressed);
            }

            return this;
        }
    }
}

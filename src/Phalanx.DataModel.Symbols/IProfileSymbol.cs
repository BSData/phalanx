namespace Phalanx.DataModel.Symbols
{
    public interface IProfileSymbol : IResourceEntrySymbol
    {
        /// <summary>
        /// The type of profile that defines name and characteristic types for this profile.
        /// </summary>
        new IProfileTypeSymbol Type { get; }
    }
}

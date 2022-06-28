namespace WarHub.ArmouryModel.Concrete;

/// <summary>
/// This enum describes the types of components that could give
/// us diagnostics.  We shouldn't read the list of diagnostics
/// until all of these types are accounted for.
/// From: https://sourceroslyn.io/#Microsoft.CodeAnalysis.CSharp/Symbols/CompletionPart.cs
/// </summary>
[Flags]
internal enum CompletionPart
{
    None = 0,

    StartBindingReferences = 1 << 0,
    FinishBindingReferences = 1 << 1,
    ReferencesCompleted = StartBindingReferences | FinishBindingReferences,
    Members = 1 << 2,
    MembersCompleted = 1 << 3,

#pragma warning disable CA1069 // The enum member has the same constant value as member
    All = (1 << 4) - 1,

    // source symbol

    SourceDeclaredSymbolAll = ReferencesCompleted | Members | MembersCompleted,

    // global namespace

    NamespaceAll = Members | MembersCompleted,
#pragma warning restore
}

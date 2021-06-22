namespace Phalanx.Tool.Editor
{
    public static class RosterOperations
    {
        public static IRosterOperation Identity { get; } = new LambdaOperation(x => x);
    }
}

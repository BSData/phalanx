namespace WarHub.ArmouryModel.SourceAnalysis
{
    internal class SimpleReferenceErrorInfo : ReferenceErrorInfo
    {
        public SimpleReferenceErrorInfo(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public override string GetMessage() => Message;
    }
}

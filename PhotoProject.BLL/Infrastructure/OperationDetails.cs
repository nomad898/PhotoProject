namespace PhotoProject.BLL.Infrastructure
{
    public class OperationDetails
    {
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }

        public OperationDetails(bool succedeed,
            string message,
            string property)
        {
            Succeeded = succedeed;
            Message = message;
            Property = property;
        }
    }
}

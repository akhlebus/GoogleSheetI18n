namespace GoogleSheetI18n.Core.Models.Validation
{
    public class ErrorModel
    {
        public ErrorModel(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}

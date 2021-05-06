namespace GoogleSheetI18n.Api.Validation
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

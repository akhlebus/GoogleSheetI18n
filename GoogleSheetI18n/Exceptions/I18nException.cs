using System;

namespace GoogleSheetI18n.Api.Exceptions
{
    public class I18nException : Exception
    {
        public I18nException(string message) : base(message)
        {
        }

        public I18nException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
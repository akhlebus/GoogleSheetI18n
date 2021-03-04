using NUnit.Framework;

namespace GoogleSheetI18n.Api.Tests.Core
{
    public static class TestContextParams
    {
        public static string SpreadSheetId => TestContext.Parameters[nameof(SpreadSheetId)];
    }
}
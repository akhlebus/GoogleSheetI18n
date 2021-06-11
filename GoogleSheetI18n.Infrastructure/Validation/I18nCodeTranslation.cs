using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GoogleSheetI18n.Common.Extensions;
using GoogleSheetI18n.Core.Models.Validation;
using GoogleSheetI18n.Infrastructure.Core;
using Newtonsoft.Json;

namespace GoogleSheetI18n.Infrastructure.Validation
{
    public class I18nCodeTranslation : II18nCodeTranslation
    {
        public string GetExtractedTranslations(string pageName)
        {
            var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "GoogleSheetI18n.UI", "extractedTranslations", "en", pageName + ".json");
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                return text;
            }

            return string.Empty;
        }

        public ErrorModel Validate(IList<I18nSheet> i18nSheets)
        {
            List<string> warnings = new List<string>();
            foreach (var sheet in i18nSheets)
            {
                if (sheet != null)
                {
                    var text = GetExtractedTranslations(sheet.SheetName.FromPascalCaseToDashLowerCase());

                    if (!string.IsNullOrEmpty(text))
                    {
                        var possibleKeys = sheet.ValueRange.Values.Skip(1).Select(x => x.FirstOrDefault()).ToList();
                        var validationErrors = GetValidationError(text, possibleKeys);
                        warnings.Add(validationErrors);
                    }


                    var languages = sheet.GetSupportedLanguages();
                    warnings.AddRange(languages.Warnings);

                    foreach (var lng in languages)
                    {
                        warnings.AddRange(sheet.GetTranslations(lng).Warnings);

                    }
                }
            }

            return new ErrorModel(CreateErrorMessage(warnings));
        }

        private string GetValidationError(string text, List<object> possibleKeys)
        {
            var keyDictionary = DeserializeJson(text);

            var keys = keyDictionary.Keys.ToList();
            var keysThatAreNotInTheTemplate = possibleKeys.Except(keys);
            var keysThatAreNotInTheDatabase = keys.Except(possibleKeys);


            var strBuilder = new StringBuilder();

            foreach (var item in keysThatAreNotInTheTemplate)
            {
                strBuilder.Append($"'{item}' key is in the database but not in the template. ");
            }

            foreach (var item in keysThatAreNotInTheDatabase)
            {
                strBuilder.Append($"'{item}' key is in the template but not in the database. ");
            }
            return strBuilder.ToString();
        }

        private Dictionary<string, object> DeserializeJson(string json)
        {
            var sourceJObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            while (sourceJObject.Any(x => x.Value.ToString() != string.Empty))
            {
                var invalidJObjects = sourceJObject.Where(x => x.Value.ToString() != string.Empty).ToArray();

                for (int i = 0; i < invalidJObjects.Length; i++)
                {
                    var jObjects = JsonConvert.DeserializeObject<Dictionary<string, object>>(invalidJObjects[i].Value.ToString());
                    foreach (var x in jObjects)
                    {
                        sourceJObject.Add($"{invalidJObjects[i].Key}.{x.Key}", x.Value);

                    }
                    sourceJObject.Remove(invalidJObjects[i].Key);
                }
            }
            return sourceJObject;
        }

        private string CreateErrorMessage(List<string> warnings)
        {
            var uniqueWarnings = warnings.Distinct();
            var strBuilder = new StringBuilder();

            foreach (var item in uniqueWarnings)
            {
                strBuilder.Append(item);
            }
            return strBuilder.ToString();
        }
    }
}

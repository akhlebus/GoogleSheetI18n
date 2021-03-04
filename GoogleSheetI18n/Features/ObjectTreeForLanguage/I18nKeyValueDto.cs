﻿using GoogleSheetI18n.Api.Core;

namespace GoogleSheetI18n.Api.Dtos
{
    public readonly struct I18nKeyValueDto
    {
        public string Path { get; }
        public string[] ScopeSegments { get; }
        public string KeySegment { get; }
        public string Value { get; }

        public I18nKeyValueDto(object path, object value)
        {
            Path = path.ToString();
            Value = value.ToString();

            var segments = I18nPath.GetPathSegments(Path);
            ScopeSegments = segments.ScopeSegments;
            KeySegment = segments.KeySegment;
        }
    }
}
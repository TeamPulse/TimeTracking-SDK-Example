using System;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Telerik.TeamPulse.Sdk.Common
{
    /// <summary>
    /// Contains common extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts a Name/Value collection to HTTP query string format.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="startWithQuestionMark">if set to <c>true</c> appends question mark at the beginning of the string.</param>
        /// <param name="urlEncode">if set to <c>true</c> the values are URL encoded before appending to the query string.</param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection collection, bool startWithQuestionMark = true, bool urlEncode = false)
        {
            var leadingChar = startWithQuestionMark ? "?" : null;
            return ToQueryString(collection, leadingChar, urlEncode);
        }

        /// <summary>
        /// Converts a Name/Value collection to HTTP query string format.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="leadingChar">The leading char.</param>
        /// <param name="urlEncode">if set to <c>true</c> the values are URL encoded before appending to the query string.</param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection collection, string leadingChar, bool urlEncode = false)
        {
            if (collection == null || !collection.HasKeys())
                return String.Empty;

            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(leadingChar))
                sb.Append(leadingChar);

            var j = 0;
            var keys = collection.Keys;
            foreach (string key in keys)
            {
                var i = 0;
                var values = collection.GetValues(key);
                foreach (var value in values)
                {
                    sb.Append(key)
                      .Append("=");

                    if (urlEncode)
                        sb.Append(HttpUtility.UrlEncode(value));
                    else
                        sb.Append(value);

                    if (++i < values.Length)
                        sb.Append("&");
                }
                if (++j < keys.Count)
                    sb.Append("&");
            }
            return sb.ToString();
        }

        public static NameValueCollection ParseNameValueCollection(string source, string regExPattern)
        {
            var coll = new NameValueCollection();
            var regEx = new Regex(regExPattern);
            var match = regEx.Match(source);
            while (match.Success)
            {
                var nameGroup = match.Groups["Name"];
                var valueGroup = match.Groups["Value"];
                if (nameGroup.Success)
                    coll[nameGroup.Value] = valueGroup.Value;
                match = match.NextMatch();
            }
            return coll;
        }

        public static class NameValueRegexPatterns
        {
            /// <summary>
            /// Example: category="My Category" item="Some Item"
            /// </summary>
            public const string EqualitySignAndQuotationMarks = @"(?<Name>\w+)\s*=\s*""\s*(?<Value>.*?)""";

            /// <summary>
            /// Example: category: My Category; item: Some Item;
            /// </summary>
            public const string ColonAndSemicolon = @"(?<Name>\w+)\s*:\s*(?<Value>.*?);";
        }
    }
}

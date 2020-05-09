using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ICalParser.Models
{
    public class Calendar
    {
        public CalendarEvent[] Events { get; }

        public Calendar(string source)
        {
            Events = GetEvents(source).ToArray();
        }

        private static IDictionary<string, string> ParseVEventFields(string source)
        {
            const string contentLinePattern = @"(?<field>.+?):(?<value>.+?)(?=\s[A-Z-]{3,}:|$)";
            const RegexOptions contentLineTRegexOptions = RegexOptions.Singleline;

            source = Regex.Unescape(source)
                          .Replace(Environment.NewLine, "\n")
                          .Replace("\r\n", "\n")
                          .Replace("\n\tn", "\n")
                          .Replace("\n\t", string.Empty);

            var matches = Regex.Matches(source, contentLinePattern, contentLineTRegexOptions);

            var eventDictionary = new Dictionary<string, string>();

            foreach(Match match in matches)
            {
                eventDictionary.Add(match.Groups["field"].ToString().Trim(), match.Groups["value"].ToString().Trim());
            }

            return eventDictionary;
        }

        private static IEnumerable<CalendarEvent> GetEvents(string source)
        {
            const string vEventContentPattern = @"BEGIN:VEVENT\s(?<vevent>.+?)\sEND:VEVENT";
            const RegexOptions vEventContentRegexOptions = RegexOptions.Singleline;

            var contentMatch = Regex.Matches(source, vEventContentPattern, vEventContentRegexOptions);

            foreach(Match match in contentMatch)
            {
                var field = ParseVEventFields(match.Groups["vevent"].ToString());

                var startDate = DateTime.ParseExact(field["DTSTART"], "yyyyMMddTHHmmssZ", CultureInfo.CurrentCulture);
                var endDate = DateTime.ParseExact(field["DTEND"], "yyyyMMddTHHmmssZ", CultureInfo.CurrentCulture);
                yield return new CalendarEvent(field["UID"], startDate, endDate, field["DESCRIPTION"],
                                               field["LOCATION"], field["SUMMARY"]);
            }
        }
    }
}
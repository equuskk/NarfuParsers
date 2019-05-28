using System;
using NarfuParsers.Parsers;

namespace NarfuParsers
{
    public class ParserService
    {
        public GroupsParser Groups { get; }
        public TeachersParser Teachers { get; }
        public SchoolsParser Schools { get; }

        public ParserService(TimeSpan timeout)
        {
            Groups = new GroupsParser(timeout);
            Teachers = new TeachersParser(timeout);
            Schools = new SchoolsParser(timeout);
        }
    }
}
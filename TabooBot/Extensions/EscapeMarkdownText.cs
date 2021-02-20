using System.Text.RegularExpressions;

namespace TabooBot.Extensions
{
    public static class EscapeMarkdownText
    {
        private const string markdownV2Pattern = @"(\[[^\][]*]\(http[^()]*\))|[_*[\]()~>#+=|{}.!-]";

        public static string EscapeMarkdownV2Characters(this string text)
        {
            return Regex.Replace(text, markdownV2Pattern, m => string.Format(@"\{0}", m.Value));
        }
    }
}
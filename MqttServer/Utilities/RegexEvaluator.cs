using System.Text.RegularExpressions;

namespace MqttServer
{
    public class RegexEvaluator
    {

        public static bool Evaluate(string pattern, string text)
        {
            var regex = new Regex(pattern);
            var matchedText = regex.Match(text);
            return matchedText.Success;
        }
    }
}

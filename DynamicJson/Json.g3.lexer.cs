using Antlr.Runtime;
using System.Text;
namespace DynamicJson
{
    public partial class JsonLexer
    {
        public override void DisplayRecognitionError(
            string[] tokenNames, 
            RecognitionException e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("There was a problem with the json input during syntactical analysis and it cannot be parsed. Here is some more info:");
            sb.AppendLine("----");

            sb.AppendLine(e.Message);
            sb.AppendLine("region: " + e.Input.ToString());
            sb.AppendLine("line: " + e.Line);
            sb.AppendLine("col: " + e.CharPositionInLine);

            throw new JsonException(sb.ToString(), e);
        }
    }
}

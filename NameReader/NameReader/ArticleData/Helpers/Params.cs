using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NameReader.ArticleData.Helpers
{
    /// <summary>
    /// Reads the embedded XML files HTMLContent.xml or TextContent.xml for the Enlighten(string licenseID, string content, string paramsXML) call
    /// </summary>
    public static class Params
    {
        /// <summary>
        /// params for HTML
        /// </summary>
        /// <returns></returns>
        public static string GetHTMLParamsXML()
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            StreamReader _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("NameReader.OpenCalaisParams.HTMLContent.xml"));
            return _textStreamReader.ReadToEnd();
        }

        /// <summary>
        /// params for text
        /// </summary>
        /// <returns></returns>
        public static string GetTextParamsXML()
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            StreamReader _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("NameReader.OpenCalaisParams.TextContent.xml"));
            return _textStreamReader.ReadToEnd();
        }
    }
}

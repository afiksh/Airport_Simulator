#region using
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;
#endregion

namespace DLL.Attributes
{
    /// <summary>
    /// Create text file to conatin all flights information
    /// </summary>
    public class AirplanesTrackAttribute : ActionFilterAttribute
    {
        #region Fields
        string path = "";
        #endregion

        #region Constructors
        public AirplanesTrackAttribute(string fileName) =>
            path = (FileNameTxt(ref fileName) != "") ?
                $"{Environment.CurrentDirectory}\\{fileName}.txt"
                : "";
        #endregion

        #region Methods

        #region Override
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (path != "" && !File.Exists(path)) File.Create(path);
        }
        #endregion

        #region Private
        /// <summary>
        /// Get the <paramref name="fileName"/>, check it and return the <paramref name="fileName"/> by <see cref="Regex.Match(string, string)"/>
        /// </summary>
        /// <param name="fileName"></param>
        private string FileNameTxt(ref string fileName)
        {
            try
            {
                fileName = Regex.Matches(fileName, @"[\p{L}\p{N}]+").First().ToString();
            }
            catch
            {
                fileName = "";
            }
            return fileName;
        }
        #endregion

        #endregion
    }

}

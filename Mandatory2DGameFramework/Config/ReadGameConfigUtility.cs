using Mandatory2DGameFramework.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mandatory2DGameFramework.Config
{
    /// <summary>
    /// Provides utility methods for reading game configuration from an XML file.
    /// </summary>
    public static class ReadGameConfigUtility
    {
        /// <summary>
        /// Reads configuration from an XML file. Falls back to defaults on error.
        /// </summary>
        /// <param name="fullFileName">The full path to the XML configuration file.</param>
        /// <returns>A <see cref="GameConfig"/> object populated with the configuration values.</returns>
        // cleaned up version
        public static GameConfig ReadConfig(string fullFileName)
        {
            var cfg = GameConfig.Instance;

            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(fullFileName);

                var root = xmlDocument.DocumentElement;
                if (root == null) return cfg;

                if (int.TryParse(root.SelectSingleNode("MaxX")?.InnerText.Trim(), out var maxX))
                    cfg.Width = maxX;

                if (int.TryParse(root.SelectSingleNode("MaxY")?.InnerText.Trim(), out var maxY))
                    cfg.Hight = maxY;

                var difficultyText = root.SelectSingleNode("Difficulty")?.InnerText.Trim();
                if (difficultyText != null &&
                    Enum.TryParse<GameConfig.DifficultyLevel>(difficultyText, true, out var level))
                    cfg.Difficulty = level;

                cfg.Validate();
            }
            catch
            {
                // Keep defaults silently or log if desired
                GameLogger.Instance.LogWarning("Config load failed; using defaults.");
            }

            return cfg;
        }



        ////TODO "clean" koden da den er lidt rodet
        //public static GameConfig ReadConfig(string fullFileName)
        //{
        //    var cfg = GameConfig.Instance;

        //    var xmlDocument = new XmlDocument();
        //    xmlDocument.Load(fullFileName);

        //    var root = xmlDocument.DocumentElement;
        //    if (root == null) return cfg;

        //    XmlNode? maxXNode = root.SelectSingleNode("MaxX");
        //    if (maxXNode != null && int.TryParse(maxXNode.InnerText.Trim(), out var maxX))
        //    {
        //        cfg.MaxX = maxX;
        //    }

        //    XmlNode? maxYNode = root.SelectSingleNode("MaxY");
        //    if (maxYNode != null && int.TryParse(maxYNode.InnerText.Trim(), out var maxY))
        //    {
        //        cfg.MaxY = maxY;
        //    }

        //    XmlNode? difficultyNode = root.SelectSingleNode("Difficulty");
        //    if (difficultyNode != null)
        //    {
        //        var text = difficultyNode.InnerText.Trim();
        //        if (Enum.TryParse<GameConfig.DifficultyLevel>(text, true, out var level))
        //        {
        //            cfg.Difficulty = level;
        //        }
        //    }

        //    try
        //    {
        //        cfg.Validate();
        //    }
        //    catch
        //    {
        //        // validation failure left to caller or ignored here depending on desired behavior
        //    }

        //    return cfg;
        //}
    }
}

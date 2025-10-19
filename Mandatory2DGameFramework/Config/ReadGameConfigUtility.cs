using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mandatory2DGameFramework.Config
{
    public static class ReadGameConfigUtility
    {
        //TODO
        public static GameConfig ReadConfig(string fullFileName)
        {
            var cfg = GameConfig.Instance;

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fullFileName);

            var root = xmlDocument.DocumentElement;
            if (root == null) return cfg;

            XmlNode? maxXNode = root.SelectSingleNode("MaxX");
            if (maxXNode != null && int.TryParse(maxXNode.InnerText.Trim(), out var maxX))
            {
                cfg.MaxX = maxX;
            }

            XmlNode? maxYNode = root.SelectSingleNode("MaxY");
            if (maxYNode != null && int.TryParse(maxYNode.InnerText.Trim(), out var maxY))
            {
                cfg.MaxY = maxY;
            }

            XmlNode? difficultyNode = root.SelectSingleNode("Difficulty");
            if (difficultyNode != null)
            {
                var text = difficultyNode.InnerText.Trim();
                if (Enum.TryParse<GameConfig.DifficultyLevel>(text, true, out var level))
                {
                    cfg.Difficulty = level;
                }
            }

            try
            {
                cfg.Validate();
            }
            catch
            {
                // validation failure left to caller or ignored here depending on desired behavior
            }

            return cfg;
        }
    }
}

using System;


namespace Mandatory2DGameFramework.Config
{
    public sealed class GameConfig
    {
        //TODO
        //DifficultyLevel kunne eventuelt indføres via en state machine? easy 120hp, medium 100 hp , hard 80 hp?
        public enum DifficultyLevel
        {
            Easy,
            Medium,
            Hard
        }

        private static readonly GameConfig instance = new GameConfig();
        public static GameConfig Instance => instance;

        private GameConfig()
        {
            // default værdier
            Width = 25;
            Hight = 25;
            Difficulty = DifficultyLevel.Medium;
        }

        public int Width { get; set; }
        public int Hight { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        // sikring af at verdenen har en hvis strrelse
        public void Validate()
        {
            if (Width <= 0) throw new InvalidOperationException($"{nameof(Width)} must be > 0");
            if (Hight <= 0) throw new InvalidOperationException($"{nameof(Hight)} must be > 0");
        }

        public override string ToString()
            => $"{{{nameof(Width)}={Width}, {nameof(Hight)}={Hight}, {nameof(Difficulty)}={Difficulty}}}";
    }
}

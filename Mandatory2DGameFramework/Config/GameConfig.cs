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
            MaxX = 25;
            MaxY = 25;
            Difficulty = DifficultyLevel.Medium;
        }

        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        // sikring af at verdenen har en hvis strrelse
        public void Validate()
        {
            if (MaxX <= 0) throw new InvalidOperationException($"{nameof(MaxX)} must be > 0");
            if (MaxY <= 0) throw new InvalidOperationException($"{nameof(MaxY)} must be > 0");
        }

        public override string ToString()
            => $"{{{nameof(MaxX)}={MaxX}, {nameof(MaxY)}={MaxY}, {nameof(Difficulty)}={Difficulty}}}";
    }
}

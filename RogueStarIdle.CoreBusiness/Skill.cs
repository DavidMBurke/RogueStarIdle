namespace RogueStarIdle.CoreBusiness
{
    public class Skill
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }

        public Skill(string name, int level, int xp)
        {
            Name = name;
            Level = level;
            Xp = xp;
        }

        // Levels and XP required for each using formula XP = (lastLvl-1) * 250 * 1.05 ^ Level
        // Hardcoded because math is hard
        List<(int, int)> levelByXpTuple = new List<(int, int)>
        {
            (1, 0),
            (2, 275),
            (3, 578),
            (4, 911),
            (5, 1276),
            (6, 1675),
            (7, 2110),
            (8, 2585),
            (9, 3102),
            (10, 3665),
            (11, 4275),
            (12, 4938),
            (13, 5656),
            (14, 6434),
            (15, 7276),
            (16, 8185),
            (17, 9168),
            (18, 10228),
            (19, 11371),
            (20, 12603),
            (21, 13929),
            (22, 15357),
            (23, 16893),
            (24, 18544),
            (25, 20318)
        };

        public int GetLevelFromXp(int currentXP)
        {
            int level = 0;

            foreach (var levelXpPair in levelByXpTuple)
            {
                if (currentXP >= levelXpPair.Item2)
                {
                    level = levelXpPair.Item1;
                }
                else
                {
                    break;
                }
            }

            return level;
        }
        public int GetXpForNextLevel()
        {
            int xp = 0;
            bool secondToLast = false;
            foreach (var levelXpPair in levelByXpTuple)
            {
                if (Level >= levelXpPair.Item1 || secondToLast == true)
                {
                    if (!secondToLast)
                    {
                        secondToLast = true;
                        continue;
                    }
                    xp = levelXpPair.Item2;
                    if (secondToLast)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return xp;
        }
        public void checkLevel(int level, int Level)
        {
            int checkedLevel = GetLevelFromXp(Level);
            if (checkedLevel > level)
            {
                level = checkedLevel;
            }
        }

    }
}
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

        public event Func<Task> OnChange;
        private async Task NotifyStateChanged()
        {
            if (OnChange == null)
                return;
            await OnChange.Invoke();
        }

        // Levels and XP required for each using formula XP = (lastLvl-1) * 250 * 1.05 ^ Level
        // Hardcoded because math is hard
        List<(int, int)> LevelByXpTuple = new List<(int, int)>
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
            (25, 20318),
            (26, 22222),
            (27, 24267),
            (28, 26460),
            (29, 28812),
            (30, 31334),
            (31, 34035),
            (32, 36928),
            (33, 40025),
            (34, 43340),
            (35, 46886),
            (36, 50678),
            (37, 54732),
            (38, 59065),
            (39, 63695),
            (40, 68639),
            (41, 73919),
            (42, 79556),
            (43, 85571),
            (44, 91989),
            (45, 98835),
            (46, 106135),
            (47, 113918),
            (48, 122214),
            (49, 131055),
            (50, 140475),
            (51, 150509),
            (52, 161195),
            (53, 172574),
            (54, 184687),
            (55, 197581),
            (56, 211301),
            (57, 225900),
            (58, 241431),
            (59, 257950),
            (60, 275517),
            (61, 294197),
            (62, 314055),
            (63, 335164),
            (64, 357598),
            (65, 381438),
            (66, 406768),
            (67, 433677),
            (68, 446260),
            (69, 492618),
            (70, 524855),
            (71, 559085),
            (72, 595426),
            (73, 634003),
            (74, 674949),
            (75, 718404),
            (76, 764518),
            (77, 813447),
            (78, 865358),
            (79, 920426),
            (80, 978838),
            (81, 1040790),
            (82, 1106490),
            (83, 1176158),
            (84, 1250026),
            (85, 1328341),
            (86, 1411362),
            (87, 1499365),
            (88, 1592639),
            (89, 1691493),
            (90, 1796250),
            (91, 1907254),
            (92, 2024868),
            (93, 2149476),
            (94, 2281482),
            (95, 2421314),
            (96, 2569427),
            (97, 2726297),
            (98, 2892431),
            (99, 3068362),
            (100, 3254656),
            (101, 2147483647)
        };

        public int GetLevelFromXp()
        {
            int level = 0;

            foreach (var levelXpPair in LevelByXpTuple)
            {
                if (Xp >= levelXpPair.Item2)
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
            foreach (var levelXpPair in LevelByXpTuple)
            {
                if (levelXpPair.Item1 > Level)
                {
                    return levelXpPair.Item2;
                }
            }
            return 0;
        }

        public void addXp(int xp)
        {
            Xp += xp;
            UpdateLevel();
        }
        public void UpdateLevel()
        {
            int checkedLevel = GetLevelFromXp();
            if (checkedLevel > Level)
            {
                Level = checkedLevel;
            }
            NotifyStateChanged();
        }
        public void LevelUp()
        {
            if (Level == 100)
            {
                return;
            }
            Xp = GetXpForNextLevel();
            UpdateLevel();
        }
    }
}
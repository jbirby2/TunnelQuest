using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Core;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientSettings
    {
        // static stuff

        public static readonly ClientSettings Instance = new ClientSettings();


        // non-static stuff

        public string ChatToken { get; set; }
        public int MaxChatLines { get; set; }
        public int MinFilterItemNameLength { get; set; }
        public Dictionary<string, string> Classes { get; set; }
        public Dictionary<string, string> Races { get; set; }
        public Dictionary<string, string> Aliases { get; set; }

        private ClientSettings()
        {
            this.ChatToken = ChatLogic.CHAT_TOKEN;
            this.MaxChatLines = ChatLogic.MAX_CHAT_LINES;
            this.MinFilterItemNameLength = ItemLogic.MIN_FILTER_ITEMNAME_LENGTH;

            this.Classes = new Dictionary<string, string>();
            this.Classes.Add(ClassCodes.Bard, ClassNames.Bard);
            this.Classes.Add(ClassCodes.Cleric, ClassNames.Cleric);
            this.Classes.Add(ClassCodes.Druid, ClassNames.Druid);
            this.Classes.Add(ClassCodes.Enchanter, ClassNames.Enchanter);
            this.Classes.Add(ClassCodes.Magician, ClassNames.Magician);
            this.Classes.Add(ClassCodes.Monk, ClassNames.Monk);
            this.Classes.Add(ClassCodes.Necromancer, ClassNames.Necromancer);
            this.Classes.Add(ClassCodes.Paladin, ClassNames.Paladin);
            this.Classes.Add(ClassCodes.Ranger, ClassNames.Ranger);
            this.Classes.Add(ClassCodes.Rogue, ClassNames.Rogue);
            this.Classes.Add(ClassCodes.ShadowKnight, ClassNames.ShadowKnight);
            this.Classes.Add(ClassCodes.Shaman, ClassNames.Shaman);
            this.Classes.Add(ClassCodes.Warrior, ClassNames.Warrior);
            this.Classes.Add(ClassCodes.Wizard, ClassNames.Wizard);

            this.Races = new Dictionary<string, string>();
            this.Races.Add(RaceCodes.Barbarian, RaceNames.Barbarian);
            this.Races.Add(RaceCodes.DarkElf, RaceNames.DarkElf);
            this.Races.Add(RaceCodes.Dwarf, RaceNames.Dwarf);
            this.Races.Add(RaceCodes.Erudite, RaceNames.Erudite);
            this.Races.Add(RaceCodes.Gnome, RaceNames.Gnome);
            this.Races.Add(RaceCodes.HalfElf, RaceNames.HalfElf);
            this.Races.Add(RaceCodes.Halfling, RaceNames.Halfling);
            this.Races.Add(RaceCodes.HighElf, RaceNames.HighElf);
            this.Races.Add(RaceCodes.Human, RaceNames.Human);
            this.Races.Add(RaceCodes.Iksar, RaceNames.Iksar);
            this.Races.Add(RaceCodes.Ogre, RaceNames.Ogre);
            this.Races.Add(RaceCodes.Troll, RaceNames.Troll);
            this.Races.Add(RaceCodes.WoodElf, RaceNames.WoodElf);

            this.Aliases = new Dictionary<string, string>();
            using (var context = new TunnelQuestContext())
            {
                var itemLogic = new ItemLogic(context);
                var aliases = itemLogic.GetAliases();
                foreach (var alias in aliases)
                {
                    this.Aliases[alias.AliasText] = alias.ItemName;
                }
            }
        }

    }
}

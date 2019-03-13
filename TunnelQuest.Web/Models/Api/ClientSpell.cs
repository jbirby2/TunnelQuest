using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TunnelQuest.Core.Models;

namespace TunnelQuest.Web.Models.Api
{
    public class ClientSpell
    {
        // static

        public static ClientSpell[] Create(Spell[] spells)
        {
            var result = new ClientSpell[spells.Length];
            for (int i = 0; i < spells.Length; i++)
            {
                result[i] = new ClientSpell(spells[i]);
            }
            return result;
        }


        // non-static

        public string SpellName { get; set; }
        public string IconFileName { get; set; }
        public string Description { get; set; }
        public Dictionary<string, int> Requirements { get; set; }
        public string[] Details { get; set; }
        public string[] Sources { get; set; }

        public ClientSpell()
        {
        }

        public ClientSpell(Spell spell)
        {
            this.SpellName = spell.SpellName;
            this.IconFileName = spell.IconFileName;
            this.Description = spell.Description;

            int i;

            this.Requirements = new Dictionary<string, int>();
            foreach (var requirement in spell.Requirements)
            {
                this.Requirements[requirement.ClassCode] = requirement.RequiredLevel;
            }

            this.Details = new string[spell.EffectDetails.Count];
            i = 0;
            foreach (var detail in spell.EffectDetails)
            {
                this.Details[i] = detail.Text;
                i++;
            }

            this.Sources = new string[spell.Sources.Count];
            i = 0;
            foreach (var source in spell.Sources)
            {
                this.Sources[i] = source.Text;
                i++;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("weapon_skill")]
    public class WeaponSkill
    {
        [Key]
        [Column("weapon_skill_code")]
        public string WeaponSkillCode { get; set; }

    }
}

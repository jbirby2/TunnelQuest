using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("filter_item")]
    public class FilterItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("filter_item_id")]
        public long FilterItemId { get; set; }

        [ForeignKey("Server")]
        [Column("server_code")]
        public string ServerCode { get; set; }
        public Server Server { get; set; }

        [Required]
        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("alias_text")]
        public string AliasText { get; set; }
    }
}

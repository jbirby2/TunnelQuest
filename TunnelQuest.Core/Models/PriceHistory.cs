using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TunnelQuest.Core.Models
{
    [Table("price_history")]
    public class PriceHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("price_history_id")]
        public long PriceHistoryId { get; set; }

        [Required]
        [ForeignKey("Server")]
        [Column("server_code")]
        public string ServerCode { get; set; }
        public Server Server { get; set; }

        // ItemName is intentionally NOT declared a ForeignKey here, because ItemName
        // might also be a text string that matches no known item (e.g. "jboots mq")
        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("one_month_median")]
        public int? OneMonthMedian { get; set; }

        [Column("three_month_median")]
        public int? ThreeMonthMedian { get; set; }

        [Column("six_month_median")]
        public int? SixMonthMedian { get; set; }

        [Column("twelve_month_median")]
        public int? TwelveMonthMedian { get; set; }

        [Column("lifetime_median")]
        public int LifetimeMedian { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

    }
}

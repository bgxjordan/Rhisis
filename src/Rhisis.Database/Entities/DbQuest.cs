using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    [Table("Quests")]
    public sealed class DbQuest : DbEntity
    {
        /// <summary>
        /// Gets or sets the quest id.
        /// </summary>
        [Column]
        [Required]
        public int QuestId { get; set; }

        /// <summary>
        /// Gets or sets the character id the quest belongs to.
        /// </summary>
        [Column]
        [Required]
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets or sets the character the quest belongs to.
        /// </summary>
        [ForeignKey(nameof(CharacterId))]
        public DbCharacter Character { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the quest is finished.
        /// </summary>
        [Column(TypeName = "BIT")]
        public bool Finished { get; set; }
    }
}

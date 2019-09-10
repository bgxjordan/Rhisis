namespace Rhisis.Core.Structures.Game.Quests
{
    /// <summary>
    /// Defines the quest data.
    /// </summary>
    public sealed class QuestData
    {
        /// <summary>
        /// Gets the quest id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the quest title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Creates a new <see cref="QuestData"/> instance.
        /// </summary>
        /// <param name="id">Quest id.</param>
        /// <param name="title">Quest title.</param>
        public QuestData(int id, string title)
        {
            this.Id = id;
            this.Title = title;
        }

        /// <summary>
        /// Display the <see cref="QuestData"/> title.
        /// </summary>
        /// <returns><see cref="QuestData"/> title.</returns>
        public override string ToString() => this.Title;
    }
}

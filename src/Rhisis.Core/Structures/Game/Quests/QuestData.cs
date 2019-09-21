using Rhisis.Core.Structures.Game.Dialogs;
using System.Collections.Generic;

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
        /// Gets the quest name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the quest title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the quest link.
        /// </summary>
        public DialogLink Link { get; internal set; }

        /// <summary>
        /// Gets the quest begin texts.
        /// </summary>
        public IEnumerable<string> BeginTexts { get; internal set; }

        /// <summary>
        /// Gets the quest end texts when the quest is completed.
        /// </summary>
        public IEnumerable<string> EndCompleteTexts { get; internal set; }

        /// <summary>
        /// Gets the quest end texts when the quest is not completed.
        /// </summary>
        public IEnumerable<string> EndFailureTexts { get; internal set; }

        /// <summary>
        /// Gets the quest text when the player accepts the quest.
        /// </summary>
        public string AcceptedText { get; internal set; }

        /// <summary>
        /// Gets the quest text when the player declines the quest.
        /// </summary>
        public string DeclineText { get; internal set; }

        /// <summary>
        /// Gets the quest start character.
        /// </summary>
        public string StartCharacter { get; internal set; }

        /// <summary>
        /// Gets the quest end character.
        /// </summary>
        public string EndCharacter { get; internal set; }

        /// <summary>
        /// Gets the quest min level to start.
        /// </summary>
        public int MinLevel { get; internal set; }

        /// <summary>
        /// Gets the quest max level to start.
        /// </summary>
        public int MaxLevel { get; internal set; }

        /// <summary>
        /// Gets the previous quest type.
        /// </summary>
        public int PreviousQuestType { get; internal set; }

        /// <summary>
        /// Gets the quest id of the previous quest.
        /// </summary>
        public int PreviousQuestId { get; internal set; }

        /// <summary>
        /// Creates a new <see cref="QuestData"/> instance.
        /// </summary>
        /// <param name="id">Quest id.</param>
        /// <param name="name">Quest name.</param>
        /// <param name="title">Quest title.</param>
        public QuestData(int id, string name, string title)
        {
            this.Id = id;
            this.Name = name;
            this.Title = title;
        }

        /// <summary>
        /// Display the <see cref="QuestData"/> title.
        /// </summary>
        /// <returns><see cref="QuestData"/> title.</returns>
        public override string ToString() => this.Title;
    }
}

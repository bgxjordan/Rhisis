namespace Rhisis.Core.Structures.Game.Quests
{
    public sealed class QuestConstants
    {
        public const string QuestSuggestState = "QUEST_SUGGEST";
        public const string QuestBeginYesState = "QUEST_BEGIN_YES";
        public const string QuestBeginNoState = "QUEST_BEGIN_NO";
        public const string QuestEndState = "QUEST_END";
        public const string QuestEndCompleteState = "QUEST_END_COMPLETE";
    }

    public enum QuestStateType
    {
        Unknown,
        Suggest,
        BeginYes,
        BeginNo,
        End,
        EndCompleted
    }
}

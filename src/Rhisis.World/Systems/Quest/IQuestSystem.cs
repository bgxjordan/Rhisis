using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Quest
{
    public interface IQuestSystem
    {
        void Initialize(IPlayerEntity player);

        bool CanStartQuest(IPlayerEntity player, QuestData quest);

        void ProcessQuest(IPlayerEntity player, INpcEntity npc, QuestData quest, QuestStateType state);
    }
}

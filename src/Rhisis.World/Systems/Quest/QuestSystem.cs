using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rhisis.World.Systems.Quest
{
    [Injectable]
    public sealed class QuestSystem : IQuestSystem
    {
        private readonly ILogger<QuestSystem> _logger;
        private readonly INpcDialogPacketFactory _npcDialogPacketFactory;

        public QuestSystem(ILogger<QuestSystem> logger, INpcDialogPacketFactory npcDialogPacketFactory)
        {
            this._logger = logger;
            this._npcDialogPacketFactory = npcDialogPacketFactory;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            // TODO: Initialize player quests.
        }

        /// <inheritdoc />
        public bool CanStartQuest(IPlayerEntity player, QuestData quest)
        {
            if (player.Object.Level < quest.MinLevel || player.Object.Level > quest.MaxLevel)
                return false;

            return true;
        }

        /// <inheritdoc />
        public void ProcessQuest(IPlayerEntity player, INpcEntity npc, QuestData quest, QuestStateType state)
        {
            switch (state)
            {
                case QuestStateType.Suggest:
                    this._logger.LogDebug($"Suggest quest '{quest.Title}' to '{player}'.");
                    this.SuggestQuest(player, npc, quest);
                    break;
                default:
                    this._logger.LogError($"Received unknown dialog quest state: {state}.");
                    break;
            }
        }

        /// <summary>
        /// Suggest a quest to the current player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Quest to suggest.</param>
        private void SuggestQuest(IPlayerEntity player, INpcEntity npc, QuestData quest)
        {
            var dialogLinks = new List<DialogLink>(npc.Data.Dialog.Links);
            dialogLinks.AddRange(npc.Quests.Where(x => this.CanStartQuest(player, x)).Select(x => x.Link));

            var questAnswersButtons = new List<DialogLink>
            {
                new DialogLink(QuestStateType.BeginYes.ToString(), DialogConstants.Yes, quest.Id),
                new DialogLink(QuestStateType.BeginNo.ToString(), DialogConstants.No, quest.Id)
            };

            this._npcDialogPacketFactory.SendDialog(player, quest.BeginTexts, dialogLinks, questAnswersButtons, quest.Id);
        }
    }
}

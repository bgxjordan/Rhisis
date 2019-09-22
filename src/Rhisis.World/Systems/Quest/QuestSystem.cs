using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

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
            {
                this._logger.LogWarning($"Cannot start quest '{quest.Title}' (id: '{quest.Id}') for player: '{player}'. Level too low or too high.");
                return false;
            }

            if (quest.Jobs != null && !quest.Jobs.Contains((DefineJob.Job)player.PlayerData.JobId))
            {
                this._logger.LogWarning($"Cannot start quest '{quest.Title}' (id: '{quest.Id}') for player: '{player}'. Invalid job.");
                return false;
            }

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
                case QuestStateType.BeginYes:
                    this.AcceptQuest(player, npc, quest);
                    break;
                case QuestStateType.BeginNo:
                    this.DeclineQuest(player, npc, quest);
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

        /// <summary>
        /// Accepts a quest.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Quest to accept.</param>
        private void AcceptQuest(IPlayerEntity player, INpcEntity npc, QuestData quest)
        {
            var dialogLinks = new List<DialogLink>(npc.Data.Dialog.Links);
            dialogLinks.AddRange(npc.Quests.Where(x => this.CanStartQuest(player, x)).Select(x => x.Link));

            var questAnswersButtons = new List<DialogLink>
            {
                new DialogLink(DialogConstants.Bye, DialogConstants.Ok)
            };

            this._npcDialogPacketFactory.SendDialog(player, new[] { quest.AcceptedText }, dialogLinks, questAnswersButtons, quest.Id);

            // TODO: add quest to player's diary
        }

        /// <summary>
        /// Declines a quest suggestion.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holding the quest.</param>
        /// <param name="quest">Declined quest.</param>
        private void DeclineQuest(IPlayerEntity player, INpcEntity npc, QuestData quest)
        {
            var dialogLinks = new List<DialogLink>(npc.Data.Dialog.Links);
            dialogLinks.AddRange(npc.Quests.Where(x => this.CanStartQuest(player, x)).Select(x => x.Link));

            var questAnswersButtons = new List<DialogLink>
            {
                new DialogLink(DialogConstants.Bye, DialogConstants.Ok)
            };

            this._npcDialogPacketFactory.SendDialog(player, new[] { quest.DeclineText }, dialogLinks, questAnswersButtons, quest.Id);
        }
    }
}

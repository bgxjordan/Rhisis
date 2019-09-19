using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Core.Resources.Include;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Core.Resources.Loaders
{
    public sealed class QuestLoader : IGameResourceLoader
    {
        private const string QuestFileStartName = "propQuest";
        private const string QuestFileExtension = ".inc";

        private readonly ILogger<QuestLoader> _logger;
        private readonly IMemoryCache _cache;
        private readonly IDictionary<string, int> _defines;
        private readonly IDictionary<string, string> _texts;

        /// <summary>
        /// Creates a new <see cref="QuestLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="cache">Memory cache.</param>
        public QuestLoader(ILogger<QuestLoader> logger, IMemoryCache cache)
        {
            this._logger = logger;
            this._cache = cache;
            this._defines = this._cache.Get<IDictionary<string, int>>(GameResourcesConstants.Defines);
            this._texts = this._cache.Get<IDictionary<string, string>>(GameResourcesConstants.Texts);
        }

        /// <inheritdoc />
        public void Load()
        {
            this._logger.LogLoading("Loading quests...");

            IEnumerable<string> questFiles = from x in Directory.GetFiles(GameResourcesConstants.Paths.DataSub1Path, "*.*", SearchOption.AllDirectories)
                                             where Path.GetFileName(x).StartsWith(QuestFileStartName) && Path.GetExtension(x).Equals(QuestFileExtension)
                                             orderby x.Length
                                             select x;
            var quests = new ConcurrentDictionary<int, QuestData>();

            foreach (string questFilePath in questFiles)
            {
                string fileName = Path.GetFileName(questFilePath);
                int questsLoaded = 0;

                using (var questFile = new IncludeFile(questFilePath, @"([(){}=,;\n\r\t ])"))
                {
                    this._logger.ClearCurrentConsoleLine();

                    foreach (IStatement statement in questFile.Statements)
                    {
                        this._logger.LogLoading($"Loading quests from '{fileName}': {questsLoaded} / {questFile.Statements.Count}");

                        if (statement is Block currentQuestBlock)
                        {
                            var quest = new QuestData(this.ParseQuestId(currentQuestBlock), currentQuestBlock.Name, this.ParseQuestTitle(currentQuestBlock));

                            this.LoadQuestSettings(quest, currentQuestBlock.GetBlockByName("setting"));
                            this.LoadQuestDialogs(quest, currentQuestBlock);

                            quests.TryAdd(quest.Id, quest);
                        }                        

                        questsLoaded++;
                    }
                }
            }

            this._logger.ClearCurrentConsoleLine();
            this._logger.LogInformation($"-> {quests.Count} quests loaded.");
            this._cache.Set(GameResourcesConstants.Quests, quests);
        }

        /// <summary>
        /// Parses the quest id from the current quest block instruction.
        /// </summary>
        /// <param name="currentQuestBlock">Quest block instruction.</param>
        /// <returns>Quest id.</returns>
        private int ParseQuestId(Block currentQuestBlock)
        {
            int questId;

            if (!this._defines.TryGetValue(currentQuestBlock.Name, out questId))
            {
                int.TryParse(currentQuestBlock.Name, out questId);
            }

            return questId;
        }

        /// <summary>
        /// Parses the quest title from the current quest block instruction.
        /// </summary>
        /// <param name="currentQuestBlock">Quest block instruction.</param>
        /// <returns>Quest title.</returns>
        private string ParseQuestTitle(Block currentQuestBlock)
        {
            string questTitleKey = currentQuestBlock.GetInstructionParameter<string>(GameResourcesConstants.QuestInstructions.SetTitle, 0);

            return this._texts.TryGetValue(questTitleKey, out string questTitle) ? questTitle : questTitleKey;
        }

        /// <summary>
        /// Loads the quest settings.
        /// </summary>
        /// <param name="quest">Current quest.</param>
        /// <param name="questSettingsBlock">Quest settings block.</param>
        private void LoadQuestSettings(QuestData quest, Block questSettingsBlock)
        {
            if (questSettingsBlock == null)
            {
                this._logger.ClearCurrentConsoleLine();
                this._logger.LogWarning($"Cannot find quest settings for quest '{quest.Title}'.");
                return;
            }

            quest.StartCharacter = questSettingsBlock.GetInstructionParameter<string>(GameResourcesConstants.QuestInstructions.SetCharacter, 0);
            quest.EndCharacter = questSettingsBlock.GetInstructionParameter<string>(GameResourcesConstants.QuestInstructions.SetEndCharacter, 0);
            quest.MinLevel = questSettingsBlock.GetInstructionParameter<int>(GameResourcesConstants.QuestInstructions.SetBeginLevel, 0);
            quest.MaxLevel = questSettingsBlock.GetInstructionParameter<int>(GameResourcesConstants.QuestInstructions.SetBeginLevel, 1);
            quest.PreviousQuestType = questSettingsBlock.GetInstructionParameter<int>(GameResourcesConstants.QuestInstructions.SetBeginPreviousQuest, 0);

            string previousQuestId = questSettingsBlock.GetInstructionParameter<string>(GameResourcesConstants.QuestInstructions.SetBeginPreviousQuest, 1);
            if (!string.IsNullOrEmpty(previousQuestId) && this._defines.TryGetValue(previousQuestId, out int questId))
            {
                quest.PreviousQuestId = questId;
            }
        }

        /// <summary>
        /// Loads the quest dialogs.
        /// </summary>
        /// <param name="quest">Current quest.</param>
        /// <param name="questBlock">Current quest block.</param>
        private void LoadQuestDialogs(QuestData quest, Block questBlock)
        {
            quest.Link = new DialogLink(QuestStateType.Suggest.ToString(), quest.Title)
            {
                QuestId = quest.Id
            };
        }
    }
}

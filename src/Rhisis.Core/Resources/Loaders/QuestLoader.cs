using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Core.Resources.Include;
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
                            var quest = new QuestData(this.ParseQuestId(currentQuestBlock), this.ParseQuestTitle(currentQuestBlock));

                            // TODO: Load settings

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
            Instruction setTitleInstruction = currentQuestBlock.GetInstruction("SetTitle");

            if (setTitleInstruction == null)
                throw new ArgumentNullException(nameof(setTitleInstruction), "Cannot find quest title definition.");

            if (setTitleInstruction.Parameters.Count < 1)
                throw new ArgumentException("Missing parameters for 'SetTitle' instruction.", nameof(setTitleInstruction.Parameters.Count));

            string questTitleKey = setTitleInstruction.Parameters.ElementAt(0).ToString();

            return this._texts.TryGetValue(questTitleKey, out string questTitle) ? questTitle : questTitleKey;
        }
    }
}

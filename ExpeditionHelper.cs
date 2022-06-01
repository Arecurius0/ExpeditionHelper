using ExileCore;
using ExileCore.PoEMemory.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace ExpeditionHelper
{
    public class ExpeditionHelper : BaseSettingsPlugin<ExpeditionHelperSettings>
    {
        private const string BASENAME_FILE = "Bases.txt";
        private const string REMNANTMODS_FILE_GOOD = "RemnantModsGood.txt";
        private const string REMNANTMODS_FILE_BAD = "RemnantModsBad.txt";
        private List<String> RemnantModifiersGood;
        private List<String> RemnantModifiersBad;
        private HashSet<String> BaseListHS;

        public override bool Initialise()
        {
            Name = "ExpeditionHelper";
            ReadSettingsFiles();
            Settings.RefreshFiles.OnPressed += () => { ReadSettingsFiles(); };
            return base.Initialise();
        }

        private void ReadSettingsFiles()
        {
            var path = $"{DirectoryFullName}\\{BASENAME_FILE}";
            if (File.Exists(path))
            {
                BaseListHS = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")).ToHashSet<String>();
            }
            else CreateSettingsFile(BASENAME_FILE);

            path = $"{DirectoryFullName}\\{REMNANTMODS_FILE_GOOD}";
            if (File.Exists(path))
            {
                RemnantModifiersGood = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")).ToList().ConvertAll(x => x.ToLower());
            }
            else CreateSettingsFile(REMNANTMODS_FILE_GOOD);

            path = $"{DirectoryFullName}\\{REMNANTMODS_FILE_BAD}";
            if (File.Exists(path))
            {
                RemnantModifiersBad = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")).ToList().ConvertAll(x => x.ToLower());
            }
            else CreateSettingsFile(REMNANTMODS_FILE_BAD);
        }

        private void CreateSettingsFile(string file)
        {
            var path = $"{DirectoryFullName}\\{file}";
            if (File.Exists(path)) return;
            using (var streamWriter = new StreamWriter(path, true))
            {
                streamWriter.Write("");
                streamWriter.Close();
            }
        }

        public override void Render()
        {
            if (Settings.GwennenBases &&
                GameController.IngameState.IngameUi.HaggleWindow.IsVisible &&
                (bool)GameController.IngameState.IngameUi.HaggleWindow.GetChildFromIndices(6, 2, 0)?.IsVisible)
            {
                HighlightGwennenBases();
            }
        }
        public void HighlightGwennenBases()
        {
            if (BaseListHS.Count == 0) return;
            var GwennenWindowItems = GameController.IngameState.IngameUi.HaggleWindow.GetChildFromIndices(8, 1, 0, 0);
            for (int i = 1; i < GwennenWindowItems.ChildCount; ++i)
            {
                if (BaseListHS.Contains(GwennenWindowItems.GetChildAtIndex(i).Entity.GetComponent<Base>().Name))
                {
                    Graphics.DrawFrame(GwennenWindowItems.GetChildAtIndex(i).GetClientRect(), Color.Red, Settings.LineThickness);
                }
            }
        }
        public override Job Tick()
        {
            if (GameController.IngameState.IngameUi.ExpeditionDetonatorElement.RemainingExplosives == 0) return null;

            var detonator = GameController.EntityListWrapper.ValidEntitiesByType[ExileCore.Shared.Enums.EntityType.IngameIcon].
                FirstOrDefault(x => x.Path == "Metadata/MiscellaneousObjects/Expedition/ExpeditionDetonator");

            if (detonator != null && 
                detonator.HasComponent<Targetable>() && 
                detonator.GetComponent<Targetable>().isTargeted && 
                GameController.IngameState.IngameUi.ExpeditionDetonatorElement.RemainingExplosives != 0)
            {
                //block left mouseclick
            }
            return null;
        }
    }
}

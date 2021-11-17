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
        private List<String> BasesList;

        public override bool Initialise()
        {
            Name = "ExpeditionHelper";
            ReadBaseFile();
            return base.Initialise();
        }

        private void ReadBaseFile()
        {
            var path = $"{DirectoryFullName}\\{BASENAME_FILE}";
            if (File.Exists(path))
            {
                BasesList = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")).ToList();
            }
            else
                CreateBaseFile();
        }

        private void CreateBaseFile()
        {
            var path = $"{DirectoryFullName}\\{BASENAME_FILE}";
            if (File.Exists(path)) return;
            using (var streamWriter = new StreamWriter(path, true))
            {
                streamWriter.Write("");
                streamWriter.Close();
            }
        }

        public override void Render()
        {
            var GwennenWindowItems = GameController.IngameState.IngameUi.HaggleWindow.GetChildFromIndices(8, 1, 0, 0);
            if (!GameController.IngameState.IngameUi.HaggleWindow.IsVisible) return;
            if ((bool)!GameController.IngameState.IngameUi.HaggleWindow.GetChildFromIndices(6,2,0)?.IsVisible) return;
            for(int i = 1; i < GwennenWindowItems.ChildCount; ++i)
            {
                if (BasesList.Contains(GwennenWindowItems.GetChildAtIndex(i).Entity.GetComponent<Base>().Name))
                {
                    Graphics.DrawFrame(GwennenWindowItems.GetChildAtIndex(i).GetClientRect(), Color.Red, Settings.LineThickness);
                }
            }
        }

        public override Job Tick()
        {
            return base.Tick();
        }
    }
}

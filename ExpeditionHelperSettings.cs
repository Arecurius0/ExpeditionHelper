using System.Windows.Forms;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace ExpeditionHelper
{
    public class ExpeditionHelperSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(false);
        [Menu("Highlight Bases in Gwennen's Gamble Window")]
        public ToggleNode GwennenBases { get; set; } = new ToggleNode(false);
        [Menu("Highlight Remnants")]
        public ToggleNode HighlightRemnants { get; set; } = new ToggleNode(false);
        //[Menu("How thick do you want the lines to be around highlighted Elements")]
        public RangeNode<int> LineThickness { get; set; } = new RangeNode<int>(2, 1, 5);
        public ButtonNode RefreshFiles { get; set; } = new ButtonNode();
    }
}

using System.Windows.Forms;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace ExpeditionHelper
{
    public class ExpeditionHelperSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(false);
        public RangeNode<int> LineThickness { get; set; } = new RangeNode<int>(2,1,5);
    }
}

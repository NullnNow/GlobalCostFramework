using StardewValley.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalCostFramework
{

    public class AdjustmentData
    {
        public const string All = "ALL";
        public string? Id = null;
        public string? Target = null;
        public List<QuantityModifier> Modifiers = [];

        public QuantityModifier.QuantityModifierMode ModifierMode = QuantityModifier.QuantityModifierMode.Stack;

    }
}

using StardewValley.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalCostFramework
{
    /// <summary>
    /// Used to change the cost of objects.
    /// Consists of the following:
    /// - Target: A GSQ used to determine which item(s) to modify.
    /// - Modifiers: A list of Quantity Modifiers used to increase or decrease the price
    ///   of items.
    /// - ModifierMode: What Quantity Modifier Mode should be used when there are multiple
    ///   valid Quantity Modifiers. Defaults to Stack.
    /// </summary>
    public class AdjustmentData
    {
        public string? Target = null;
        public List<QuantityModifier> Modifiers = [];

        public QuantityModifier.QuantityModifierMode ModifierMode = QuantityModifier.QuantityModifierMode.Stack;

    }
}

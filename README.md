# GlobalCostFramework
Global Cost Framework is a Stardew Valley mod that adds the ability to easily and dynamically change the cost of items.

## Price Adjustment Asset
This mod adds a ``Nullnnow.GCF/PriceAdjustmentData`` asset, which is a dictionary used to change the price of items.

NOTE: You can provide ``Input`` to the game state queries (GSQs) used in the ``Target`` and ``Condition`` fields of the quantity modifiers to refer to a specific item instance.
| Field | Type   | Description | Mandatory
| ------|--------| ------------| ---------
| Target | string | A GSQ used to determine which item(s) to modify. | Yes
| Modifiers | List<QuantityModifier> | A list of QuantityModifiers used to increase or decrease the price of the target. | Yes
| ModifierMode | QuantityModifierMode | How to handle cases when multiple QuantityModifiers can be applied. | No (Defaults to Stack).

## New Game State Queries
### Nullnnow.GCF_SELLING_TO
Whether the player is currently selling an item to a specific shop.
| Field | Type   | Description | Mandatory
| ------|--------| ------------| ---------
| Shop  | string | The name of the shop (e.g., SeedShop). | Yes



### Nullnnow.GCF_NPC_HAS_TASTE
Whether a specific NPC has a specific gift taste for a provided item.

| Field | Type   | Description | Mandatory
| ------|--------| ------------| ---------
| NPC   | string | The name of an NPC (e.g., Pierre). | Yes
| Item  | string | Either the qualified or unqualified ID of an object. Can also be ``Input`` for use in Price Adjustments. | Yes
| Taste | string | What taste(s) will return a true value. Can be ``Love``, ``Like``, ``Neutral``, ``Dislike``, and/or ``Hate``. | No (Defaults to ``Love``).

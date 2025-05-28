using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestMod.Content.Materials
{
    public class Blood : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
            
        }
        public override void SetDefaults()
        {
            Item.width = 20; 
            Item.height = 20; 

            Item.maxStack = Item.CommonMaxStack; 
            Item.value = Item.buyPrice(silver: 1); 
        }
    }
}
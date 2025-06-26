using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using TestMod.Content.Projectiles;

namespace TestMod.Content.Items
{ 
	
	public class TestSword : ModItem
	{		
        public override void SetDefaults()
		{
			Item.DefaultToStaff(ModContent.ProjectileType<Projectiles.IceProjectile>(), 20, 10, 11);
			Item.SetWeaponValues(500, 0, 50);
			Item.DamageType = DamageClass.Magic;
			Item.width = 40;
			Item.height = 40;
            Item.SetShopValues(ItemRarityColor.LightRed4, 10000);
            Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;

        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 100);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.statLife < player.statLifeMax2 / 2)
			{
				mult *= 0.5f;
			}
		}

    }
}

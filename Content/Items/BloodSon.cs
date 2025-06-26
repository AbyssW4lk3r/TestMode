using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TestMod.Content.Materials;

namespace TestMod.Content.Items
{
    public class BloodSon : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToRangedWeapon(ProjectileID.PurificationPowder, AmmoID.Bullet, 5, 16f, true);
            Item.width = 80;
            Item.height = 40;
            Item.scale = 0.75f;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item11;


            Item.useTime = 8;
            Item.useAnimation = 8; 
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged; 
            Item.damage = 70; 
            Item.knockBack = 1f; 
            Item.noMelee = true;

            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Blood>(100)
                .AddIngredient(ItemID.ShroomiteBar, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6f, -2f);
        }

    }
}
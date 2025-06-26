using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TestMod.Content.Materials;
using TestMod.Content.Projectiles;
using Microsoft.Xna.Framework;

namespace TestMod.Content.Ammo
{
    public class BloodBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;

            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;

            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.knockBack = 2f;
            Item.value = 10;
            Item.rare = ItemRarityID.Red;
            Item.shootSpeed = 4.5f;
            Item.shoot = ModContent.ProjectileType<Projectiles.BloodBullet>();

            Item.ammo = AmmoID.Bullet;

            



        }
        public override void AddRecipes()
        {
            CreateRecipe(100)
                .AddIngredient(ItemID.MusketBall, 100)
                .AddTile(TileID.Anvils)
                .Register();
                
        }
    }
}
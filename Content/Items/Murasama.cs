using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestMod.Content.Items
{
    public class Murasama : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 96;
            Item.height = 96;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.damage = 50;
            Item.knockBack = 5f;
            Item.shoot = ModContent.ProjectileType<Projectiles.MurasamaSwing>();
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = Main.MouseWorld - player.Center;
            velocity.Normalize();
            velocity *= Item.shootSpeed;
        }
    }
}

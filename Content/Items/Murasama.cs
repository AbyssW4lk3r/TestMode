using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TestMod.Content.Buffs;
using TestMod.Content.Projectiles;

namespace TestMod.Content.Items
{
    public class Murasama : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 13));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

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
            Item.shoot = ModContent.ProjectileType<MurasamaSwing>();
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
            Item.useTurn = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // Правая кнопка мыши
            {
                Item.useStyle = ItemUseStyleID.HoldUp; // Изменяем стиль анимации
                Item.shoot = ModContent.ProjectileType<DistortionProjectile>(); // Снаряд для ПКМ
                return true;
            }
            else // Левая кнопка мыши
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.shoot = ModContent.ProjectileType<MurasamaSwing>(); // Обычная атака
                return player.ownedProjectileCounts[Item.shoot] <= 0;
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2) // Только для ПКМ
            {
                position = player.Center;
                velocity = Vector2.Zero; // Статичный снаряд
            }
            else
            {
                velocity = Main.MouseWorld - player.Center;
                velocity.Normalize();
                velocity *= Item.shootSpeed;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<FrozenBurn>(), 120);

            if (target.TryGetGlobalNPC(out FrozenBurnNPC frozenBurnNPC))
            {
                frozenBurnNPC.frozenBurnData.HitCount++;
            }
        }
    }
}
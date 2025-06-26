using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TestMod.Content.Accessories
{
    public class Singularity : ModItem
    {
        public static readonly int AdditiveDamageBonus = 50;
        public static readonly int CritChanceBonus = 25;
        public static readonly int AttackSpeedBonus = 20;
        public static readonly int KnockbackBonus = 100;
        public static readonly int MoveSpeedBonus = 30;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(AdditiveDamageBonus, CritChanceBonus, AttackSpeedBonus, KnockbackBonus, MoveSpeedBonus);

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.accessory = true;
            Item.SetShopValues(ItemRarityColor.Cyan9, Item.buyPrice(platinum: 50));
        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += AdditiveDamageBonus / 100f;
            player.GetCritChance(DamageClass.Generic) += CritChanceBonus;
            player.GetAttackSpeed(DamageClass.Generic) += AttackSpeedBonus / 100f;
            player.GetKnockback(DamageClass.Generic) += KnockbackBonus / 100f;
            player.moveSpeed += MoveSpeedBonus / 100f;
        }

    }
}
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using TestMod.Content.Items;

namespace TestMod.Content.Buffs
{
    public class FrozenBurn : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Frostburn;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            var data = npc.GetGlobalNPC<FrozenBurnNPC>().frozenBurnData;

            npc.color = Color.Lerp(npc.color, new Color(100, 100, 255), 0.2f);

            var frozenBurnData = npc.GetGlobalNPC<FrozenBurnNPC>().frozenBurnData;

            if (frozenBurnData.HitCount >= 30 && !frozenBurnData.IsDamaging)
            {
                frozenBurnData.IsDamaging = true;
                frozenBurnData.DamageTicksLeft = 5;
                frozenBurnData.LastDamageTime = Main.GameUpdateCount;
            }

            if (Main.GameUpdateCount - data.LastHitTime > 300)
            {
                npc.color = Color.White;               
                npc.DelBuff(buffIndex);
                data.Reset();
                return;
            }

            if (frozenBurnData.IsDamaging && Main.GameUpdateCount - frozenBurnData.LastDamageTime >= 60)
            {
                // Рассчитываем ЧИСТЫЙ урон (10% от max HP, разделённые на 5 тиков)
                int pureDamage = (int)(npc.lifeMax * 0.03f); // 2% за тик

                // Наносим урон, игнорируя броню
                npc.SimpleStrikeNPC(pureDamage, hitDirection: 1, noPlayerInteraction: true);

                frozenBurnData.DamageTicksLeft--;
                frozenBurnData.LastDamageTime = Main.GameUpdateCount;

                if (frozenBurnData.DamageTicksLeft <= 0)
                {
                    npc.color = Color.White;
                    npc.DelBuff(buffIndex);
                    frozenBurnData.Reset();
                }
            }
        }
    }

    public class FrozenBurnData
    {
        public int HitCount = 0;
        public bool IsDamaging = false;
        public int DamageTicksLeft = 0;
        public ulong LastDamageTime = 0;
        public ulong LastHitTime;

        public void Reset()
        {
            HitCount = 0;
            IsDamaging = false;
            DamageTicksLeft = 0;
            LastDamageTime = 0;
            LastHitTime = 0;
        }
    }

    public class FrozenBurnNPC : GlobalNPC
    {

        public override bool InstancePerEntity => true;

        public FrozenBurnData frozenBurnData = new FrozenBurnData();

        public override void ResetEffects(NPC npc)
        {
            if (!npc.HasBuff(ModContent.BuffType<FrozenBurn>()))
            {
                frozenBurnData.Reset();
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.type == ModContent.ItemType<Murasama>())
            {
                ProcessFrozenBurnHit(npc);
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            // Фильтр по типам урона (если нужно)
            if (projectile.DamageType == DamageClass.Melee ||
                projectile.DamageType == DamageClass.Ranged ||
                projectile.DamageType == DamageClass.Magic)
            {
                ProcessFrozenBurnHit(npc);
            }
        }

        private void ProcessFrozenBurnHit(NPC npc)
        {
            var data = frozenBurnData;
            data.LastHitTime = Main.GameUpdateCount;

            if (npc.TryGetGlobalNPC(out FrozenBurnNPC frozenBurnNPC))
            {
                data.HitCount++;
                npc.AddBuff(ModContent.BuffType<FrozenBurn>(), 301); // 3 секунды
            }
        }
    }
}
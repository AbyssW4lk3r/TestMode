using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestMod.Content.Projectiles
{
    public class DistortionProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 600;
            Projectile.height = 143;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center + new Vector2(0, -player.height / 2);

            Vector2 mousePos = Main.MouseWorld;
            Vector2 direction = mousePos - player.Center;
            direction.Normalize();
            Projectile.rotation = direction.ToRotation();
            Projectile.direction = direction.X > 0 ? 1 : -1; // Устанавливаем направление для удара

            if (Projectile.timeLeft < 30)
            {
                float scale = Projectile.timeLeft / 30f;
                Projectile.scale = scale;
            }
        }

        public override void OnKill(int timeLeft)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.getRect().Intersects(Projectile.getRect()))
                {
                    NPC.HitInfo hitInfo = new NPC.HitInfo()
                    {
                        Damage = 50,
                        Knockback = 5f,
                        HitDirection = Projectile.direction,
                        Crit = false
                    };

                    npc.StrikeNPC(hitInfo);

                    if (Main.netMode != NetmodeID.Server)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
                        }
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("TestMod/Content/Projectiles/DistortionProjectile").Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }
    }
}
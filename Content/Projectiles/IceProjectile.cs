using TestMod.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestMod.Content.Projectiles
{
    public class IceProjectile : ModProjectile
    {
        private NPC HomingTarget
        {
            get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];
            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
            }
        }

        public ref float DelayTimer => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0f, 1f);

            float maxDetectRadius = 400f;


            if (DelayTimer < 10)
            {
                DelayTimer += 1;
                return;
            }

            if (HomingTarget == null)
            {
                Projectile.ai[0] += 1f;


                Projectile.rotation = Projectile.velocity.ToRotation();

                if (Projectile.spriteDirection == -1)
                {
                    Projectile.rotation += MathHelper.Pi;
                }
                HomingTarget = FindClosestNPC(maxDetectRadius);

            }

            if (HomingTarget != null && !IsValidTarget(HomingTarget))
            {
                HomingTarget = null;
            }

            if (HomingTarget == null)
            {
                return;
            }
            


            float length = Projectile.velocity.Length();
            float targetAngle = Projectile.AngleTo(HomingTarget.Center);
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(3)).ToRotationVector2() * length;
            Projectile.rotation = Projectile.velocity.ToRotation();

    }
        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

          
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        
            foreach (var target in Main.ActiveNPCs)
            {
              
                if (IsValidTarget(target))
                {
                 
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);


                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }
        public bool IsValidTarget(NPC target)
        {
            return target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 1, 1, target.position, target.width, target.height);
        }

        
        

    }
}
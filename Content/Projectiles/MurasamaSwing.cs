using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TestMod.Content.Projectiles
{
    public class MurasamaSwing : ModProjectile
    {
        private const int FrameWidth = 384;
        private const int FrameHeight = 270;
        private const int TotalFrames = 14;
        private const float TotalAnimationTime = 0.6f; 
        private readonly int[] HitFrames = { 0, 6, 11 }; 

        private Player Owner => Main.player[Projectile.owner];
        private float timer;
        private bool hasHit = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = TotalFrames;
        }

        public override void SetDefaults()
        {
            Projectile.width = FrameWidth;
            Projectile.height = FrameHeight;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = (int)(TotalAnimationTime * 60);
        }

        public override void AI()
        {
            timer++;
            Projectile.frame = (int)(timer / (TotalAnimationTime * 60) * TotalFrames);

            UpdateDamage();
            UpdateDirection();
            UpdatePlayer();

            if (timer >= TotalAnimationTime * 60)
                Projectile.Kill();
        }

        private void UpdateDamage()
        {
            bool shouldDamage = false;
            foreach (int frame in HitFrames)
            {
                if (Projectile.frame == frame)
                {
                    shouldDamage = true;
                    break;
                }
            }

            Projectile.damage = shouldDamage ? (int)(Owner.GetWeaponDamage(Owner.HeldItem) * 1.2f) : 0;

            if (shouldDamage && !hasHit)
            {
                hasHit = true;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                }
            }
            else if (!shouldDamage)
            {
                hasHit = false;
            }
        }

        private void UpdateDirection()
        {
            Vector2 direction = Main.MouseWorld - Owner.Center;
            direction.Normalize();

            Projectile.spriteDirection = direction.X > 0 ? 1 : -1;
            Projectile.rotation = direction.ToRotation();
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.Pi;

            Projectile.Center = Owner.Center + direction * 140f;
        }

        private void UpdatePlayer()
        {
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.ChangeDir(Projectile.spriteDirection);
        }       
    }
}
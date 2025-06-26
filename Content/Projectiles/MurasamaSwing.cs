using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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

        private static readonly SoundStyle SwingSound1 = new SoundStyle("TestMod/Content/Sounds/MurasamaSwing") { Volume = 0.7f };
        private static readonly SoundStyle SwingSound2 = new SoundStyle("TestMod/Content/Sounds/MurasamaBigSwing") { Volume = 0.7f };
        private static readonly SoundStyle OrganicHitSound = new SoundStyle("TestMod/Content/Sounds/MurasamaHitOrganic") { Volume = 0.8f };
        private static readonly SoundStyle InorganicHitSound = new SoundStyle("TestMod/Content/Sounds/MurasamaHitInorganic") { Volume = 0.8f };

        private bool hasPlayedSwingSound = false;
        private bool[] hasPlayedHitSound = new bool[3];


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
            Projectile.DamageType = DamageClass.Generic;
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
            Lighting.AddLight(Projectile.Center, new Vector3(0.3f, 0.5f, 0.8f) * 5);

            if (!hasPlayedSwingSound && (Projectile.frame == 0 || Projectile.frame == 6))
            {
                SoundEngine.PlaySound(SwingSound1, Projectile.Center);
                hasPlayedSwingSound = true;
            }
            else if (!hasPlayedSwingSound && Projectile.frame == 11)
            {
                SoundEngine.PlaySound(SwingSound2, Projectile.Center);
                hasPlayedSwingSound = true;
            }
            if (Projectile.frame != 0 && Projectile.frame != 6 && Projectile.frame != 11)
            {
                hasPlayedSwingSound = false;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            bool isOrganic = !target.SpawnedFromStatue && !target.immortal &&
                           (target.boss || target.lifeMax > 5 || target.friendly);

            // Воспроизводим соответствующий звук
            if (Projectile.frame == 0 && !hasPlayedHitSound[0])
            {
                SoundEngine.PlaySound(isOrganic ? OrganicHitSound : InorganicHitSound, target.Center);
                hasPlayedHitSound[0] = true;
            }
            else if (Projectile.frame == 6 && !hasPlayedHitSound[1])
            {
                SoundEngine.PlaySound(isOrganic ? OrganicHitSound : InorganicHitSound, target.Center);
                hasPlayedHitSound[1] = true;
            }
            else if (Projectile.frame == 11 && !hasPlayedHitSound[2])
            {
                SoundEngine.PlaySound(isOrganic ? OrganicHitSound : InorganicHitSound, target.Center);
                hasPlayedHitSound[2] = true;
            }
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
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFlare);
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

            Projectile.Center = Owner.Center + direction * 130f;
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
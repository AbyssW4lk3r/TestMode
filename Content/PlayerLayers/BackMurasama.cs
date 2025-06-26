using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace TestMod.Content.PlayerLayers
{
    public class MurasamaBack : PlayerDrawLayer
    {
        public static float SheathedPosX = -5f; 
        public static float SheathedPosY = 5f; 
        public static float DrawnPosX = -5f;   
        public static float DrawnPosY = 5f;   

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player player = drawInfo.drawPlayer;

            if (!player.HasItem(ModContent.ItemType<Items.Murasama>()))
                return;

            bool isAttacking = player.HeldItem.type == ModContent.ItemType<Items.Murasama>() &&
                              player.itemAnimation > 0;

            Texture2D texture = ModContent.Request<Texture2D>(
                isAttacking ?
                "TestMod/Content/Items/MurasamaBack2" : 
                "TestMod/Content/Items/MurasamaBack"      
            ).Value;

            Vector2 position = player.MountedCenter + new Vector2(
                (isAttacking ? DrawnPosX : SheathedPosX) * player.direction,
                isAttacking ? DrawnPosY : SheathedPosY
            ) - Main.screenPosition;

            drawInfo.DrawDataCache.Add(new DrawData(
                texture,
                position,
                null,
                Color.White,
                0f,
                new Vector2(texture.Width / 2f, texture.Height / 2f),
                0.9f,
                player.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0
            ));
        }
    }
}
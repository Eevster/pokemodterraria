using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Pokemod.Content.Buffs
{
    public class StringShotDebuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
		}

		public override void Update(Player player, ref int buffIndex) {
			player.velocity.X *= 0.5f;
            if(player.velocity.Y<0){
                player.velocity.Y *= 0.5f;
            }
		}

        public override void Update(NPC npc, ref int buffIndex){
            npc.GetGlobalNPC<StringShotGlobalNPC>().hasBuff = true;
        }
    }

    public class StringShotGlobalNPC : GlobalNPC
    {
        public bool hasBuff;
        private static Asset<Texture2D> stringTexture;
        public override void Load()
        { 
            stringTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/BuffVisuals/StringShotVisual");
        }

        public override void Unload()
        { 
            stringTexture = null;
        }
        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            hasBuff = false;;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(hasBuff){
                Main.EntitySpriteDraw(stringTexture.Value, npc.Center - Main.screenPosition, stringTexture.Value.Bounds, drawColor, npc.rotation, stringTexture.Size() * 0.5f, (npc.width/stringTexture.Width()>1f)?(npc.width/stringTexture.Width()):1f, SpriteEffects.None, 0);
            }
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public override void PostAI(NPC npc)
        {
            base.PostAI(npc);
            if(hasBuff){
                float speedMultiplier = 0.9f;

                if(npc.ModNPC is PokemonWildNPC){
                    speedMultiplier = 0.4f;
                }

                npc.velocity.X *= speedMultiplier;
                if(npc.noGravity){
                    npc.velocity.Y *= speedMultiplier;
                }else{
                    if(npc.velocity.Y < 0){
                        npc.velocity.Y *= speedMultiplier;
                    }
                }
            }
        }
    }

    public class StringShotPlayer : ModPlayer
	{
        private static Asset<Texture2D> stringTexture;
        public override void Load()
        { 
            stringTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/BuffVisuals/StringShotVisual");
        }

        public override void Unload()
        { 
            stringTexture = null;
        }
        
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if(Player.HasBuff<StringShotDebuff>()){
                Color drawColor = Lighting.GetColor((int)(Player.Center.X / 16), (int)(Player.Center.X / 16));
                Main.EntitySpriteDraw(stringTexture.Value, Player.Center - Main.screenPosition, stringTexture.Value.Bounds, drawColor, Player.fullRotation, stringTexture.Size() * 0.5f, 1, SpriteEffects.None, 0);
            }
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }
    }
}
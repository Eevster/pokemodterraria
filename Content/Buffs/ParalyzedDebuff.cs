using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Pokemod.Content.Buffs
{
    public class ParalyzedDebuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<ParalyzedPlayer>().hasBuff = true;
		}

        public override void Update(NPC npc, ref int buffIndex){
            npc.GetGlobalNPC<ParalyzedGlobalNPC>().hasBuff = true;
        }
    }

    public class ParalyzedGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool hasBuff;
        public int paralyzeTimer = 0;
        public bool canParalyze = false;
        const int paralyzeTime = 20;

        private static Asset<Texture2D> paralyzedTexture;
        public override void Load()
        { 
            paralyzedTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/ThunderWaveExplosion");
        }

        public override void Unload()
        { 
            paralyzedTexture = null;
        }

        public override void ResetEffects(NPC npc)
        {
            if(hasBuff){
                if(paralyzeTimer > 0) paralyzeTimer--;

                if(paralyzeTimer <= 0){
                    if(canParalyze){
                        paralyzeTimer = Main.rand.Next(npc.boss?80:20,100);
                        canParalyze = false;
                    }else{
                        paralyzeTimer = (int)(paralyzeTime *(npc.boss?0.5f:1f));
                        canParalyze = true;
                    }
                }
            }else{
                paralyzeTimer = 0;
                canParalyze = false;
            }

            hasBuff = false;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(paralyzeTimer > 0 && canParalyze){
                Main.EntitySpriteDraw(paralyzedTexture.Value, npc.Center - Main.screenPosition, paralyzedTexture.Frame(1, 4, 0, paralyzeTimer/3), Color.White, 0, paralyzedTexture.Frame(1, 4).Size() / 2f, (npc.width/paralyzedTexture.Width()>1f)?(npc.width/paralyzedTexture.Width()):1f, SpriteEffects.None, 0);
            }
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public override void PostAI(NPC npc)
        {
            base.PostAI(npc);

            if(paralyzeTimer > 0 && canParalyze){
                float speedMultiplier = 0.75f;

                if(npc.boss){
                    speedMultiplier = 0.8f;
                }

                if(npc.ModNPC is PokemonWildNPC){
                    speedMultiplier = 0.1f;
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

    public class ParalyzedPlayer : ModPlayer
	{
        public bool hasBuff;
        
        public int paralyzeTimer = 0;
        public bool canParalyze = false;
        const int paralyzeTime = 10;

        private static Asset<Texture2D> paralyzedTexture;
        public override void Load()
        { 
            paralyzedTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/ThunderWaveExplosion");
        }

        public override void Unload()
        { 
            paralyzedTexture = null;
        }

        public override void ResetEffects()
        {
            if(hasBuff){
                if(paralyzeTimer > 0) paralyzeTimer--;

                if(paralyzeTimer <= 0){
                    if(canParalyze){
                        paralyzeTimer = Main.rand.Next(20,100);
                        canParalyze = false;
                    }else{
                        paralyzeTimer = paralyzeTime;
                        canParalyze = true;
                    }
                }
            }else{
                paralyzeTimer = 0;
                canParalyze = false;
            }

            hasBuff = false;
        }

        public override void PreUpdateMovement()
        {
            if(paralyzeTimer > 0 && canParalyze){
                Player.velocity.X *= 0.2f;
                if(Player.velocity.Y<0){
                    Player.velocity.Y *= 0.2f;
                }
            }

            base.PreUpdateMovement();
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if(paralyzeTimer > 0 && canParalyze){
                Main.EntitySpriteDraw(paralyzedTexture.Value, Player.Center - Main.screenPosition, paralyzedTexture.Frame(1, 4, 0, paralyzeTimer/3), Color.White, 0, paralyzedTexture.Frame(1, 4).Size() / 2f, 1f, SpriteEffects.None, 0);
            }
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }
    }
}
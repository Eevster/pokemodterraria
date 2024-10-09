using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Buffs
{
    public class ParalizedDebuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<ParalizedPlayer>().hasBuff = true;
		}

        public override void Update(NPC npc, ref int buffIndex){
            npc.GetGlobalNPC<ParalizedGlobalNPC>().hasBuff = true;
        }
    }

    public class ParalizedGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool hasBuff;
        public int paralizeTimer = 0;
        public bool canParalize = false;
        const int paralizeTime = 20;

        private static Asset<Texture2D> paralizedTexture;
        public override void Load()
        { 
            paralizedTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/ThunderWaveExplosion");
        }

        public override void Unload()
        { 
            paralizedTexture = null;
        }

        public override void ResetEffects(NPC npc)
        {
            if(hasBuff){
                if(paralizeTimer > 0) paralizeTimer--;

                if(paralizeTimer <= 0){
                    if(canParalize){
                        paralizeTimer = Main.rand.Next(npc.boss?80:20,100);
                        canParalize = false;
                    }else{
                        paralizeTimer = (int)(paralizeTime *(npc.boss?0.5f:1f));
                        canParalize = true;
                    }
                }
            }else{
                paralizeTimer = 0;
                canParalize = false;
            }

            hasBuff = false;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(paralizeTimer > 0 && canParalize){
                Main.EntitySpriteDraw(paralizedTexture.Value, npc.Center - Main.screenPosition, paralizedTexture.Frame(1, 4, 0, paralizeTimer/3), Color.White, 0, paralizedTexture.Frame(1, 4).Size() / 2f, (npc.width/paralizedTexture.Width()>1f)?(npc.width/paralizedTexture.Width()):1f, SpriteEffects.None, 0);
            }
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public override void PostAI(NPC npc)
        {
            base.PostAI(npc);

            if(paralizeTimer > 0 && canParalize){
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

    public class ParalizedPlayer : ModPlayer
	{
        public bool hasBuff;
        
        public int paralizeTimer = 0;
        public bool canParalize = false;
        const int paralizeTime = 10;

        private static Asset<Texture2D> paralizedTexture;
        public override void Load()
        { 
            paralizedTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/ThunderWaveExplosion");
        }

        public override void Unload()
        { 
            paralizedTexture = null;
        }

        public override void ResetEffects()
        {
            if(hasBuff){
                if(paralizeTimer > 0) paralizeTimer--;

                if(paralizeTimer <= 0){
                    if(canParalize){
                        paralizeTimer = Main.rand.Next(20,100);
                        canParalize = false;
                    }else{
                        paralizeTimer = paralizeTime;
                        canParalize = true;
                    }
                }
            }else{
                paralizeTimer = 0;
                canParalize = false;
            }

            hasBuff = false;
        }

        public override void PreUpdateMovement()
        {
            if(paralizeTimer > 0 && canParalize){
                Player.velocity.X *= 0.2f;
                if(Player.velocity.Y<0){
                    Player.velocity.Y *= 0.2f;
                }
            }

            base.PreUpdateMovement();
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if(paralizeTimer > 0 && canParalize){
                Main.EntitySpriteDraw(paralizedTexture.Value, Player.Center - Main.screenPosition, paralizedTexture.Frame(1, 4, 0, paralizeTimer/3), Color.White, 0, paralizedTexture.Frame(1, 4).Size() / 2f, 1f, SpriteEffects.None, 0);
            }
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }
    }
}
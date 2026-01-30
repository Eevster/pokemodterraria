using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Players;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using Pokemod.Content.Projectiles;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Pokemod.Content.Buffs
{
    public class LeechSeedDebuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<LeechSeedPlayer>().hasBuff = true;
		}

        public override void Update(NPC npc, ref int buffIndex){
            npc.GetGlobalNPC<LeechSeedGlobalNPC>().hasBuff = true;
        }
    }

    public class LeechSeedGlobalNPC : GlobalNPC
    {
        public bool hasBuff;
        public float activeBuffTime;
        public Player targetPlayer;
        private static Asset<Texture2D> visualTexture;
        public override void Load()
        { 
            visualTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/BuffVisuals/LeechSeedVisual");
        }

        public override void Unload()
        { 
            visualTexture = null;
        }
        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            if (hasBuff)
            {
                activeBuffTime += 1f;
                if(activeBuffTime > 1000) activeBuffTime = 35;
                hasBuff = false;
            }
            else{
                if(targetPlayer != null) targetPlayer = null;
                if(activeBuffTime > 0f)
                {
                    activeBuffTime = 0f;
                }
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(hasBuff){
                Main.EntitySpriteDraw(visualTexture.Value, npc.Center + new Vector2(0,-0.5f*npc.height).RotatedBy(npc.rotation) - Main.screenPosition, visualTexture.Frame(1,8,0, Math.Clamp((int)(activeBuffTime/5),0,7)), drawColor, npc.rotation, visualTexture.Frame(1,8).Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public override void PostAI(NPC npc)
        {
            base.PostAI(npc);
            if(activeBuffTime > 35f && activeBuffTime%60 == 0)
            {
                npc.SimpleStrikeNPC(5, 1);
                
                if(targetPlayer != null)
                {
                    if (targetPlayer.GetModPlayer<PokemonPlayer>().currentActivePokemon.Count > 0)
                    {
                        PokemonPetProjectile pokemonProj = targetPlayer.GetModPlayer<PokemonPlayer>().GetPokemonProjectile(0);
                        if(pokemonProj != null)
                        {
                            PokemonAttack.HealEffect(pokemonProj, 5);
                        }
                    }
                    else
                    {
                        PokemonAttack.HealEffect(targetPlayer, 2);
                    }
                }
            }
        }
    }

    public class LeechSeedPlayer : ModPlayer
	{
        public bool hasBuff;
        public float activeBuffTime;
        public Player targetPlayer;

        private static Asset<Texture2D> visualTexture;
        public override void Load()
        { 
            visualTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/BuffVisuals/LeechSeedVisual");
        }

        public override void Unload()
        { 
            visualTexture = null;
        }

        public override void ResetEffects()
        {
            if (hasBuff)
            {
                activeBuffTime += 1f;
                if(activeBuffTime > 1000) activeBuffTime = 35;
                hasBuff = false;
            }
            else{
                if(targetPlayer != null) targetPlayer = null;
                if(activeBuffTime > 0f)
                {
                    activeBuffTime = 0f;
                }
            }
        }

        public override void PostUpdateBuffs()
        {
            base.PostUpdateBuffs();
            if(activeBuffTime > 35f && activeBuffTime%60 == 0)
            {
                Player.HurtInfo hurtInfo = new Player.HurtInfo{Damage = 5, HitDirection = 1, Knockback = 0};
                Player.Hurt(hurtInfo);

                if (targetPlayer != null)
                {
                    if (targetPlayer.GetModPlayer<PokemonPlayer>().currentActivePokemon.Count > 0)
                    {
                        PokemonPetProjectile pokemonProj = targetPlayer.GetModPlayer<PokemonPlayer>().GetPokemonProjectile(0);
                        if(pokemonProj != null)
                        {
                            PokemonAttack.HealEffect(pokemonProj, 5);
                        }
                    }
                    else
                    {
                        PokemonAttack.HealEffect(targetPlayer, 2);
                    }
                }
            }
        }
        
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if(hasBuff){
                Color drawColor = Lighting.GetColor((int)(Player.Center.X / 16), (int)(Player.Center.X / 16));
                Main.EntitySpriteDraw(visualTexture.Value, Player.Center + new Vector2(0,-0.5f*Player.height).RotatedBy(Player.fullRotation) - Main.screenPosition, visualTexture.Frame(1,8,0, Math.Clamp((int)(activeBuffTime/5),0,7)), drawColor, Player.fullRotation, visualTexture.Frame(1,8).Size() * 0.5f, 1, SpriteEffects.None, 0);
            }
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        }
    }
}
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Pokemod.Common.GlobalNPCs
{
    public class HitByPokemonNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public List<Projectile> pokemonProjs = [];
        public bool canGiveExp = true;

        private static Asset<Texture2D> buffVisualTexture;
        public Vector2 buffVisualPosition;
        public override void Load()
        {
            buffVisualTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/BuffVisuals/EXPFocusVisuals");
        }

        public override void Unload()
        {
            buffVisualTexture = null;
        }

        /*public override void OnKill(NPC head)
        {
            Main.NewText("OnKill");
            if(pokemonProj != null){
				if(pokemonProj.active){
					if(pokemonProj.ModProjectile is PokemonPetProjectile){
                        PokemonPetProjectile pokemonMainProj = (PokemonPetProjectile)pokemonProj?.ModProjectile;
                        pokemonMainProj?.SetExtraExp(SetExpGained(head));
                        Main.NewText(pokemonMainProj.Name);
                    }
				}else{
                    Main.NewText("No Active");
                }
			}else{
                Main.NewText("Null");
            }
            base.OnKill(head);
        }*/

        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            pokemonProjs = pokemonProjs.Distinct().ToList();
            pokemonProjs = pokemonProjs.FindAll(x => x.active);

            if (npc.realLife == -1)
            {
                if (npc.life <= 0 && !npc.immortal && canGiveExp)
                {
                    if (pokemonProjs.Count > 0)
                    {
                        for (int i = 0; i < pokemonProjs.Count; i++)
                        {
                            Projectile proj = pokemonProjs[i];
                            if (proj != null)
                            {
                                if (proj.active)
                                {
                                    if (proj.ModProjectile is PokemonPetProjectile)
                                    {
                                        PokemonPetProjectile pokemonMainProj = (PokemonPetProjectile)proj?.ModProjectile;
                                        pokemonMainProj?.SetGainedExp(SetExpGained(npc, pokemonProjs.Count));
                                    }
                                }
                            }
                        }
                        canGiveExp = false;
                    }
                }
            }
            else
            {
                NPC realNpc = Main.npc?[npc.realLife];
                if (realNpc != null)
                {
                    if (realNpc.life <= 0 && !realNpc.immortal && realNpc.GetGlobalNPC<HitByPokemonNPC>().canGiveExp)
                    {
                        if (pokemonProjs.Count > 0)
                        {
                            for (int i = 0; i < pokemonProjs.Count; i++)
                            {
                                Projectile proj = pokemonProjs[i];
                                if (proj != null)
                                {
                                    if (proj.active)
                                    {
                                        if (proj.ModProjectile is PokemonPetProjectile)
                                        {
                                            PokemonPetProjectile pokemonMainProj = (PokemonPetProjectile)proj?.ModProjectile;
                                            pokemonMainProj?.SetGainedExp(SetExpGained(realNpc, pokemonProjs.Count));
                                        }
                                    }
                                }
                            }
                            realNpc.GetGlobalNPC<HitByPokemonNPC>().canGiveExp = false;
                        }
                    }
                }
            }
            base.HitEffect(npc, hit);
        }

        public static int SetExpGained(NPC npc, int split)
        {
            split = Math.Clamp(split, 1, 10);
            int exp;
            if (npc.ModNPC is PokemonWildNPC pokemonNPC) exp = (int)(100f * pokemonNPC.lvl / 7f);
            else exp = (int)Math.Sqrt(5 * npc.value);

            exp = (int)Math.Ceiling((double)exp / split);

            if (npc.value <= 0) exp = (int)(0.2f * exp);
            if (exp < 1) exp = 1;

            //Main.NewText("Value: "+npc.value+" Split: "+split+" Exp: "+exp);

            return exp;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            bool marked = false;

            if (pokemonProjs.Count > 0)
            {
                foreach (Projectile pokemon in pokemonProjs)
                {
                    if (pokemon != null && pokemon.active)
                    {
                        marked = true;
                    }
                }

                if (marked)
                {
                    buffVisualPosition = Vector2.UnitY * (npc.boss ? 40 : 27) - Main.screenPosition;
                    if (npc.realLife == npc.whoAmI) //Worm Head
                    {
                        buffVisualPosition += npc.Center + FindWormCenter(npc);
                    }
                    else if (npc.realLife != -1) //Worm Segment
                    {
                        NPC realNPC = Main.npc[npc.realLife];

                        foreach (Projectile myMark in pokemonProjs)
                        {
                            if (!realNPC.GetGlobalNPC<HitByPokemonNPC>().pokemonProjs.Contains(myMark))
                            {
                                realNPC.GetGlobalNPC<HitByPokemonNPC>().pokemonProjs.Add(myMark);
                            }
                        }
                    }
                    else buffVisualPosition += npc.Bottom;

                    Color fadeColor = drawColor;
                    fadeColor.A = (Byte)(drawColor.ToVector3().Length() / (255 * Math.Sqrt(3d)));
                    Main.EntitySpriteDraw(buffVisualTexture.Value, buffVisualPosition, buffVisualTexture.Frame(), fadeColor, 0, buffVisualTexture.Frame().Size() / 2f, 1f, SpriteEffects.None);
                }
            }
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }

        private Vector2 FindWormCenter(NPC head)
        {
            Vector2 headToTail = Vector2.Zero;
            int segmentCount = 0;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC segment = Main.npc[i];
                if(segment.realLife != head.whoAmI) continue;
                if (segment.ai[0] <= 0)
                {
                    headToTail += segment.Center - head.Center;
                }
                segmentCount++;
            }

            if (segmentCount > 0)
            {
                return headToTail / 2;
            }
            else return Vector2.Zero;
        }
    }
}
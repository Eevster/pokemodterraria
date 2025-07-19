using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	// This is an example bug net designed to demonstrate the use cases for various hooks related to catching NPCs such as critters with items.
	public abstract class BallItem : ModItem
	{
		public override string Texture => "Pokemod/Assets/Textures/Pokeballs/"+ GetType().Name;
		protected virtual int BallProj => ModContent.ProjectileType<BallProj>();
		protected virtual int BallValue => 1000;
		protected virtual float CatchRate => 1f;
		protected virtual float ThrowSpeed => 15f;
		
		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 14;
			Item.rare = ItemRarityID.Blue;
			Item.value = BallValue;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			// Use Properties
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.shootSpeed = ThrowSpeed;
			Item.UseSound = SoundID.Item1;
			Item.noUseGraphic = true;

			Item.shoot = BallProj;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, 1, 0, player.whoAmI, CatchRate);

            return false;
        }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tooltipLine = new TooltipLine(Mod, "CatchRate", CatchRate+" catch rate");
            tooltips.Add(tooltipLine);
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			Asset<Texture2D> ballTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokeballs/"+GetType().Name+"Big");

			spriteBatch.Draw(ballTexture.Value,
				position: position,
				sourceRectangle: ballTexture.Value.Bounds,
				drawColor,
				rotation: 0f,
				origin: ballTexture.Size()/2,
				scale: scale,
				SpriteEffects.None,
				layerDepth: 0f);
				
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
	}

	public abstract class BallProj : ModProjectile
	{
		public override string Texture => "Pokemod/Assets/Textures/Pokeballs/"+ GetType().Name.Replace("Proj","Item");
		protected virtual bool hasGravity => true;
		protected virtual float gravityScale => 1f; 
		private ref float catchRate => ref Projectile.ai[0];
		private int bounces = 3;
		private int captureStage = -1;
		private ref float canCapture => ref Projectile.localAI[0];
		public NPC targetPokemon;
		private int targetPokemonLife;
		private int moveTimer = 0;
		public const int moveTime = 80;

		public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write((double)catchRate);
            writer.Write((double)bounces);
			writer.Write((double)captureStage);
			writer.Write((double)moveTimer);
			writer.Write(targetPokemon != null?(double)targetPokemon.whoAmI:-1);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			catchRate = (float)reader.ReadDouble();
            bounces = (int)reader.ReadDouble();
			captureStage = (int)reader.ReadDouble();
			moveTimer = (int)reader.ReadDouble();
			int targetIndex = (int)reader.ReadDouble();
			targetPokemon = targetIndex != -1?Main.npc[targetIndex]:null;
            base.ReceiveExtraAI(reader);
        }
		
		public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<MeleeDamageClass>();
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			float moveAdjust = 0;

			if(moveTimer >= moveTime/2){
				moveAdjust = (float)(Math.Sin((2f*((float)moveTimer/moveTime)-1f)*MathHelper.TwoPi));
			}

			Main.EntitySpriteDraw(texture, Projectile.Center + new Vector2(8f*moveAdjust,0) - Main.screenPosition,
            texture.Bounds, lightColor, Projectile.rotation + moveAdjust*MathHelper.PiOver2, texture.Size() * 0.5f, 1f*Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

        public override void AI()
        {
            if(captureStage < 0){
				Projectile.rotation += MathHelper.ToRadians(10);
			}else{
				targetPokemon.velocity = Vector2.Zero;
				targetPokemon.Center = Projectile.Center + new Vector2(0,-targetPokemon.height/2);
				targetPokemon.hide = true;
				targetPokemon.friendly = true;
				targetPokemon.dontTakeDamageFromHostiles = true;
				Projectile.rotation = 0;
			}

			if(Projectile.owner == Main.myPlayer){
				if(targetPokemon != null){
					if (targetPokemon.life > 0) targetPokemonLife = targetPokemon.life;
					canCapture = FailureProb(catchRate)?1:0;
				}else{
					canCapture = 0;
				}
				Projectile.netUpdate = true;
			}

			if(bounces < 0){
				Projectile.timeLeft = 10;
				if(moveTimer <= 0){
					captureStage++;
					if(canCapture>0){
						CaptureFailure();
					}else{
						if(captureStage > 3){
							CaptureSuccess();
						}
					}
					moveTimer = moveTime;
					SoundEngine.PlaySound(new SoundStyle($"{nameof(Pokemod)}/Assets/Sounds/PBWiggle") with {Volume = 1.2f}, Projectile.Center);
				}else{
					moveTimer--;
				}
			}

			if(hasGravity || captureStage >= 0){
				Projectile.velocity.Y += 0.2f*gravityScale;
			}

			if(targetPokemon != null){
				if(Main.myPlayer == Projectile.owner){
					targetPokemon.netUpdate = true;
				}
			}

			if(Main.myPlayer == Projectile.owner){
				Projectile.netUpdate = true;
			}
        }
		

		public virtual bool FailureProb(float catchRate){
			return RegularProb(catchRate);
		}

		public bool RegularProb(float catchRate){
			if(catchRate >= 255){
				return false;
			}
			float prob = 1;
			if(targetPokemon.ModNPC is PokemonWildNPC nPC)
            {
				float pokemonRate = nPC.catchRate;
				if(targetPokemon.lifeMax > 0){
					prob =  pokemonRate * catchRate * 1.5f * (3*targetPokemon.lifeMax-2*targetPokemon.life)/(3*targetPokemon.lifeMax);
				}else{
					prob =  pokemonRate * catchRate * 1.5f;
				}

				if(prob < pokemonRate/3){
					prob = pokemonRate/3;
				}
			}

			int shakeProb = (int)(1048560/(int)Math.Sqrt((int)Math.Sqrt((int)(16711680/prob))));

			return Main.rand.Next(65536) >= shakeProb;
		}

        public override bool? CanHitNPC(NPC target)
        {
            return target.GetGlobalNPC<PokemonNPCData>().isPokemon && !target.friendly;
        }

        public override bool? CanDamage()
        {
            return captureStage < 0;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if(captureStage < 0){
				Projectile.tileCollide = true;
				targetPokemon = target;
				targetPokemon.hide = true;
				targetPokemon.friendly = true;
				targetPokemon.dontTakeDamageFromHostiles = true;

				captureStage = 0;
				Projectile.velocity = new Vector2(0,-5);

				if(Main.myPlayer == Projectile.owner){
					targetPokemon.netUpdate = true;
				}
			}

            base.OnHitNPC(target, hit, damageDone);
        }
		
        public void CaptureFailure(){
			if(targetPokemon != null){
				targetPokemon.Center = Projectile.Center + new Vector2(0,-targetPokemon.height/2);
				targetPokemon.hide = false;
				targetPokemon.friendly = false;
				targetPokemon.dontTakeDamageFromHostiles = false;
				SoundEngine.PlaySound(new SoundStyle($"{nameof(Pokemod)}/Assets/Sounds/PBOut"), Projectile.Center);
				if(targetPokemon.life <= 0) ServerText.SendMessageToPlayer(Language.GetTextValue("Mods.Pokemod.PokemonInfo.RanAway"),Main.player[Projectile.owner]);
				Projectile.Kill();
			}
		}

		public void CaptureSuccess()
		{
			if (targetPokemon != null)
			{
				Player player = Main.player[Projectile.owner];
				int item;
				string pokemonName = targetPokemon.GetGlobalNPC<PokemonNPCData>().pokemonName;
				bool shiny = targetPokemon.GetGlobalNPC<PokemonNPCData>().shiny;
				int lvl = targetPokemon.GetGlobalNPC<PokemonNPCData>().lvl;
				int[] IVs = targetPokemon.GetGlobalNPC<PokemonNPCData>().IVs;
				int nature = targetPokemon.GetGlobalNPC<PokemonNPCData>().nature;
				string variant = targetPokemon.GetGlobalNPC<PokemonNPCData>().variant;

				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					item = Item.NewItem(targetPokemon.GetSource_Death(), targetPokemon.position, targetPokemon.Size, ModContent.ItemType<CaughtPokemonItem>());
					CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[item].ModItem;
					pokeItem.SetPokemonData(pokemonName, Shiny: shiny, BallType: GetType().Name.Replace("Proj", "Item"), lvl, IVs, nature, variant: variant);
					pokeItem.currentHP = targetPokemon.life;
					SetExtraPokemonEffects(ref pokeItem);
				}
				else if (Main.netMode == NetmodeID.MultiplayerClient && Main.myPlayer == Projectile.owner)
				{
					//item = player.QuickSpawnItem(Projectile.InheritSource(Projectile), ModContent.ItemType<CaughtPokemonItem>());
					item = Item.NewItem(Projectile.InheritSource(Projectile), (int)player.position.X, (int)player.position.Y, player.width, player.height, ModContent.ItemType<CaughtPokemonItem>(), 1, noBroadcast: false, -1);
					CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[item].ModItem;
					pokeItem.SetPokemonData(pokemonName, Shiny: shiny, BallType: GetType().Name.Replace("Proj", "Item"), lvl, IVs, nature, variant: variant);
					pokeItem.currentHP = targetPokemonLife;
					SetExtraPokemonEffects(ref pokeItem);
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient) targetPokemon.StrikeInstantKill();
			}
			Projectile.Kill();
		}

		public virtual void SetExtraPokemonEffects(ref CaughtPokemonItem pokeItem){

		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if(captureStage < 0){
				SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

				for (int i = 0; i < 5; i++)
				{
					int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Dirt, 0f, 0f, 100, default(Color), 1f);
					Main.dust[dustIndex].noGravity = true;
					Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Dirt, 0f, 0f, 100, default(Color), 1f);
				}
			}else{
				if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f) {
					Projectile.velocity.X = 0;
				}
				if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f) {
					if(bounces>0 && oldVelocity.Y > 1f){
						Projectile.timeLeft = 360;
						Projectile.velocity.Y = Math.Clamp(oldVelocity.Y,-10,10) * -0.6f;
						bounces--;
					}else{
						Projectile.timeLeft = 360;
						Projectile.velocity.Y = 0;
						bounces = -1;
					}
				}
				return false;
			}
            return base.OnTileCollide(oldVelocity);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 6;
			height = 6;
            fallThrough = true;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnKill(int timeLeft)
        {
            if(targetPokemon != null){
				if(targetPokemon.hide){
					targetPokemon.hide = false;
					targetPokemon.friendly = false;
					targetPokemon.dontTakeDamageFromHostiles = false;
				}
				if(Main.myPlayer == Projectile.owner){
					targetPokemon.netUpdate = true;
				}
			}
        }
    }
}

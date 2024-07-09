using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items
{
	// This is an example bug net designed to demonstrate the use cases for various hooks related to catching NPCs such as critters with items.
	public class PokeballItemAlt : ModItem
	{
		public override string Texture => "Pokemod/Content/Items/PokeballItem";
		public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 14;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 10, 0);
			Item.maxStack = 999;
			Item.consumable = true;
			// Use Properties
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.shootSpeed = 12f;
			Item.UseSound = SoundID.Item1;

			Item.shoot = ModContent.ProjectileType<PokeballProj>();
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, type, 1, 0, player.whoAmI);

            return false;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.GemTreeRubySeed, 3)
				.AddIngredient(ItemID.IronBar, 3)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class PokeballProj : ModProjectile
	{
		public override string Texture => "Pokemod/Content/Items/PokeballItem";
		private float catchRate = 1f;
		private int bounces = 3;
		private int captureStage = -1;
		private NPC targetPokemon;
		private int moveTimer = 0;
		public const int moveTime = 80;
		public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<MeleeDamageClass>();
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
            Projectile.ignoreWater = false;
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
				Projectile.rotation = 0;
			}

			if(bounces < 0){
				Projectile.timeLeft = 10;
				if(moveTimer <= 0){
					captureStage++;
					if(Main.rand.NextBool((int)(5 * catchRate))){
						CaptureFailure();
					}else{
						if(captureStage > 3){
							CaptureSuccess();
						}
					}
					moveTimer = moveTime;
				}else{
					moveTimer--;
				}
			}

			Projectile.velocity.Y += 0.2f;

			float maxFallSpeed = 10f;
			if(Projectile.velocity.Y > maxFallSpeed){
				Projectile.velocity.Y = maxFallSpeed;
			}
        }

        public override bool? CanHitNPC(NPC target)
        {
            return target.GetGlobalNPC<PokemonNPCData>().isPokemon && !target.friendly;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if(captureStage < 0){
				targetPokemon = target;
				targetPokemon.hide = true;
				targetPokemon.friendly = true;

				captureStage = 0;
				Projectile.velocity = new Vector2(0,-5);
			}

            base.OnHitNPC(target, hit, damageDone);
        }

		public void CaptureFailure(){
			targetPokemon.Center = Projectile.Center + new Vector2(0,-targetPokemon.height/2);
			targetPokemon.hide = false;
			targetPokemon.friendly = false;
			Projectile.Kill();
		}

		public void CaptureSuccess(){
			int item = Item.NewItem(targetPokemon.GetSource_Death(), targetPokemon.position, targetPokemon.Size, ModContent.ItemType<CapturedPokemonItem>());
			string pokemonName = targetPokemon.GetGlobalNPC<PokemonNPCData>().pokemonName;
			bool shiny = targetPokemon.GetGlobalNPC<PokemonNPCData>().shiny;
			CapturedPokemonItem pokeItem = (CapturedPokemonItem)Main.item[item].ModItem;
			pokeItem.SetPokemonData(pokemonName, shiny);
			targetPokemon.StrikeInstantKill();
			Projectile.Kill();
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
						Projectile.velocity.Y = oldVelocity.Y * -0.6f;
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
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Items.Armor.BossSets;
using Pokemod.Content.Items.Consumables.BossItems;
using Pokemod.Content.Items.GeneticSamples;
using Pokemod.Content.NPCs.Bosses.BossBars;
using Pokemod.Content.Projectiles;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Pokemod.Content.Tiles.BossTrophies;
using Pokemod.Content.Projectiles.BossProjectiles;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Content.NPCs.Bosses.Mewtwo
{
	public class MewtwoBossBody : PokemonBossBody
	{
        public override int attackType => (int)TypeIndex.Fire;
        public override int defenceType1 => (int)TypeIndex.Psychic;
        public override int defenceType2 => (int)TypeIndex.Fire;
        public static int PhysicalDamage = 80;
        public static int MinionCount = 5;
        public int tooCloseToTarget = 0;
        public int secondPhaseHeadSlot = -1;
        public int MinionMaxHealthTotal { get; set; }
        public int MinionHealthTotal //Boss specific NPC.ai[] index. [0],[1],and [2] are already used for AI behaviour state, and a 2D Movement Destination.
        { 
            get => (int)NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        public bool SpawnedMinions = false;


        public override void Load()
        {
            // We want to give it a second boss head icon, so we register one
            string texture = BossHeadTexture + "_SecondPhase"; // Our texture is called "ClassName_Head_Boss_SecondPhase"
            secondPhaseHeadSlot = Mod.AddBossHeadTexture(texture, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 10;

            // Add this in for bosses that have a summon item, requires corresponding code in the item (See MewtwoBossSummonItem.cs)
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Specify the debuffs it is immune to.
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            // This boss also becomes immune to OnFire and all buffs that inherit OnFire immunity during the second half of the fight. See the ApplySecondPhaseBuffImmunities method.

            // Influences how the NPC looks in the Bestiary
            /*
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "BossSetup/Assets/Textures/Bestiary/MewtwoBoss_Preview",
                PortraitScale = 0.6f, // Portrait refers to the full picture when clicking on the icon in the bestiary
                PortraitPositionYOverride = 0f,
            }; 
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);*/
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 100;
            NPC.scale = 2f;
            NPC.damage = 0;
            NPC.defense = 55;
            NPC.lifeMax = 175000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = Item.buyPrice(platinum: 5);
            base.SetDefaults();
            NPC.dontTakeDamage = true;
            NPC.BossBar = ModContent.GetInstance<MewtwoBossBar>();
            MinionMaxHealthTotal = ModContent.GetInstance <MewtwoBossMinion>().NPC.lifeMax * MinionCount;

            accelerationPower = 0.6f;
            maxVelocity = 20f;
            neutralState = new MewtwoIdleState1(this);
            bossState = new MewtwoIdleState1(this);

            //Sets boss music client-side.
            if (!Main.dedServ)
            {
                Music = MusicID.Boss5;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // 1. Trophy
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MewtwoBossTrophyItem>(), 10));

            // 2. Classic Mode drops
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MewtwoBossMask>(), 7));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MalformedSample>(), 1, 8, 16));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PerfectedSample>(), 1, 0, 2));
            npcLoot.Add(notExpertRule);

            // 3. Expert Mode (Treasure Bag)
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MewtwoBossBag>()));

            // 4. Master Mode (relic first, pet last, everything else inbetween)
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<MewtwoBossRelicItem>()));
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit)
        {
            base.OnHitNPC(target, hit);
            if (bossState is MewtwoOverheatState)
            {
                target.AddBuff(BuffID.CursedInferno, 100);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(target, hurtInfo);
            if (bossState is MewtwoOverheatState)
            {
                target.AddBuff(BuffID.CursedInferno, 100);
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            
            // If the NPC dies, spawn gore and play a sound
            if (NPC.life <= 0)
            {
                BossRoar();
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
            SpawnMinions();
            BossRoar();
        }

        private void SpawnMinions()
        {
            if (SpawnedMinions)
            {
                // No point executing the code in this method again
                return;
            }

            SpawnedMinions = true;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Because we want to spawn minions, and minions are NPCs, we have to do this on the server (or singleplayer, "!= NetmodeID.MultiplayerClient" covers both)
                // This means we also have to sync it after we spawned and set up the minion
                return;
            }

            var entitySource = NPC.GetSource_FromAI();

            MinionMaxHealthTotal = 0;
            for (int i = 0; i < MinionCount; i++)
            {
                NPC minionNPC = NPC.NewNPCDirect(entitySource, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MewtwoBossMinion>(), NPC.whoAmI);
                if (minionNPC.whoAmI == Main.maxNPCs)
                    continue; // spawn failed due to spawn cap

                // Now that the minion is spawned, we need to prepare it with data that is necessary for it to work
                // This is not required usually if you simply spawn NPCs, but because the minion is tied to the body, we need to pass this information to it
                MewtwoBossMinion minion = (MewtwoBossMinion)minionNPC.ModNPC;
                minion.ParentIndex = NPC.whoAmI; // Let the minion know who the "parent" is
                minion.PositionOffset = i / (float)MinionCount; // Give it a separate position offset

                MinionMaxHealthTotal += minionNPC.lifeMax; // add the total minion life for boss bar shield text

                // Finally, syncing, only sync on server and if the NPC actually exists (Main.maxNPCs is the index of a dummy NPC, there is no point syncing it)
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: minionNPC.whoAmI);
                }
            }

            // sync MinionMaxHealthTotal
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }
        }

        public Vector2 IdleMovement(bool tryToHitTarget = true)
        {
            int minRange = 80;
            int maxRange = 300;
            bool makingSpace = tooCloseToTarget % 2 == 1;
            float verticalOffset = makingSpace || !tryToHitTarget ? -maxRange * 1.5f : 0;
            float horizontalOffset = makingSpace? (tooCloseToTarget % 4 == 1 ? -maxRange * 0.5f : maxRange * 0.5f) : 0f;
            MovementDestination = targetPlayer.Center + new Vector2(horizontalOffset, verticalOffset);
            int distanceToTarget = (int)(MovementDestination - NPC.Center).Length();
            
            if (distanceToTarget < minRange)
            {
                tooCloseToTarget ++;
                if (tooCloseToTarget > 4) tooCloseToTarget = 0;
            }

            return NPC.velocity + (MovementDestination - NPC.Center).SafeNormalize(-Vector2.UnitY) * accelerationPower;
        }
    }

    public class MewtwoIdleState1 : BossState
    {
        public MewtwoIdleState1(PokemonBossBody bossRef)
        {
            boss = bossRef;
            boss.NPC.damage = 0;
            frameStart = 0;
            frameEnd = 1;
            ticksPerFrame = 30;
            boss.accelerationPower = 0.2f;
        }

        public void SyncMinionLife(MewtwoBossBody mewtwo)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                mewtwo.MinionHealthTotal = GetMinionHealth();
                boss.NPC.netUpdate = true;
            }
        }

        public int GetMinionHealth()
        {
            int MinionHealthTotal = 0;

            foreach (var otherNPC in Main.ActiveNPCs)
            {
                if (otherNPC != null && otherNPC.ModNPC is MewtwoBossMinion minion)
                {
                    if (minion.ParentIndex == boss.NPC.whoAmI)
                    {
                        MinionHealthTotal += otherNPC.life;
                    }
                }
            }
            return MinionHealthTotal;
        }

        public override Vector2 StateAI()
        {
            if (boss is MewtwoBossBody mewtwo)
            {
                SyncMinionLife(mewtwo);
                return mewtwo.IdleMovement(false);
            }
            return boss.NPC.velocity * 0.95f;
        }

        public override BossState FindNextState()
        {
            if (GetMinionHealth() <= 0)
            {
                return new MewtwoTransState(boss);
            }
            return new MewtwoIdleState1(boss);
        }
    }

    public class MewtwoTransState : BossState
    {
        public MewtwoTransState(PokemonBossBody bossRef)
        {
            boss = bossRef;
            boss.NPC.damage = MewtwoBossBody.PhysicalDamage;
            boss.drag = 0.85f;
            frameStart = 2;
            frameEnd = 2;
            duration = 300;
            boss.accelerationPower = 0.6f;
        }

        public override Vector2 StateAI()
        {
            float auraAlpha = (float)Math.Pow(boss.StateTimer / duration, 2);
            boss.SetAura(ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/BossProjectiles/MewtwoBossOverheatGlow"), 2, 40, 0, color: new Color(auraAlpha, auraAlpha, auraAlpha, auraAlpha));
            Dust.NewDust(boss.NPC.position, boss.NPC.width, boss.NPC.height, DustID.CursedTorch);

            if (boss.StateTimer % 20 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item34 with { Pitch = -1f * ((duration - boss.StateTimer) / duration) }, boss.NPC.Center);
            }

            return boss.NPC.velocity;
        }

        public override BossState FindNextState()
        {
            boss.drag = 0.95f;
            boss.NPC.dontTakeDamage = false;
            boss.SetAura(null, active: false);
            boss.neutralState = new MewtwoIdleState2(boss);
            return new MewtwoOverheatState(boss, 4);
        }
    }

    public class MewtwoIdleState2 : BossState
    {
        public MewtwoIdleState2(PokemonBossBody bossRef)
        {
            boss = bossRef;
            float rageScale = (2 - (boss.NPC.life / boss.NPC.lifeMax));
            duration = (int)(Main.rand.Next(80, 120) / rageScale);
            frameStart = 5;
            frameEnd = 7;
        }

        public override Vector2 StateAI()
        {
            if (boss is MewtwoBossBody mewtwo)
            {
                return mewtwo.IdleMovement();
            }
            return boss.NPC.velocity * 0.95f;
        }

        public override BossState FindNextState()
        {
            int selection = 0;
            int rageScale = 3 + boss.NPC.life / boss.NPC.lifeMax * 100;
            if (!Main.rand.NextBool(rageScale))
            {
                int distanceToTarget = (int)(boss.targetPlayer.Center - boss.NPC.Center).Length();
                if (distanceToTarget < 1200)
                {
                    selection = Main.rand.Next(4) + 1;
                    if (selection == boss.lastActiveState) selection++;
                }
            }
            BossState nextState;
            switch (selection)
            {
                case 1:
                    nextState = new MewtwoOverheatState(boss);
                    break;
                case 2:
                    nextState = new MewtwoFlameWheelState(boss);
                    break;
                case 3:
                    nextState = new MewtwoShadowBallState(boss);
                    break;
                case 4:
                    nextState = new MewtwoPsychicState(boss);
                    break;
                default:
                    nextState = new MewtwoTeleportState(boss);
                    break;
            }
            return nextState;
        }
    }

    public class MewtwoOverheatState : BossState
    {
        private bool triggered;
        public Projectile overheatProj;
        public MewtwoOverheatState(PokemonBossBody bossRef, int transitionFrame = 3)
        {
            boss = bossRef;
            frameStart = transitionFrame;
            frameEnd = 4;
            ticksPerFrame = 60;
            boss.drag = 0.7f;
            triggered = false;
        }

        public void Overheat()
        {
            SoundEngine.PlaySound(SoundID.Item74, boss.NPC.position);

            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            boss.NPC.netUpdate = true;

            //shoot projectile
            var source = boss.NPC.GetSource_FromAI();
            Vector2 position = boss.NPC.Center;
            Vector2 velocity = Vector2.Zero;
            int type = ModContent.ProjectileType<MewtwoBossOverheat>();
            int damage = 60;

            overheatProj = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, 0f, -1, damage, 10f)];
            if (overheatProj.ModProjectile is PokemonBossProjectile bossProjectile)
            {
                bossProjectile.boss = boss;
            }
        }

        public override Vector2 StateAI()
        {
            if (boss.CurrentFrame != frameEnd)
            {
                float auraAlpha = (float)Math.Pow(boss.StateTimer / ticksPerFrame, 2);
                boss.SetAura(ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/BossProjectiles/MewtwoBossOverheatGlow"), 2, ticksPerFrame, 1, color: new Color(auraAlpha, auraAlpha, auraAlpha, auraAlpha));
                Dust.NewDust(boss.NPC.position, boss.NPC.width, boss.NPC.height, DustID.CursedTorch);
            }

            if (boss.CurrentFrame == frameEnd && !triggered)
            {
                Overheat();
                boss.SetAura(null, active: false);
                triggered = true;
            }

            if (overheatProj != null)
            {
                overheatProj.Center = boss.NPC.Center;
            }

            if (boss.StateTimer % 20 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item34 with { Pitch = -0.5f }, boss.NPC.Center);
            }

            if (boss is MewtwoBossBody mewtwo)
            {
                return mewtwo.IdleMovement();
            }
            return boss.NPC.velocity * 0.95f;
        }

        public override BossState FindNextState()
        {
            if (overheatProj != null)
            {
                overheatProj.Kill();
                overheatProj = null;
            }
            boss.drag = 0.95f;
            boss.lastActiveState = 1;
            return new MewtwoIdleState2(boss);
        }
    }

    public class MewtwoFlameWheelState : BossState
    {
        public float dashSpeed = 0;
        int totalEffectFrames = 3;
        public MewtwoFlameWheelState(PokemonBossBody bossRef)
        {
            boss = bossRef;
            frameStart = 8;
            frameEnd = 8;
            ticksPerFrame = 3;
            duration = 40;
            dashSpeed = 18f;
        }

        public void StartEffects()
        {
            boss.SetAura(ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/BossProjectiles/MewtwoBossFlameWheel"), totalFrames: totalEffectFrames, ticksPerFrame: ticksPerFrame, color: new Color(0.5f, 0.5f, 0.5f, 0.3f));
            //Play lunge sound
            SoundEngine.PlaySound(SoundID.Item117, boss.NPC.Center);
            boss.DustBurst(259, 10, 20, 1f);
        }

        public void TravelEffects()
        {
            Dust.NewDust(boss.NPC.Left - Vector2.UnitY * boss.NPC.width, boss.NPC.width, boss.NPC.width, DustID.CursedTorch);

            //dust trail
        }

        public void AlignToVelocity(Vector2 velocity)
        {
            Vector2 movementDirection = boss.NPC.direction * velocity;
            boss.NPC.rotation = movementDirection.ToRotation();
        }

        public override Vector2 StateAI()
        {
            Vector2 newVelocity = boss.NPC.velocity;
            if (boss.StateTimer == 0) //first frame
            {
                boss.drag = 1f;
                Vector2 direction = (boss.targetPlayer.Center - boss.NPC.Center).SafeNormalize(-Vector2.UnitY);
                newVelocity = direction * dashSpeed;
            }
            if (boss.StateTimer == 1)
            {
                StartEffects();
            }
            if (boss.StateTimer > 1)
            {
                TravelEffects();
            }
            AlignToVelocity(newVelocity);
            return newVelocity;
        }

        public override BossState FindNextState()
        {
            boss.drag = 0.95f;
            boss.NPC.rotation = 0f;
            boss.SetAura(null, active: false);
            bool repeat = Main.rand.NextBool();

            boss.lastActiveState = 2;
            return repeat? new MewtwoFlameWheelState(boss) : new MewtwoIdleState2(boss);
        }
    }

    public class MewtwoPsychicState : BossState
    {
        public Vector2 headPosition;
        public MewtwoPsychicState(PokemonBossBody bossRef)
        {
            boss = bossRef;
            frameStart = 4;
            frameEnd = 4;
            ticksPerFrame = 60;
            duration = 90;
        }

        public void Psychic()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            boss.NPC.netUpdate = true;

            //shoot projectile
            int projectileSpeed = 15;
            var source = boss.NPC.GetSource_FromAI();
            Vector2 position = headPosition;
            Vector2 direction = Main.rand.NextVector2Unit();
            Vector2 velocity = direction * projectileSpeed;
            int type = ModContent.ProjectileType<Psychic>();
            int damage = 100;

            Projectile bullet = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, 0f, -1)];
            bullet.scale = 1.7f;
            bullet.friendly = false;
            bullet.hostile = true;
            bullet.timeLeft = 360;

            if (bullet.ModProjectile is PokemonAttack pokemonBullet)
            {
                //pokemonBullet.wildOwner = boss.NPC;
            }

            //play shoot sound
            SoundEngine.PlaySound(SoundID.Item42, headPosition);

            boss.DustBurst(134, 2, 9, 1f, headPosition);
        }

        public override Vector2 StateAI()
        {
            headPosition = boss.NPC.Center + new Vector2(boss.NPC.direction * 5, -43) * 2f;

            if (boss.StateTimer < ticksPerFrame) 
            {
                if (boss.StateTimer % 20 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item15 with { Pitch = 0.6f, Volume = 0.3f + boss.StateTimer * 0.8f / (ticksPerFrame) }, boss.NPC.Center);
                }
                boss.DustBurst(255, 1, 5, 0.7f, headPosition);
            }
            else if (boss.StateTimer % ((duration - ticksPerFrame) / 8) == 0) //Three points following frame change
            {
                Psychic();
            }
            if (boss is MewtwoBossBody mewtwo)
            {
                return mewtwo.IdleMovement();
            }
            return boss.NPC.velocity;
        }

        public override BossState FindNextState()
        {
            boss.lastActiveState = 3;
            return new MewtwoIdleState2(boss);
        }
    }

    public class MewtwoShadowBallState : BossState
    {
        private Vector2 handPosition;
        public MewtwoShadowBallState(PokemonBossBody bossRef)
        {
            boss = bossRef;
            frameStart = 9;
            frameEnd = 9;
            ticksPerFrame = 40;
            duration = 60;
        }

        public void StartEffects()
        {
            boss.SetAura(ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/BossProjectiles/MewtwoBossShadowBallCharge"), 8, 5, color: Color.White);
            SoundEngine.PlaySound(SoundID.Item103, boss.NPC.Center);
        }

        public Vector2 ShadowBall()
        {
            int recoilPower = 10;
            boss.drag = 1f;
            Vector2 direction = (boss.targetPlayer.Center - handPosition).SafeNormalize(boss.NPC.direction * Vector2.UnitX);

            if (Main.netMode == NetmodeID.MultiplayerClient) return -direction * recoilPower;
            boss.NPC.netUpdate = true;

            boss.SetAura(null, active: false);

            //shoot projectile
            int projectileSpeed = 30;
            var source = boss.NPC.GetSource_FromAI();
            Vector2 position = handPosition;
            
            Vector2 velocity = direction * projectileSpeed;
            int type = ModContent.ProjectileType<MewtwoBossShadowBall>();
            int damage = 80;

            Projectile bullet = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, 0f, -1, damage, 1.5f)];
            bullet.scale = 3f;
            bullet.friendly = false;
            bullet.hostile = true;

            if (bullet.ModProjectile is PokemonAttack pokemonBullet)
            {
                //pokemonBullet.wildOwner = boss.NPC;
            }

            boss.DustBurst(DustID.Shadowflame, 10, 30, 2f, handPosition);
            SoundEngine.PlaySound(SoundID.Item77, boss.NPC.Center);

            return -direction * recoilPower;
        }

        public override Vector2 StateAI()
        {
            handPosition = boss.NPC.Center + new Vector2(boss.NPC.direction * 41, -22) * 2f;

            if (boss.StateTimer == 0) //first frame
            {
                StartEffects();
            }
            if (boss.StateTimer == ticksPerFrame)
            {
                return ShadowBall();
            }
            return boss.NPC.velocity;
        }

        public override BossState FindNextState()
        {
            boss.drag = 0.95f;
            boss.lastActiveState = 4;
            return new MewtwoIdleState2(boss);
        }
    }

    public class MewtwoTeleportState : BossState
    {
        int totalEffectFrames = 4;
        public MewtwoTeleportState(PokemonBossBody bossRef)
        {
            boss = bossRef;
            ticksPerFrame = (boss.NPC.life / boss.NPC.lifeMax < 0.5)? 3 : 6;
            duration = ticksPerFrame * totalEffectFrames * 2;
            boss.SetAura(ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/BossProjectiles/MewtwoBossTeleport"), totalFrames: totalEffectFrames, ticksPerFrame: ticksPerFrame, color: Color.White);
        }

        public Vector2 FindTeleportPosition()
        {
            int teleportRange = 380;
            Vector2 target = boss.targetPlayer.Center + Main.rand.NextVector2Unit() * teleportRange;
            return target;
        }

        public void Teleport()
        {
            boss.NPC.netUpdate = true;
            boss.DustBurst(74, 7, 12, 1f);
            //play sound
            SoundEngine.PlaySound(SoundID.Item24, boss.NPC.Center);
            boss.NPC.Center = FindTeleportPosition();
            boss.SetAura(ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/BossProjectiles/MewtwoBossTeleport"), totalFrames: totalEffectFrames, ticksPerFrame: -ticksPerFrame, color: Color.White);
        }

        public void StartEffects()
        {
            boss.bodyVisible = false;
            //play sound
        }

        public override Vector2 StateAI()
        {
            if (boss.StateTimer == 0) //first frame
            {
                StartEffects();
            }
            if (boss.StateTimer == duration / 2) //On teleport
            {
                Teleport();
            }
            return new Vector2(0, 0);
        }

        public override BossState FindNextState()
        {
            //Boss reappears
            boss.bodyVisible = true;
            boss.SetAura(null, active: false);

            int selection = 5;
            int rageScale = 3 + boss.NPC.life / boss.NPC.lifeMax * 200;
            if (!Main.rand.NextBool(rageScale))
            {
                selection = Main.rand.Next(6);
            }
            BossState nextState;
            switch (selection)
            {
                case 1:
                    nextState = new MewtwoOverheatState(boss);
                    break;
                case 2:
                    nextState = new MewtwoFlameWheelState(boss);
                    break;
                case 3:
                    nextState = new MewtwoPsychicState(boss);
                    break;
                case 4:
                    nextState = new MewtwoShadowBallState(boss);
                    break;
                case 5:
                    nextState = new MewtwoTeleportState(boss);
                    break;
                default:
                    nextState = new MewtwoIdleState2(boss);
                    break;
            }
            return nextState;
        }
    }
}

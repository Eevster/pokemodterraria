using System.Reflection;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items.Apricorns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Build.Tasks;
using Terraria.GameContent.Drawing;
using ReLogic.Content;
using Pokemod.Content.TileEntities;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ObjectInteractions;
using Terraria.GameContent;
using Terraria.Audio;
using Pokemod.Content.Items;
using Pokemod.Content.Pets;
using Pokemod.Content.Buffs;

namespace Pokemod.Content.TitleScreens;

public class PokemodMenu : ModMenu
{
    public override string DisplayName => "Pokemod";
    


    public override Asset<Texture2D> Logo =>
        ModContent.Request<Texture2D>("Pokemod/Assets/Textures/PokemodTitle");

    public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation,
        ref float logoScale,
        ref Color drawColor)
    {
        /*var logoTexture = Logo.Value;
        var b = (byte)((255 + Main.tileColor.R * 2) / 3);
        var color = new Color(b, b, b, 255);
        logoDrawCenter.Y += 16;
        spriteBatch.Draw(logoTexture, logoDrawCenter, new Rectangle(0, 0, logoTexture.Width, logoTexture.Height),
            color, logoRotation, new Vector2(logoTexture.Width * 0.5f, logoTexture.Height * 0.5f), logoScale,
            SpriteEffects.None, 0f);
        return false;*/

        logoDrawCenter.Y += 16;
        return true;
    }

    
    public void ForceSwitchToThis()
    {
        var menuLoaderType = typeof(MenuLoader);
        menuLoaderType.GetField("switchToMenu", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, this);
        menuLoaderType.GetField("LastSelectedModMenu", BindingFlags.NonPublic | BindingFlags.Static)?
            .SetValue(null, FullName);
    }
}
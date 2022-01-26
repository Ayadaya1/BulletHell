using System;
using System.Collections.Generic;
using System.Text;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace game
{
    class Bonus : Entity
    {
        private static Texture texture;
        public string BonusType;
        public bool isDisposed = false;
        public Bonus(string bonusType, Enemy e) : base()
        {
            sprite = new Sprite(texture);
            BonusType = bonusType;
            if (BonusType == "Speed")
            {
                sprite.TextureRect = new IntRect(new Vector2i(24,200), new Vector2i(108, 212));
            }
            if (BonusType == "HP")
            {
                sprite.TextureRect = new IntRect(new Vector2i(266, 200), new Vector2i(108, 212));
            }
            if (BonusType == "AttackSpeed")
            {
                sprite.TextureRect = new IntRect(new Vector2i(140, 200), new Vector2i(108, 212));
            }
            if(BonusType == "Freezing")
            {
                sprite.TextureRect = new IntRect(new Vector2i(266, 200), new Vector2i(108, 212));
                sprite.Scale = new Vector2f(2f, 2f);
            }
            coordinates = new Vector2f(e.coordinates.X + e.sprite.GetGlobalBounds().Width / 2, e.coordinates.Y);
            sprite.Scale = new Vector2f(0.5f, 0.5f);
        }
        public void Fall()
        {
            if (!isDisposed)
                coordinates.Y += 3;
        }
        public void PickUp(Player player)
        {
            if (!isDisposed && !player.isDead)
            {
                if (Collides(player))
                {
                    //Console.WriteLine("Bonus picked up");
                    if (BonusType == "Speed")
                    {
                        if (player.movespeed < Player.MAX_MOVESPEED)
                        {
                            player.movespeed++;
                        }
                    }
                    if (BonusType == "AttackSpeed")
                    {
                        if (player.AttackSpeed < Player.MAX_ATTACK_SPEED)
                        {
                            player.AttackSpeed++;
                        }
                    }
                    if (BonusType == "HP")
                    {
                        player.AddHealth();
                    }
                    if (BonusType == "Upgrade")
                    {
                        player.UpgradePickUp();
                    }
                    if(BonusType=="TwinStrike")
                    {
                        player.twinStrike = true;
                    }
                    isDisposed = true;
                }
            }
        }
        public void Clear()
        {
            if (isDisposed)
            {
                sprite.Dispose();
                //tex.Dispose();
            }
        }
        new public static void LoadContent(string path)
        {
            texture = new Texture(path);
        }
    }
}
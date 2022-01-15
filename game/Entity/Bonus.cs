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
        public string BonusType;
        public bool isDisposed = false;
        public Bonus(string texPath, string bonusType, Enemy e) : base(texPath)
        {
            BonusType = bonusType;
            if (BonusType == "Speed")
            {
                sprite.Color = Color.Yellow;
            }
            if (BonusType == "HP")
            {
                sprite.Color = Color.Red;
            }
            if (BonusType == "AttackSpeed")
            {
                sprite.Color = Color.Green;
            }
            coordinates = new Vector2f(e.coordinates.X + e.sprite.GetGlobalBounds().Width / 2, e.coordinates.Y);
            sprite.Scale = new Vector2f(0.25f, 0.25f);
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
                    isDisposed = true;
                }
            }
        }
        public void Clear()
        {
            if (isDisposed)
            {
                sprite.Dispose();
                tex.Dispose();
            }
        }
    }
}
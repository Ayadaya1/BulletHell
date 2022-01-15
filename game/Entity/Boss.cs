using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace game
{
    class Boss : Enemy
    {
        private int shootingX = 130;
        public bool hasSpawned = false;
        private int phase = 0;
        private byte transparency = 0;
        public Boss(string path, int x, int y) : base(path, x, y)
        {
            sprite.Scale = new Vector2f(0.25f, 0.25f);
            sprite.TextureRect = new IntRect(new Vector2i(304, 96), new Vector2i(1488, 1696));
            Color color = sprite.Color;
            color.A = 0;
            sprite.Color = color;
            Health = 100;
        }
        public void Spawn()
        {
            if (!hasSpawned)
            {
                transparency++;
                Color color = sprite.Color;
                color.A = transparency;
                sprite.Color = color;
                if (transparency == 100)
                    hasSpawned = true;
            }
        }
        public override void Shoot(Player player)
        {
            if (!isDead && ticks % 15 == 0)
            {
                int y = Convert.ToInt32(coordinates.Y + 15 + sprite.GetGlobalBounds().Height);
                if (shootingX < 680)
                {
                    Bullet b = new Bullet("Z:\\progs\\game\\game\\Graphics\\Bullet.png", shootingX, y, 0, 0);
                    bullets.AddLast(b);
                    shootingX += 115;
                }
                else
                {
                    foreach (Bullet b in bullets)
                    {
                        if (!b.isDisposed)
                        {
                            b.XSpeed = 0;
                            b.YSpeed = 5;
                        }
                    }
                    phase++;
                    if (phase % 2 == 0)
                        shootingX = 130;
                    else
                        shootingX = 180;
                }
            }
            foreach (Bullet b in bullets)
            {
                if (!b.isDisposed)
                {
                    if (isDead)
                    {
                        b.XSpeed = 0;
                        b.YSpeed = 5;
                    }
                    b.fly();
                    b.Update();
                    b.Animate();
                }
            }
            player.Hurt(bullets);
        }
    }
}
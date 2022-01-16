using System;
using System.Collections.Generic;
using System.Text;

namespace game
{
    class HomingBulletGuy : Enemy
    {
        public HomingBulletGuy(string Path, int x, int y) : base(Path, x, y)
        {
            bullets = new LinkedList<Bullet>();
            Health = 8;
        }
        public override void Shoot(Player player)
        {
            if (!player.isDead)
            {
                int x = Convert.ToInt32(player.coordinates.X + player.sprite.GetGlobalBounds().Width / 2);
                int y = Convert.ToInt32(player.coordinates.Y + player.sprite.GetGlobalBounds().Height / 2);
                if (!isDead)
                {
                    if (ticks % 58 == 0)
                    {
                        Bullet bul = new Bullet("Z:\\progs\\game\\game\\Graphics\\Bullet.png", this, 1, 1);
                        bullets.AddLast(bul);
                    }
                    foreach (Bullet b in bullets)
                    {
                        if (b.coordinates.X < x)
                        {
                            b.XSpeed = 1;
                        }
                        else if (b.coordinates.X > x)
                        {
                            b.XSpeed = -1;
                        }
                        else
                        {
                            b.XSpeed = 0;
                        }
                        if (b.coordinates.Y < y)
                        {
                            b.YSpeed = 1;
                        }
                        else if (b.coordinates.Y > y)
                        {
                            b.YSpeed = -1;
                        }
                        else
                        {
                            b.YSpeed = 0;
                        }
                    }
                }
                Clean();
                foreach (Bullet b in bullets)
                {
                    if (!b.isDisposed)
                    {
                        b.fly();
                        b.Animate();
                        b.Update();
                    }
                }
                player.Hurt(bullets);
            }

        }
        public override int GetHashCode()
        {
            return 3;
        }
    }
}
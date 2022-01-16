using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace game
{
    class Enemy : Entity
    {
        protected int Health;
        public bool isDead = false;
        public LinkedList<Bullet> bullets = new LinkedList<Bullet>();
        public Enemy(string path) : base(path)
        {
            Health = 5;
            sprite.Scale = new Vector2f(1.5f, 1.5f);
            //coordinates = new Vector2f(300, 100);
            sprite.TextureRect = new IntRect(new Vector2i(14, 10), new Vector2i(34, 44));
            //sprite.Position = coordinates;
            //Console.WriteLine(sprite.GetGlobalBounds().Left+", "+sprite.GetGlobalBounds().Left+sprite.GetGlobalBounds().Width);
            bullets = new LinkedList<Bullet>();
            Color color = sprite.Color;
            color.A = 100;
            sprite.Color = color;
        }
        public Enemy(string path, int x, int y) : this(path)
        {
            Health = 5;
            sprite.Scale = new Vector2f(1.5f, 1.5f);
            coordinates = new Vector2f(300, 100);
            sprite.TextureRect = new IntRect(new Vector2i(14, 10), new Vector2i(34, 44));
            sprite.Position = coordinates;
            //Console.WriteLine(sprite.GetGlobalBounds().Left + ", " + sprite.GetGlobalBounds().Left + sprite.GetGlobalBounds().Width);
            coordinates = new Vector2f(x, y);
            bullets = new LinkedList<Bullet>();
            Color color = sprite.Color;
            color.A = 100;
            sprite.Color = color;
        }
        public void Death(Bullet bullet, LinkedList<Bonus> bonuses)
        {
            if (!isDead && !bullet.isDisposed)
            {
                if (this.Collides(bullet))
                {
                    Health--;
                    if (Health == 0)
                    {
                        isDead = true;
                        Random rand = new Random();
                        int value = rand.Next(100);
                        if (value <= 18)
                        {
                            if (value <= 10)
                            {
                                Bonus bonus = new Bonus("Z:\\progs\\game\\game\\Graphics\\Heart.png", "HP", this);
                                bonuses.AddLast(bonus);
                                //Console.WriteLine("AAAAAAAAAAAAAAA");
                            }
                            else if (value <= 13)
                            {
                                Bonus bonus = new Bonus("Z:\\progs\\game\\game\\Graphics\\Heart.png", "AttackSpeed", this);
                                bonuses.AddLast(bonus);
                            }
                            else
                            {
                                Bonus bonus = new Bonus("Z:\\progs\\game\\game\\Graphics\\Heart.png", "Speed", this);
                                bonuses.AddLast(bonus);
                            }

                        }
                        //Console.WriteLine("Random:"+value);
                        sprite.Dispose();
                        tex.Dispose();

                    }
                    bullet.sprite.Dispose();
                    bullet.tex.Dispose();
                    bullet.isDisposed = true;
                }
            }
        }
        public virtual void Shoot(Player player)
        {
            if (!isDead)
            {
                if (ticks % 60 == 0)
                {
                    int xspeed;
                    int yspeed;
                    //int yspeed = 1;
                    /*int xspeed = Convert.ToInt32(MathF.Sqrt((MathF.Pow(coordinates.Y-player.coordinates.Y,2f)+MathF.Pow(coordinates.X-player.coordinates.X,2f)))/-(coordinates.Y-player.coordinates.Y));*/
                    try
                    {
                        xspeed = Convert.ToInt32((player.coordinates.X - coordinates.X) / -(coordinates.Y - player.coordinates.Y));
                        yspeed = Convert.ToInt32((player.coordinates.Y - coordinates.Y) / MathF.Abs((coordinates.X - player.coordinates.X)));
                    }
                    catch (System.OverflowException)
                    {
                        xspeed = 0;
                        yspeed = 5;
                    }

                    if (yspeed > 5)
                        yspeed = 5;
                    if (xspeed > 5)
                        xspeed = 5;
                    if (yspeed < -5)
                        yspeed = -5;
                    //Console.WriteLine(xspeed+" "+yspeed);
                    Bullet bul = new Bullet("Z:\\progs\\game\\game\\Graphics\\Bullet.png", this, xspeed, yspeed);
                    bullets.AddLast(bul);
                }
            }
            Clean();
            foreach (Bullet b in bullets)
            {
                b.fly();
                b.Animate();
                b.Update();
            }
            player.Hurt(bullets);
            Clean();
        }
        protected void Clean()
        {
            Bullet[] buls = new Bullet[bullets.Count];
            if (buls.Length > 0)
            {
                bullets.CopyTo(buls, 0);
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (buls[i].isDisposed)
                    {
                        bullets.Remove(buls[i]);
                        Console.WriteLine(bullets.Count+" Bullets");
                    }
                }
            }
        }
        public override int GetHashCode()
        {
            return 2;
        }
    }
}
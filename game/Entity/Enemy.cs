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
        private static Texture texture;
        protected int Health;
        public bool isDead = false;
        public bool Frozen = false;
        public int FreezeTicks = 0;
        private bool StartedFreezing;
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
            sprite.Scale = new Vector2f(0.75f, 0.75f);
            sprite.TextureRect = new IntRect(new Vector2i(6, 26), new Vector2i(56, 92));
            sprite.Position = coordinates;
            //Console.WriteLine(sprite.GetGlobalBounds().Left + ", " + sprite.GetGlobalBounds().Left + sprite.GetGlobalBounds().Width);
            coordinates = new Vector2f(x, y);
            bullets = new LinkedList<Bullet>();
            Color color = sprite.Color;
            color.A = 100;
            sprite.Color = color;
        }
        public Enemy(int x,int y):base(x,y)
        {
            sprite = new Sprite(texture);
            Health = 5;
            //sprite.Scale = new Vector2f(1.5f, 1.5f);
            sprite.TextureRect = new IntRect(new Vector2i(6, 26), new Vector2i(56, 92));
            bullets = new LinkedList<Bullet>();
            Color color = sprite.Color;
            color.A = 100;
            sprite.Color = color;
        }
        public Enemy():base()
        {

        }
        public void Death(Bullet bullet, LinkedList<Bonus> bonuses, Player player)
        {
            if (!isDead && !bullet.isDisposed)
            {
                if (this.Collides(bullet))
                {
                    Health-=player.Damage;
                    if(player.IceBullets)
                    {
                        Freeze();
                    }
                    if (Health <= 0)
                    {
                        isDead = true;
                        Random rand = new Random();
                        int value = rand.Next(100);
                        if (value <= 18)
                        {
                            if (value <= 10 && (!player.HeartOfIce||player.type!="Ice")&&player.Health<5)
                            {
                                Bonus bonus = new Bonus("HP", this);
                                bonuses.AddLast(bonus);
                                //Console.WriteLine("AAAAAAAAAAAAAAA");
                            }
                            else if (value <= 13&&value>10)
                            {
                                Bonus bonus = new Bonus("AttackSpeed", this);
                                bonuses.AddLast(bonus);
                            }
                            else if(value>13)
                            {
                                Bonus bonus = new Bonus("Speed", this);
                                bonuses.AddLast(bonus);
                            }
                            Console.WriteLine("Bonus dropped");

                        }
                        //Console.WriteLine("Random:"+value);
                        if(GetHashCode()!=4)
                            sprite.Dispose();
                        //tex.Dispose();

                    }
                    bullet.sprite.Dispose();
                    //bullet.tex.Dispose();
                    bullet.isDisposed = true;
                }
            }
        }
        public virtual void Shoot(Player player)
        {
            if (!isDead&&!Frozen)
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
                    Bullet bul = new Bullet( this, xspeed, yspeed);
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
        new public static void LoadContent(string path)
        {
            texture = new Texture(path);
        }
        private void Freeze()
        {
            StartedFreezing = true;
        }
        public void Freezing()
        {
            if(StartedFreezing&&!Frozen)
            {
                Frozen = true;
                FreezeTicks = ticks;
            }
            if(ticks-FreezeTicks>=120&&Frozen)
            {
                Frozen = false;
                StartedFreezing = false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace game
{
    public class Player:Entity
    {
        public bool isInvincible = false;
        public bool inInvincibility = false;
        private int startingTicks = 0;
        private int Health;
        public bool isDead= false;
        public int movespeed;
        private bool canMove = true;
        private bool canShoot = true;
        //private static string path = "Z:\\progs\\game\\game\\Graphics\\heart.png";
        public Player(string path):base(path)
        {
            Health = 10;
            sprite.TextureRect = new IntRect(new Vector2i(4,30), new Vector2i(56,42));
            sprite.Scale = new Vector2f(1.5f,1.5f);
            movespeed = 3;
            
        }
        public void Move()
        {
            if (!isDead&&canMove)
            {
                bool MovingRight = Keyboard.IsKeyPressed(Keyboard.Key.Right);
                bool MovingLeft = Keyboard.IsKeyPressed(Keyboard.Key.Left);
                bool MovingUp = Keyboard.IsKeyPressed(Keyboard.Key.Up);
                bool MovingDown = Keyboard.IsKeyPressed(Keyboard.Key.Down);
                if (MovingDown && sprite.GetGlobalBounds().Height + coordinates.Y <= 640)
                {
                    coordinates.Y += movespeed;
                }
                if (MovingUp && coordinates.Y >= 0)
                {
                    coordinates.Y -= movespeed;
                }
                if (MovingLeft && coordinates.X >= 120)
                {
                    coordinates.X -= movespeed;
                }
                if (MovingRight && coordinates.X + sprite.GetGlobalBounds().Width <= 680)
                {
                    coordinates.X += movespeed;
                }
            }
        }
        public void Shoot(LinkedList<Bullet> bullets, GameLoop gameLoop, int ticks)
        {
            if (!isDead&&canShoot)
            {
                bool Shooting = Keyboard.IsKeyPressed(Keyboard.Key.Z);
                if (Shooting && gameLoop.TotalTicksBeforeShooting >= ticks)
                {
                    Bullet bul = new Bullet("Z:\\progs\\game\\game\\Graphics\\Bullet.png", this, 0, -5);
                    bullets.AddLast(bul);
                    //Console.WriteLine(bullets.Count);
                    gameLoop.TotalTicksBeforeShooting = 0;
                    //Console.WriteLine(bul.coordinates.X.ToString()+", "+(bul.coordinates.X + bul.sprite.GetLocalBounds().Width).ToString() );

                }
            }
        }
        public void Hurt(LinkedList<Bullet> bullets)
        {
            if (!isDead)
            {
                foreach (Bullet b in bullets)
                {
                    if (!b.isDisposed)
                    {
                        if (this.Collides(b))
                        {
                            b.isDisposed = true;
                            if (!isInvincible)
                            {
                                Health--;
                                isInvincible = true;
                            }
                            b.sprite.Dispose();
                            b.tex.Dispose();

                        }
                    }
                }
            }
        }
        public void Death(GameTime gameTime)
        {
            if(Health==0)
            {
                isDead = true;
                sprite.Dispose();
                tex.Dispose();
                gameTime.Pause();
            }
        }
        public void Invincibility()
        {
            if (isInvincible == true && inInvincibility == false)
            {
                inInvincibility = true;
                startingTicks = ticks;
                Console.WriteLine("Entered Invincibility");
            }
            if(isInvincible&&inInvincibility)
            {
                if(ticks-startingTicks==180)
                {
                    isInvincible = false;
                    inInvincibility = false;
                    Console.WriteLine("Left Invincibility");
                }
            }
        }
        public void DisableMovement()
        {
            canMove = false;
        }
        public void EnableMovement()
        {
            canMove = true;
        }
        public void DisableShooting()
        {
            canShoot = false;
        }
        public void EnableShooting()
        {
            canShoot = true;
        }
    }
}

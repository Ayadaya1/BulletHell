using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace game
{
    public class Player : Entity
    {
        public string type;
        public bool isInvincible = false;
        public bool inInvincibility = false;
        private int startingTicks = 0;
        public int Health;
        public Shield shield;
        public bool isDead = false;
        public int movespeed;
        public int AttackSpeed;
        public const int MAX_MOVESPEED = 6;
        public const int MAX_ATTACK_SPEED = 8;
        private bool canMove = true;
        private bool canShoot = true;
        private const int STARTING_X = 350;
        private const int STARTING_Y = 500;
        private const int STARTING_HEALTH = 2;
        private const int STARTING_SPEED = 3;
        private const int STARTING_ATTACK_SPEED = 1;
        //private static string path = "Z:\\progs\\game\\game\\Graphics\\heart.png";
        public Player(string path) : base(path)
        {
            type = "Ice";
            coordinates = new Vector2f(STARTING_X,STARTING_Y);
            Health = STARTING_HEALTH;
            sprite.TextureRect = new IntRect(new Vector2i(4, 30), new Vector2i(56, 42));
            sprite.Scale = new Vector2f(1.2f, 1.2f);
            movespeed = STARTING_SPEED;
            AttackSpeed = STARTING_ATTACK_SPEED;
            shield = new Shield("Z:\\progs\\game\\game\\Graphics\\shield.png", this);
        }
        public void Move()
        {
            if (!isDead && canMove)
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
        public void Shoot(LinkedList<Bullet> bullets, GameLoop gameLoop)
        {
            if (!isDead && canShoot)
            {
                bool Shooting = Keyboard.IsKeyPressed(Keyboard.Key.Z);
                if (Shooting && gameLoop.TotalTicksBeforeShooting >= 11 - AttackSpeed)
                {
                    Bullet bul = new Bullet("Z:\\progs\\game\\game\\Graphics\\all.png", this, 0, -5);
                    bul.sprite.TextureRect = new IntRect(new Vector2i(31, 26), new Vector2i(9, 11));
                    bul.sprite.Scale = new Vector2f(2.5f, 2.5f);
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
                                if(Health>0)
                                    isInvincible = true;
                            }
                            b.sprite.Dispose();
                            b.tex.Dispose();
                            b.Dispose();
                        }
                    }
                }
            }
        }
        public void Death(GameTime gameTime)
        {
            if (Health == 0)
            {
                isDead = true;
                //sprite.Dispose();
                //tex.Dispose();
                //gameTime.Pause();
            }
        }
        public void Invincibility()
        {
            if (isInvincible == true && inInvincibility == false && Health > 0)
            {
                inInvincibility = true;
                startingTicks = ticks;
                shield.Enable();
                //Console.WriteLine("Entered Invincibility");
            }
            if (isInvincible && inInvincibility)
            {
                if (ticks - startingTicks == 180)
                {
                    isInvincible = false;
                    inInvincibility = false;
                    shield.Disable();
                    //Console.WriteLine("Left Invincibility");
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
        public void AddHealth()
        {
            Health++;
            Console.WriteLine(Health);
        }
        public void Respawn()
        {
            Health = STARTING_HEALTH;
            coordinates = new Vector2f(STARTING_X, STARTING_Y);
            movespeed = STARTING_SPEED;
            AttackSpeed = STARTING_ATTACK_SPEED;
            isDead = false;
        }
        public override int GetHashCode()
        {
            return 1;
        }

    }
}

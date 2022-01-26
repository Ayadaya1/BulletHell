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
        public bool HeartOfIce;
        public string type;
        public bool isInvincible = false;
        public bool IceBullets;
        public bool inInvincibility = false;
        public int TwinStrikeTicks = 0;
        private int startingTicks = 0;
        private int MeltingTicks = 0;
        public bool twinStrike = false;
        public bool TwinStrikeActive = false;
        public int Health;
        public Shield shield;
        public bool isDead = false;
        public int movespeed;
        public int AttackSpeed;
        public bool Upgrading=false;
        public bool UnderUpgrade = false;
        public int UpgradingTicks = 0;
        public const int MAX_MOVESPEED = 6;
        public int Damage = 1;
        public const int MAX_ATTACK_SPEED = 8;
        private bool canMove = true;
        private bool canShoot = true;
        private bool startedMelting = false;
        private const int STARTING_X = 350;
        private const int STARTING_Y = 500;
        private const int STARTING_HEALTH = 2;
        private const int GIRL_STARTING_HEALTH = 3;
        private const int STARTING_SPEED = 3;
        private const int STARTING_ATTACK_SPEED = 1;
        public bool Upgraded = false;
        private static Texture IceBoy;
        private static Texture SomeGirl;
        public Player() : base()
        {
            coordinates = new Vector2f(STARTING_X,STARTING_Y);
            if (type == "Ice")
            {
                Health = STARTING_HEALTH;
                HeartOfIce = true;
                sprite = new Sprite(IceBoy);
                sprite.TextureRect = new IntRect(new Vector2i(4, 30), new Vector2i(56, 42));
                sprite.Scale = new Vector2f(1.2f, 1.2f);
            }
            else
            {
                Health = STARTING_HEALTH;
                HeartOfIce = false;
                sprite = new Sprite(SomeGirl);
                sprite.TextureRect = new IntRect(new Vector2i(4, 30), new Vector2i(56, 42));
                sprite.Scale = new Vector2f(1.2f, 1.2f);
            }
            movespeed = STARTING_SPEED;
            AttackSpeed = STARTING_ATTACK_SPEED;
            shield = new Shield(this);
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
                    if (!TwinStrikeActive)
                    {
                        Bullet bul = new Bullet(this, 0, -5);
                        bullets.AddLast(bul);
                    }
                    else
                    {
                        Bullet bul1 = new Bullet(this, 1, -3);
                        Bullet bul2 = new Bullet(this, -1, -3);
                        bullets.AddLast(bul1);
                        bullets.AddLast(bul2);
                    }
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
                                if (type != "Ice" || !HeartOfIce)
                                {
                                    Health--;
                                    startedMelting = false;
                                    HeartOfIce = true;
                                    if (Health > 0)
                                        isInvincible = true;
                                }
                                else if (type == "Ice" && HeartOfIce)
                                {
                                    startedMelting = true;
                                }
                                
                            }
                            b.sprite.Dispose();
                            //b.tex.Dispose();
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
        public void MoveThere()
        {
            coordinates.Y -= 2;
        }
        public void Melt()
        {
            if(HeartOfIce && startedMelting)
            {
                MeltingTicks = ticks;
                HeartOfIce = false;
            }
            else if(ticks-MeltingTicks==600)
            {
                HeartOfIce = true;
                startedMelting = false;
            }
        }
        public static void LoadContent(string path1, string path2)
        {
            IceBoy = new Texture(path1);
            SomeGirl = new Texture(path2);
        }
        public void SetType(string p_type)
        {
            type = p_type;
        }
        public void UpdateType()
        {
            if (type == "Ice")
            {
                Health = STARTING_HEALTH;
                HeartOfIce = true;
                sprite = new Sprite(IceBoy);
                sprite.TextureRect = new IntRect(new Vector2i(4, 30), new Vector2i(56, 42));
                sprite.Scale = new Vector2f(1.2f, 1.2f);
            }
            else
            {
                Health = GIRL_STARTING_HEALTH;
                HeartOfIce = false;
                sprite = new Sprite(SomeGirl);
                sprite.TextureRect = new IntRect(new Vector2i(172, 0), new Vector2i(48, 42));
                sprite.Scale = new Vector2f(1.2f, 1.2f);
            }
        }
        public void UpgradePickUp()
        {
            Upgrading = true;
        }
        public void UpgradeBullets()
        {
            if (Upgrading && !UnderUpgrade)
            {
                UpgradingTicks = ticks;
                UnderUpgrade = true;
            }
            if (Upgrading && UnderUpgrade)
            {
                if (ticks-UpgradingTicks>=600)
                {
                    IceBullets = false;
                    Upgrading = false;
                    UnderUpgrade = false;
                    if (type != "Ice")
                        Damage--;
                    Console.WriteLine("ENDED");
                    Upgraded = false;
                }
                if (type == "Ice")
                {
                    IceBullets = true;
                }
                else if (!Upgraded)
                {
                    Damage++;
                    Upgraded = true;
                }
            }
        }
        public void TwinStrike()
        {
            if(twinStrike&&!TwinStrikeActive)
            {
                TwinStrikeActive = true;
                TwinStrikeTicks = ticks;
            }
            if(ticks - TwinStrikeTicks >=600)
            {
                TwinStrikeActive = false;
                twinStrike = false;
            }
        }
    }
}

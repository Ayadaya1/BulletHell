using System;
using System.Collections.Generic;
using System.Text;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Numerics;

namespace game
{
    public class Game:GameLoop
    {
        public const uint DEFAULT_WINDOW_WIDTH = 800;
        public const uint DEFAULT_WINDOW_HEIGHT = 640;
        public const string WINDOW_TITLE = "da";
        private bool dial = false;
        private bool level_1 = true;
        protected int waves = 1;
        Enemy enemy;
        LinkedList<Enemy> enemies;
        LinkedList<HomingBulletGuy> HBGs;
        Enemy enemy2;
        Player player;
        LinkedList<Bullet> bullets;
        Background Lvl1BG;
        HomingBulletGuy hbg;
        Dialogue loh;
        public Game() : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, WINDOW_TITLE, Color.White)
        {
        }
        public override void LoadContent()
        {
        }
        public override void Initialize()
        {
            player = new Player("Z:\\progs\\game\\game\\Graphics\\male_movement_right.png");
            bullets = new LinkedList<Bullet>();
            enemy = new Enemy("Z:\\progs\\game\\game\\Graphics\\Ghostie.png");
            enemy2 = new Enemy("Z:\\progs\\game\\game\\Graphics\\Ghostie.png",100,100);
            HBGs = new LinkedList<HomingBulletGuy>();
            Lvl1BG = new Background("Z:\\progs\\game\\game\\Graphics\\Lvl1bg.png");
            hbg = new HomingBulletGuy("Z:\\progs\\game\\game\\Graphics\\Ghostie.png", 500, 300);
            string[] phrases = { "Я", "Лох","Да" };
            loh = new Dialogue(phrases);
            enemies = new LinkedList<Enemy>();
            //enemies.AddLast(enemy);
           // enemies.AddLast(enemy2);
            //HBGs.AddLast(hbg);
        }
        public override void Update(GameTime gameTime)
        {
            if (!gameTime.Paused)
            {
                Lvl1BG.Animate();
                Lvl1BG.Update();
                Level_1();
                Player_Control(gameTime);
                //enemy.Shoot(EnemyBullets, player);

                //enemy2.Shoot(EnemyBullets, player);
                enemy2.Update();
                foreach (Enemy e in enemies)
                {

                    e.Update();
                    e.Shoot(player);
                }
                foreach (HomingBulletGuy e in HBGs)
                {

                    e.Update();
                    e.Shoot(player);
                }
                foreach (Bullet b in bullets)
                {
                    b.fly();
                }
                foreach (Bullet b in bullets)
                {
                    b.Animate();
                    b.Update();
                    foreach (Enemy e in enemies)
                    {
                        e.Death(b);
                    }
                    foreach (HomingBulletGuy e in HBGs)
                    {
                        e.Death(b);
                    }
                }
            }
            else
            if(player.isDead)
            {
                Console.WriteLine("Restart?");
                string rs = Console.ReadLine();
                if(rs.ToLower()=="yes")
                {
                    Initialize();
                    gameTime.Paused = false;
                }
                else
                {
                    Window.Close();
                }
            }
           
        }
        public override void Draw(GameTime gameTime)
        {
            Draw(Lvl1BG);
            if (!player.isDead)
                Draw(player);
            foreach(Bullet b in bullets)
            {
                if (b.isDisposed == false)
                {
                    Draw(b);
                }
            }
            foreach(Enemy e in enemies)
            {
                if (!e.isDead)
                    Draw(e);
                foreach(Bullet b in e.bullets)
                {
                    if(!b.isDisposed)
                    {
                        Draw(b);
                    }
                }
            }
            foreach (HomingBulletGuy e in HBGs)
            {
                if (!e.isDead)
                    Draw(e);
                foreach (Bullet b in e.bullets)
                {
                    if (!b.isDisposed)
                    {
                        Draw(b);
                    }
                }
            }
            loh.Draw(this);
        }
        protected void Draw(Entity entity)
        {
            Window.Draw(entity.sprite);
        }
        protected bool StageClear()
        {
            int alive = 0;
            foreach (Enemy e in enemies)
            {
                if (!e.isDead)
                    alive++;
            }
            foreach (HomingBulletGuy e in HBGs)
            {
                if (!e.isDead)
                    alive++;
            }
            if (alive == 0)
                return true;
            return false;
        }
        private void SpawnEnemies(int a, int b)
        {
            int ax= 300;
            int bx = 200;
            for (int i = 0; i < a; i++)
                {
                if(ax<580)
                    enemies.AddLast(new Enemy("Z:\\progs\\game\\game\\Graphics\\Ghostie.png", ax += 100, 100));
                }
            for (int i = 0; i < b; i++)
            {
                if(bx<580)
                     HBGs.AddLast(new HomingBulletGuy("Z:\\progs\\game\\game\\Graphics\\Ghostie.png", bx += 100, 250));
            }
        }
        private void Level_1()
        {
            if (level_1)
            {
                if (waves == 2 && StageClear())
                {
                    player.DisableMovement();
                    player.DisableShooting();
                    player.isInvincible = true;
                    loh.Start();
                    loh.Change();
                    if(loh.isFinished)
                    {
                        player.EnableMovement();
                        player.EnableShooting();
                        level_1 = false;
                    }
                    
                }
                else if (StageClear())
                {
                    Console.WriteLine(waves);
                    if (waves % 3 == 0)
                        SpawnEnemies(waves, waves / 3);
                    else
                        SpawnEnemies(waves, 0);
                    waves++;
                }
                
            }
        }
        private void Player_Control(GameTime gameTime)
        {
            player.Move();
            player.Update();
            player.Invincibility();
            player.Death(gameTime);
            player.Shoot(bullets, this, 10);
        }
    }
}

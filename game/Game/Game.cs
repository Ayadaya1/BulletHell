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
        private bool NonBattle = false;
        private bool isPaused;
        public const uint DEFAULT_WINDOW_WIDTH = 800;
        public const uint DEFAULT_WINDOW_HEIGHT = 640;
        public const string WINDOW_TITLE = "da";
        private bool level_1 = true;
        private bool darkness = false;
        protected int waves = 1;
        GUI gui;
        private int floors = 1;
        LinkedList<Enemy> enemies;
        LinkedList<Bonus> bonuses;
        LinkedList<HomingBulletGuy> HBGs;
        Player player;
        LinkedList<Bullet> bullets;
        Background Lvl1BG;
        Dialogue loh;
        Dialogue EnteringDarkness;
        Dialogue TalkingInTheDark;
        Dialogue OhWow;
        Boss firstBoss;
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
            HBGs = new LinkedList<HomingBulletGuy>();
            Lvl1BG = new Background("Z:\\progs\\game\\game\\Graphics\\Lvl1bg.png");
            string[] phrases = { "Я", "Лох","Да" };
            string[] phrases1 = { "Да что за..?" };
            string[] enteringDarkness = { "Нужно двигаться дальше..." };
            loh = new Dialogue(phrases);
            TalkingInTheDark = new Dialogue(phrases1);
            EnteringDarkness = new Dialogue(enteringDarkness);
            enemies = new LinkedList<Enemy>();
            bonuses = new LinkedList<Bonus>();
            firstBoss = new Boss("Z:\\progs\\game\\game\\Graphics\\scary_face_2.png", 200, 0);
            OhWow = new Dialogue(new string[] { "Жесть.." });
            gui = new GUI(player);
        }
        public override void Update(GameTime gameTime)
        {
            if (!isPaused)
            {
                Lvl1BG.Animate();
                Lvl1BG.Update();
                Level_1();
                Darkness();
                Player_Control(gameTime);
                Enemies_Control();
                Bonuses_Control();
                firstBoss.Update();
                gui.Update(player);
                if (firstBoss.hasSpawned&&OhWow.isFinished)
                {
                    firstBoss.Shoot(player);
                    foreach (Bullet b in bullets)
                    {
                        firstBoss.Death(b,bonuses);
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
                    UnPause();
                    waves = 0;
                    floors = 0;
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
            if(level_1)
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
            foreach(Bullet b in firstBoss.bullets)
            {
                if(!b.isDisposed)
                {
                    Draw(b);
                }
            }
            foreach(Bonus b in bonuses)
            {
                if(!b.isDisposed)
                {
                    Draw(b);
                }
            }
            loh.Draw(this);
            TalkingInTheDark.Draw(this);
            EnteringDarkness.Draw(this);
            OhWow.Draw(this);
            if(!firstBoss.isDead)
            {
                Draw(firstBoss);
            }
            if(player.shield.isOn)
            {
                Draw(player.shield);
            }
            if(!NonBattle)
                gui.Draw(this);
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
                if (waves == 10 && StageClear())
                {
                    player.DisableMovement();
                    player.DisableShooting();
                    player.isInvincible = true;
                    loh.Start();
                    loh.Change();
                    if(loh.isFinished)
                    {
                        //player.EnableMovement();
                        level_1 = false;
                        darkness = true;
                        bullets.Clear();
                        enemies.Clear();
                        HBGs.Clear();
                        //GC.Collect();
                    }
                    
                }
                else if (StageClear())
                {
                    //Console.WriteLine(waves);
                    if (waves % 3 == 0)
                        SpawnEnemies(waves, waves / 3);
                    else
                        SpawnEnemies(waves, 0);
                    waves++;
                }
                
            }
        }
        private void Darkness()
        {
            if (darkness)
            {
                if (!firstBoss.hasSpawned)
                    NonBattle = true;
                else
                    NonBattle = false;
                EnteringDarkness.Start();
                EnteringDarkness.Change();
                if(EnteringDarkness.isFinished)
                {
                    player.EnableMovement();
                }
                if (floors < 3)
                {
                    if (player.coordinates.Y <= 0)
                    {
                        player.coordinates.Y = 640 - player.sprite.GetGlobalBounds().Height;
                        floors++;
                    }
                   
                }
                else
                {
                    TalkingInTheDark.Start();
                    player.DisableMovement();
                    TalkingInTheDark.Change();
                    if(TalkingInTheDark.isFinished)
                    {
                        
                        firstBoss.Spawn();
                        OhWow.Start();
                        OhWow.Change();
                        if (OhWow.isFinished)
                        {
                            player.EnableMovement();
                            player.EnableShooting();
                        }
                    }
                }

            }

        }
        private void Player_Control(GameTime gameTime)
        {
            player.Move();
            player.shield.MoveWithPlayer(player);
            player.shield.Update();
            player.Update();
            player.Invincibility();
            player.Death(gameTime);
            if (player.isDead)
                Pause();
            player.Shoot(bullets, this);
            foreach (Bullet b in bullets)
            {
                b.fly();
            }
            foreach (Bullet b in bullets)
            {
               // b.Animate();
                b.Update();
                foreach (Enemy e in enemies)
                {
                    e.Death(b,bonuses);
                }
                foreach (HomingBulletGuy e in HBGs)
                {
                    e.Death(b,bonuses);
                }
            }
        }
        private void Enemies_Control()
        {
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
        }
        private void Bonuses_Control()
        {
            foreach(Bonus b in bonuses)
            {
                if(!b.isDisposed)
                {
                    b.Fall();
                    b.Update();
                    b.PickUp(player);
                    b.Clear();
                }
            }
        }
        private void Pause()
        {
            isPaused = true;
        }
        private void UnPause()
        {
            isPaused = false;
        }
    }
}

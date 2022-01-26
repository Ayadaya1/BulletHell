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
    public class Game : GameLoop
    {
        bool DemoCompleted = false;
        private long ticks;
        private long startingTicks = 0;
        private bool NonBattle = false;
        private bool isPaused;
        public const uint DEFAULT_WINDOW_WIDTH = 800;
        public const uint DEFAULT_WINDOW_HEIGHT = 640;
        public const string WINDOW_TITLE = "da";
        private bool GameStarted = false;
        private bool level_1 = false;
        private bool darkness = false;
        private bool FirstCutscenePlaying = false;
        private bool showContinue = false;
        private bool characterSelected = false;
        private bool LevelSelected = true;
        private LinkedList<Dialogue> dialogues;
        protected int waves = 1;
        GUI gui;
        private int floors = 0;
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
        Menu Start;
        Menu Continue;
        Music Lvl1Music, DarknessMusic, Boss1Music;
        Cutscene FirstCutscene;
        Cutscene DeathCutscene;
        Dialogue IceBoyFirstDialogue;
        bool canSpeak = true;
        LinkedList<Music> music;
        Music DeathSound;
        CharacterSelect characterSelect;
        Dialogue GirlIntro;
        Dialogue GirlFinish;
        Dialogue GirlEnteringDarkness;
        Dialogue GirlScared;
        Dialogue GirlFreakedOut;
        Dialogue EndDemo;
        Menu LevelSelect;
        public Game() : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, WINDOW_TITLE, Color.White)
        {

        }
        public override void LoadContent()
        {
            GUI.LoadContent("..\\..\\..\\Graphics\\gui.png", "..\\..\\..\\Graphics\\gui.png", "..\\..\\..\\Graphics\\hearts.png");
            Enemy.LoadContent("..\\..\\..\\Graphics\\enemies.png");
            Boss.LoadContent("..\\..\\..\\Graphics\\scary_face_2.png");
            Bullet.LoadContent("..\\..\\..\\Graphics\\Bullet.png", "..\\..\\..\\Graphics\\all.png");
            Shield.LoadContent("..\\..\\..\\Graphics\\shield.png");
            Player.LoadContent("..\\..\\..\\Graphics\\male_movement_right.png", "..\\..\\..\\Graphics\\all.png");
            Dialogue.LoadContent("..\\..\\..\\Graphics\\Dialogue.png", "..\\..\\..\\Fonts\\DefaultFont.ttf");
            CharacterSelect.LoadContent("..\\..\\..\\Graphics\\all.png", "..\\..\\..\\Fonts\\DefaultFont.ttf");
            Bonus.LoadContent("..\\..\\..\\Graphics\\bonuses.png");
        }
        public override void Initialize()
        {
            player = new Player();
            bullets = new LinkedList<Bullet>();
            HBGs = new LinkedList<HomingBulletGuy>();
            Lvl1BG = new Background("..\\..\\..\\Graphics\\Lvl1bg.png");
            string[] phrases = { "Развелось же их тут...", "Что ж, кроме этой тёмной дыры\n путей я больше не вижу" };
            string[] phrases1 = { "Тут явно кто-то есть." };
            string[] enteringDarkness = { "Нужно двигаться дальше..." };
            loh = new Dialogue(phrases);
            TalkingInTheDark = new Dialogue(phrases1);
            EnteringDarkness = new Dialogue(enteringDarkness);
            enemies = new LinkedList<Enemy>();
            bonuses = new LinkedList<Bonus>();
            firstBoss = new Boss(200, 0);
            OhWow = new Dialogue(new string[] { "Ох..." });
            gui = new GUI(player);
            Start = new Menu(new string[] { "Начать игру", "Выбор уровня", "Выход" });
            Continue = new Menu(new string[] { "Продолжить", "В главное меню", "Выход" });
            Lvl1Music = new Music("..\\..\\..\\Sounds\\lvl1.wav");
            DarknessMusic = new Music("..\\..\\..\\Sounds\\Darkness.wav");
            Boss1Music = new Music("..\\..\\..\\Sounds\\boss1.wav");
            DeathSound = new Music("..\\..\\..\\Sounds\\Death.wav");
            FirstCutscene = new Cutscene("..\\..\\..\\Graphics\\intro_iceboy.png", 400, 320);
            DeathCutscene = new Cutscene("..\\..\\..\\Graphics\\death.png", 800, 640);
            DeathCutscene.sprite.Scale = new Vector2f(1, 1);
            IceBoyFirstDialogue = new Dialogue(new string[] { "Кажется, это здесь.." });
            dialogues = new LinkedList<Dialogue>();
            dialogues.AddLast(loh);
            dialogues.AddLast(TalkingInTheDark);
            dialogues.AddLast(EnteringDarkness);
            dialogues.AddLast(OhWow);
            dialogues.AddLast(IceBoyFirstDialogue);
            music = new LinkedList<Music>();
            music.AddLast(Lvl1Music);
            music.AddLast(Boss1Music);
            music.AddLast(DarknessMusic);
            GirlIntro = new Dialogue(new string[] { "Наконец-то я заработаю целое\n состояние..." });
            GirlFinish = new Dialogue(new string[] { "Мне что, нужно лезть туда?!", "Ни за что!", "Ладно, ради светлого\n будущего..." });
            GirlEnteringDarkness = new Dialogue(new string[] { "Жутковато тут....." });
            GirlScared = new Dialogue(new string[] { "Мне кажется, или кто-то дышит?" });
            EndDemo = new Dialogue(new string[] { "Дорогой игрок, спасибо за то, \nчто помог мне добраться сюда" , "Возможно, в будушем, если Филипп\nне забьёт, мы ещё встретимся."});
            GirlFreakedOut = new Dialogue(new string[] { "О ГОСПОДИ" });
            dialogues.AddLast(GirlIntro);
            dialogues.AddLast(GirlFinish);
            dialogues.AddLast(GirlEnteringDarkness);
            dialogues.AddLast(GirlScared);
            dialogues.AddLast(GirlFreakedOut);
            dialogues.AddLast(EndDemo);
            characterSelect = new CharacterSelect();
            LevelSelect = new Menu(new string[] { "Уровень 1", "Уровень 2" });
            //music.AddLast(DeathSound);
        }
        public override void Update(GameTime gameTime)
        {
            ticks++;
            if(!GameStarted)
            {
                Start.Update();
                Start.Choose(this);
                if (ticks - startingTicks > 10)
                {
                    bool EnterPressed = Keyboard.IsKeyPressed(Keyboard.Key.Enter);
                    if (EnterPressed)
                    {
                        switch (Start.At())
                        {
                            case 1:
                                GameStarted = true;
                                GameTime.Unpause();
                                FirstCutscenePlaying = true;
                                break;
                            case 2:
                                GameStarted = true;
                                GameTime.Unpause();
                                LevelSelected = false;
                                break;
                            case 3:
                                Window.Close();
                                break;

                        }
                        startingTicks = ticks;
                    }
                }
            }
            if(!LevelSelected)
            {
                LevelSelect.Update();
                LevelSelect.Choose(this);
                bool EnterPressed = Keyboard.IsKeyPressed(Keyboard.Key.Enter);
                if(ticks-startingTicks>10)
                {
                    if (EnterPressed)
                    {
                        switch (LevelSelect.At())
                        {
                            case 1:
                                GameTime.Unpause();
                                FirstCutscenePlaying = true;
                                break;
                            case 2:
                                GameStarted = true;
                                GameTime.Unpause();
                                darkness = true;
                                break;
                        }
                        startingTicks = ticks;
                        LevelSelected = true;
                    }
                }
            }
            if(!characterSelected)
            {
                characterSelect.Update();
                characterSelect.Selection();
                if (ticks - startingTicks > 10)
                {
                    bool EnterPressed = Keyboard.IsKeyPressed(Keyboard.Key.Enter);
                    if (EnterPressed)
                    {
                        switch (characterSelect.At())
                        {
                            case 1:
                                player.SetType("Ice");
                                break;
                            case 2:
                                player.SetType("Girl");
                                break;
                        }
                        player.UpdateType();
                        characterSelected = true;
                    }
                    startingTicks = ticks;
                }
            }
            if (!isPaused && GameStarted && characterSelected&&!gameTime.Paused)
            {
                if (level_1)
                {
                    Lvl1BG.Animate();
                    Lvl1BG.Update();
                }
                // CleanUp();
                CutsceneOne();
                Level_1();
                Darkness();
                Player_Control(gameTime);
                //Clean();
                if(enemies.Count>0||HBGs.Count>0)
                    Enemies_Control();
                Clean();
                if(bonuses.Count>0)
                    Bonuses_Control();
                firstBoss.Update();
                gui.Update(player);
                if (firstBoss.hasSpawned && (OhWow.isFinished || GirlFreakedOut.isFinished))
                {
                    firstBoss.Shoot(player);
                    foreach (Bullet b in bullets)
                    {
                        firstBoss.Death(b, bonuses, player);
                    }
                }
            }
            else
            if (player.isDead&&isPaused)
            {
                foreach (Music m in music)
                {
                    if (m.Status == SoundStatus.Playing)
                    {
                        m.Stop();
                    }
                }
                if(DeathSound.Status !=SoundStatus.Playing)
                     DeathSound.Play();
                DeathCutscene.Play();
                DeathCutscene.Update();
                DeathCutscene.Animate(18);
                if (DeathCutscene.isStopped)
                {
                    DeathCutscene.Stop();
                    showContinue = true;
                }
                Continue.Choose(this);
                Continue.Update();
                if (ticks - startingTicks > 10)
                {
                    bool EnterPressed = Keyboard.IsKeyPressed(Keyboard.Key.Enter);
                    if (EnterPressed)
                    {
                        switch (Continue.At())
                        {
                            case 1:
                                UnPause();
                                Retry(0);
                                break;
                            case 2:
                                UnPause();
                                Retry(1);
                                break;
                            case 3:
                                Window.Close();
                                break;
                        }
                        startingTicks = ticks;
                    }
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            if(level_1)
                Draw(Lvl1BG);
            if (!player.isDead&&characterSelected)
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
                if(!e.isDead)
                    Draw(e);
                foreach(Bullet b in e.bullets)
                {
                    if(!b.isDisposed)
                        Draw(b);
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
            if(!GameStarted)
                Start.Draw(this);
            if (isPaused && showContinue)
                Continue.Draw(this);
            if(FirstCutscene.isStarted&&!FirstCutscene.isFinished)
            {
                FirstCutscene.Draw(this);
            }
            if (DeathCutscene.isStarted && !DeathCutscene.isFinished)
            {
                DeathCutscene.Draw(this);
            }

            if(GameStarted&&!characterSelected)
            {
                characterSelect.Draw(this);
            }
            foreach (Dialogue d in dialogues)
            {
                if (d.isStarted && !d.isFinished)
                {
                    d.Draw(this);
                }
            }
            if(!LevelSelected)
            {
                LevelSelect.Draw(this);
            }
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
                {
                    alive++;
                }

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
                    enemies.AddLast(new Enemy(ax += 100, 100));
                }
            for (int i = 0; i < b; i++)
            {
                if(bx<580)
                     HBGs.AddLast(new HomingBulletGuy(bx += 100, 250));
            }
        }
        private void Level_1()
        {
            if (level_1)
            {
                if (Lvl1Music.Status != SoundStatus.Playing)
                    Lvl1Music.Play();
                if (waves == 10 && StageClear())
                {
                    player.DisableMovement();
                    player.DisableShooting();
                    player.isInvincible = true;
                    if (player.type == "Ice")
                    {
                        loh.Start();
                        loh.Change();
                    }
                    else
                    {
                        GirlFinish.Start();
                        GirlFinish.Change();
                    }
                    if(loh.isFinished||GirlFinish.isFinished)
                    {
                        //player.EnableMovement();
                        level_1 = false;
                        darkness = true;
                        bullets.Clear();
                        enemies.Clear();
                        HBGs.Clear();
                        Lvl1Music.Stop();
                    }
                    
                }
                else if (StageClear())
                {
                    if (waves % 3 == 0)
                        SpawnEnemies(waves, waves / 3);
                    else
                        SpawnEnemies(waves, 0);
                    waves++;
                }
                
            }
        }
        private void CutsceneOne()
        {
            if (FirstCutscenePlaying)
            {
                FirstCutscene.Play();
                FirstCutscene.Update();
                FirstCutscene.Animate(20);
                if (FirstCutscene.isStopped)
                {
                    if (player.type == "Ice")
                    {
                        IceBoyFirstDialogue.Start();
                        IceBoyFirstDialogue.Change();
                    }
                    else
                    {
                        GirlIntro.Start();
                        GirlIntro.Change();
                    }
                    if (IceBoyFirstDialogue.isFinished||GirlIntro.isFinished)
                    {
                        FirstCutscene.Stop();
                        FirstCutscenePlaying = false;
                        level_1 = true;
                    }
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
                if (player.type == "Ice")
                {
                    if (canSpeak)
                        EnteringDarkness.Start();
                    EnteringDarkness.Change();
                }
                else
                {
                    if (canSpeak)
                        GirlEnteringDarkness.Start();
                    GirlEnteringDarkness.Change();
                }
                if (EnteringDarkness.isFinished||GirlEnteringDarkness.isFinished)
                {
                    canSpeak = false;
                    if(!TalkingInTheDark.isStarted&&!GirlScared.isStarted)
                    {
                        if (DarknessMusic.Status != SoundStatus.Playing)
                            DarknessMusic.Play();
                        player.DisableMovement();
                        player.MoveThere();
                    }
                }
                if (floors < 2)
                {
                    if (player.coordinates.Y <= 0)
                    {
                        player.coordinates.Y = 640 - player.sprite.GetGlobalBounds().Height;
                        floors++;
                    }
                   
                }
                else
                {
                    if (player.type == "Ice")
                    {
                        TalkingInTheDark.Start();
                        player.DisableMovement();
                        TalkingInTheDark.Change();
                    }
                    else
                    {
                        GirlScared.Start();
                        player.DisableMovement();
                        GirlScared.Change();
                    }
                    if(TalkingInTheDark.isFinished||GirlScared.isFinished)
                    {
                        
                        firstBoss.Spawn();
                        if (player.type == "Ice")
                        {
                            OhWow.Start();
                            OhWow.Change();
                        }
                        else
                        {
                            GirlFreakedOut.Start();
                            GirlFreakedOut.Change();
                        }
                        if (OhWow.isFinished||GirlFreakedOut.isFinished)
                        {
                            player.EnableMovement();
                            player.EnableShooting();
                            if(Boss1Music.Status!=SoundStatus.Playing)
                                Boss1Music.Play();
                            if(firstBoss.isDead)
                            {
                                EndDemo.Start();
                                EndDemo.Change();
                                if(EndDemo.isFinished)
                                {
                                    UnPause();
                                    DemoCompleted = true;
                                    Retry(1);
                                }
                            }
                        }
                    }
                }

            }

        }
        private void Player_Control(GameTime gameTime)
        {
            player.Move();
            player.UpgradeBullets();
            player.TwinStrike();
            player.shield.MoveWithPlayer(player);
            player.shield.Update();
            player.Melt();
            if(!player.isDead&&characterSelected)
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
                    e.Death(b,bonuses,player);
                }
                foreach (HomingBulletGuy e in HBGs)
                {
                    e.Death(b,bonuses,player);
                }
            }
        }
        private void Enemies_Control()
        {
            foreach (Enemy e in enemies)
            {

                e.Update();
                e.Shoot(player);
                e.Freezing();
            }
            foreach (HomingBulletGuy hbg in HBGs)
            {

                hbg.Update();
                hbg.Shoot(player);
                hbg.Freezing();
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
        private void Clean()
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
                        Console.WriteLine(bullets.Count);
                    }
                }
            }
            Enemy[] es = new Enemy[enemies.Count];
            if (es.Length > 0)
            {
                enemies.CopyTo(es, 0);
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (es[i].isDead && es[i].bullets.Count==0)
                    {
                        enemies.Remove(es[i]);
                        Console.WriteLine(enemies.Count);
                    }
                }
            }
            HomingBulletGuy[] hbgs = new HomingBulletGuy[HBGs.Count];
            if (hbgs.Length > 0)
            {
                HBGs.CopyTo(hbgs, 0);
                for (int i = 0; i < HBGs.Count; i++)
                {
                    if (hbgs[i].isDead&&hbgs[i].bullets.Count==0)
                    {
                        HBGs.Remove(hbgs[i]);
                        Console.WriteLine(HBGs.Count);
                    }
                }
            }
        }
        private void Retry(int degree)
        {
            player.Respawn();
            bullets.Clear();
            enemies.Clear();
            HBGs.Clear();
            bonuses.Clear();
            //GC.Collect();
            if (degree == 1)
            {
                GameStarted = false;
                level_1 = false;
                darkness = false;
                showContinue = false;
                characterSelected = false;
            }
            waves = 0;
            floors = 0;
            FirstCutscene.Replay();
            DeathCutscene.Replay();
            firstBoss.Respawn();

            foreach(Dialogue d in dialogues)
            {
                d.isFinished = false;
                d.isStarted = false;
            }    
            foreach(Music m in music)
            {
                if(m.Status==SoundStatus.Playing)
                {
                    m.Stop();
                }
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace game
{
    public abstract class GameLoop
    {
        public const int TARGET_FPS = 60;
        public const float TIME_UNTIL_UPDATE = 1f / TARGET_FPS;
        public int TotalTicksBeforeShooting = 0;
        public int TotalTicks=0;
        public RenderWindow Window
        {
            get;
            protected set;
        }
        public GameTime GameTime
        {
            get;
            protected set;
        }
        public Color WindowClearColor
        {
            get;
            protected set;
        }

        protected GameLoop(uint windowWidth, uint windowHeight, string windowTitle, Color windowClearColor)
        {
            this.WindowClearColor = WindowClearColor;
            this.Window = new RenderWindow(new VideoMode(windowWidth,windowHeight),windowTitle);
            this.GameTime = new GameTime();
            Window.Closed += WindowClosed;
            Window.KeyPressed += KeyboardInput;
            Window.LostFocus += lostFocus;
            Window.GainedFocus += gainedFocus;
        }
        public void Run()
        {
            LoadContent();
            Initialize();
            float totalTimeBeforeUpdate = 0f;
            float previousTimeElapsed = 0f;
            float deltaTime = 0f;
            float totalTimeElapsed = 0f;

            Clock clock = new Clock();
            
            while(Window.IsOpen)
            {
                Window.DispatchEvents();
                totalTimeElapsed = clock.ElapsedTime.AsSeconds();
                deltaTime = totalTimeElapsed - previousTimeElapsed;
                previousTimeElapsed = totalTimeElapsed;
                totalTimeBeforeUpdate += deltaTime;
                if(totalTimeBeforeUpdate>=TIME_UNTIL_UPDATE)
                {
                    GameTime.Update(totalTimeBeforeUpdate, clock.ElapsedTime.AsSeconds());
                    totalTimeBeforeUpdate = 0f;
                    TotalTicksBeforeShooting++;
                    Update(GameTime);
                    Window.Clear(WindowClearColor);
                    Draw(GameTime);
                    Window.Display();
                    TotalTicks++;
                }
            }
        }
        public abstract void LoadContent();
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        private void WindowClosed(object sender, EventArgs e)
        {
            Window.Close();
        }
        private void KeyboardInput(object sender, KeyEventArgs e)
        {
        }
        private void lostFocus(object sender, EventArgs e)
        {
            GameTime.Pause();
        }
        private void gainedFocus(object sender, EventArgs e)
        {
            GameTime.Unpause();
        }
    }
}

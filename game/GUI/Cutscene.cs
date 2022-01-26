using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace game
{
    class Cutscene
    {
        private int ticks = 0;
        public Sprite sprite;
        private Texture tex;
        private int Frame;
        private int x = 0;
        private int y = 0;
        public bool isStopped = false;
        public bool isStarted = false;
        public bool isFinished = false;
        private int w;
        private int h;
        public Cutscene(string Path,int width, int height)
        {
            w = width;
            h = height;
            tex = new Texture(Path);
            sprite = new Sprite(tex);
            sprite.Position = new Vector2f(0, 0);
            sprite.TextureRect = new IntRect(new Vector2i(0, 0), new Vector2i(w,h));
            sprite.Scale = new Vector2f(2f, 2f);
            Frame = 1;
        }

        public void Animate(int frames)
        {
            if (!isFinished && ticks % 6 == 0 && !isStopped)
            {
                if (Frame == frames)
                {
                    y = 0;
                    x = 0;
                    Frame = 1;
                    isStopped = true;
                    Console.WriteLine("STOP");
                }
                else if (x == 1600)
                {
                    y += h;
                    x = 0;
                    Frame++;
                    Console.WriteLine(Frame);
                }

                else
                {
                    Frame++;
                    Console.WriteLine(Frame);
                    sprite.TextureRect = new IntRect(new Vector2i(x, y), new Vector2i(w, h));
                    x += w;
                }
            }

        }
        public void Stop()
        {
            isFinished = true;
        }
        public void Play()
        {
            isStarted = true;
        }
        public void Replay()
        {
            isFinished = false;
            isStopped = false;
            isStarted = false;
        }
        public void Draw(GameLoop gameLoop)
        {
            gameLoop.Window.Draw(sprite);
        }
        public int At()
        {
            return Frame;
        }
        public void Update()
        {
            ticks++;
        }
    }
}

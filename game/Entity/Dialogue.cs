using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace game
{
    class Dialogue
    {
        private Font font;
        private RectangleShape rect;
        private Text text;
        private int ticks = 0;
        private string[] Phrases;
        private int CurrentPhrase = 0;
        private bool isStarted = false;
        public bool isFinished = false;
        public Dialogue(string[] phrases)
        {
            Phrases = phrases;
            font = new Font("Z:\\progs\\game\\game\\Fonts\\DefaultFont.ttf");
            text = new Text(phrases[CurrentPhrase], font);
            rect = new RectangleShape(new Vector2f(560, 200));
            rect.FillColor = Color.Cyan;
            text.FillColor = Color.Black;
            rect.Position = new Vector2f(120, 440);
            text.Position = new Vector2f(140, 460);
        }
        public void Draw(GameLoop gameLoop)
        {
            if(isStarted&&!isFinished)
            {
                gameLoop.Window.Draw(rect);
                gameLoop.Window.Draw(text);
                ticks++;
            }
        }
        public void Change()
        {
            bool Changing = Keyboard.IsKeyPressed(Keyboard.Key.Enter);
            if (Changing && ticks > 20)
            {
                CurrentPhrase++;
                if (CurrentPhrase < Phrases.Length)
                {
                    text = new Text(Phrases[CurrentPhrase], font);
                    text.Position = new Vector2f(140, 460);
                    text.FillColor = Color.Black;
                    Console.WriteLine("Changed");
                    ticks = 0;
                }
                else
                {
                    isFinished = true;
                }
            }
        }
        public void Start()
        {
            isStarted = true;
        }
    }
}

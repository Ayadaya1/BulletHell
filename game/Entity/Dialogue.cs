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
        private static Font font;
        private static Texture texture;
        private static Sprite sprite;
        private Text text;
        private int ticks = 0;
        private string[] Phrases;
        private int CurrentPhrase = 0;
        public bool isStarted = false;
        public bool isFinished = false;
        public Dialogue(string[] phrases)
        {
            Phrases = phrases;
            text = new Text(phrases[CurrentPhrase], font);
            text.FillColor = Color.Black;
            text.Position = new Vector2f(160, 485);
            sprite.Position = new Vector2f(120, 440);
            sprite.TextureRect = new IntRect(new Vector2i(1, 0),new Vector2i(586,161));
            sprite.Scale = new Vector2f(1f, 1.4f);
            Color color = sprite.Color;
            color.A = 220;
            sprite.Color = color;
        }
        public void Draw(GameLoop gameLoop)
        {
            if(isStarted&&!isFinished)
            {
                gameLoop.Window.Draw(sprite);
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
                    text.Position = new Vector2f(160, 485);
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
        public static void LoadContent(string path1, string path2)
        {
            texture = new Texture(path1);
            sprite = new Sprite(texture);
            font = new Font(path2);
        }
    }
}

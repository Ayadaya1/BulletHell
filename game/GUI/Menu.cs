using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace game
{
    class Menu
    {
        private Text[] Titles;
        private int Number;
        private Font font;
        private RectangleShape backGround;
        private int Choice = 1;

        public Menu(string[] titles)
        {
            font = new Font("Z:\\progs\\game\\game\\Fonts\\DefaultFont.ttf");
            Number = titles.Length;
            Titles = new Text[Number];
            for(int i = 0; i<titles.Length;i++)
            {
                Titles[i] = new Text(titles[i],font);
            }
            foreach(Text t in Titles)
            {
                t.FillColor = Color.White;
            }
            backGround = new RectangleShape(new Vector2f(800, 640));
            backGround.Position = new Vector2f(0, 0);
            backGround.FillColor = new Color(46, 63, 92);
        }
        public void Choose(GameLoop gameLoop)
        {
            bool Up = Keyboard.IsKeyPressed(Keyboard.Key.Up);
            bool Down = Keyboard.IsKeyPressed(Keyboard.Key.Down);
            if(Up)
            {
                if(Choice==1)
                {
                    Choice = Number;
                }
                else
                {
                    Choice--;
                }
            }
            if(Down)
            {
                if(Choice == Number)
                {
                    Choice = 1;
                }
                else
                {
                    Choice++;
                }
            }
        }
        public int At()
        {
            return Choice;
        }
        public void Update()
        {
            for(int i = 1; i<=Number; i++)
            {
                if (i == Choice)
                {
                    Titles[i-1].FillColor = Color.Red;
                }
                else
                {
                    Titles[i-1].FillColor = Color.White;
                }
            }
        }
        public void Draw(GameLoop gameLoop)
        {
            gameLoop.Window.Draw(backGround);
            int y = 100;
            for(int i = 0; i<Number;i++)
            {
                Titles[i].Position = new Vector2f(300, y);
                gameLoop.Window.Draw(Titles[i]);
                y += 100;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace game
{

    class GUI
    {
        private Sprite Hearts;
        private RectangleShape LeftRect;
        private RectangleShape RightRect;
        private Sprite Miniature;
        private int MoveSpeed;
        private int HP;
        private int AttackSpeed;
        public GUI(Player player)
        {
            Texture tex = new Texture("Z:\\progs\\game\\game\\Graphics\\Heart.png");
            Hearts = new Sprite(tex);
            Hearts.Scale = new Vector2f(0.25f, 0.25f);
            LeftRect = new RectangleShape(new Vector2f(120,640));
            RightRect = new RectangleShape(new Vector2f(120, 640));
            LeftRect.Position = new Vector2f(0, 0);
            RightRect.Position = new Vector2f(680, 0);
            LeftRect.FillColor = new Color(46,63,92);
            RightRect.FillColor = new Color(46, 63, 92);
            HP = player.Health;
            MoveSpeed = player.movespeed;
            AttackSpeed = player.AttackSpeed;
        }
        public void Draw(GameLoop gameLoop)
        {
            int y = 50;
            gameLoop.Window.Draw(LeftRect);
            gameLoop.Window.Draw(RightRect);
            for (int i = 0; i<HP; i++)
            {
                Hearts.Position = new Vector2f(690, y);
                gameLoop.Window.Draw(Hearts);
                y += Convert.ToInt32(Hearts.GetGlobalBounds().Height);
            }
        }
        public void Update(Player player)
        {
            HP = player.Health;
            MoveSpeed = player.movespeed;
            AttackSpeed = player.AttackSpeed;
        }
    }
}

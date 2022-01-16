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
        private Sprite Lines;
        private static Texture IceGui;
        private static Texture GirlGui;
        private static Texture HeartTexture;
        private int MoveSpeed;
        private int HP;
        private int AttackSpeed;

        public GUI(Player player)
        {
            Texture tex = new Texture("Z:\\progs\\game\\game\\Graphics\\Heart.png");
            if(player.type == "Ice")
            {
                Lines = new Sprite(IceGui);
            }
            else
            {
                Lines = new Sprite(GirlGui);
            }
            Lines.Scale = new Vector2f(2f, 2f);
            Hearts = new Sprite(tex);
            Hearts.Scale = new Vector2f(0.25f, 0.25f);
            HP = player.Health;
            MoveSpeed = player.movespeed;
            AttackSpeed = player.AttackSpeed;
        }
        public void Draw(GameLoop gameLoop)
        {
            int y = 50;
            gameLoop.Window.Draw(Lines);
            for (int i = 0; i<HP; i++)
            {
                Hearts.Position = new Vector2f(700, y);
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
        public static void LoadContent(string path1, string path2)//, string path3)
        {
            IceGui = new Texture(path1);
            GirlGui = new Texture(path2);
           // HeartTexture = new Texture(path3);
        }
    }
}

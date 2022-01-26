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
        private Sprite IceHeart;
        private Sprite Lines;
        private static Texture IceGui;
        private static Texture GirlGui;
        private static Texture HeartTexture;
        private int MoveSpeed;
        private int HP;
        private int AttackSpeed;
        private string type;
        private bool HeartOfIce;

        public GUI(Player player)
        {
            type = player.type;
            if(player.type == "Ice")
            {
                Lines = new Sprite(IceGui);
            }
            else
            {
                Lines = new Sprite(GirlGui);
            }
            Lines.Scale = new Vector2f(2f, 2f);
            Hearts = new Sprite(HeartTexture);
            IceHeart = new Sprite(HeartTexture);
            Hearts.Scale = new Vector2f(0.75f, 0.75f);
            Hearts.TextureRect = new IntRect(new Vector2i(9, 114), new Vector2i(102, 75));
            IceHeart.TextureRect = new IntRect(new Vector2i(164, 85), new Vector2i(129, 104));
            IceHeart.Scale = new Vector2f(0.7f, 0.7f);
            HP = player.Health;
            MoveSpeed = player.movespeed;
            AttackSpeed = player.AttackSpeed;
        }
        public void Draw(GameLoop gameLoop)
        {
            int y = 50;
            gameLoop.Window.Draw(Lines);
            if (type == "Ice")
            {
                for (int i = 0; i < HP - 1; i++)
                {
                    IceHeart.Position = new Vector2f(710, y);
                    gameLoop.Window.Draw(IceHeart);
                    y += Convert.ToInt32(IceHeart.GetGlobalBounds().Height);
                }
                if (!HeartOfIce)
                {
                    Hearts.Position = new Vector2f(710, y);
                    gameLoop.Window.Draw(Hearts);
                }
                else
                {
                    IceHeart.Position = new Vector2f(710, y);
                    gameLoop.Window.Draw(IceHeart);
                }
            }
            else
            {
                for (int i = 0; i < HP; i++)
                {
                    Hearts.Position = new Vector2f(710, y);
                    gameLoop.Window.Draw(Hearts);
                    y += Convert.ToInt32(Hearts.GetGlobalBounds().Height);
                }
            }
        }
        public void Update(Player player)
        {
            HP = player.Health;
            HeartOfIce = player.HeartOfIce;
            MoveSpeed = player.movespeed;
            AttackSpeed = player.AttackSpeed;
            type = player.type;
        }
        public static void LoadContent(string path1, string path2, string path3)
        {
            IceGui = new Texture(path1);
            GirlGui = new Texture(path2);
            HeartTexture = new Texture(path3);
        }
    }
}

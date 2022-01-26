using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace game
{
    class CharacterSelect
    {
        private Sprite IceBoy;
        private Sprite Girl;
        private static Texture texture;
        private static Font font;
        private RectangleShape rect;
        private int TheChosenOne;
        private int ticks=0;
        private int startingTicks=0;
        private Text choose;
        private Text Glacius;
        private Text GlaciusStory;
        private Text GirlsName;
        private Text GirlStory;
        public CharacterSelect()
        {
            string Glacius_story = "Глаций отправился в это путешествие, получив\n таинственное письмо от того, кто назвался его\n дальним родственником.\nТеперь ему предстоит выяснить тёмные тайны своей\n родословной. А может быть, это письмо было\n всего лишь ловушкой?";
            string Girl_story = "Призраки - довольно распространённые вредители.\nДля того, чтобы с ними бороться, люди нанимают\n профессиональных охотников на призраков.\n????? - одна из таких охотников\nВ этот раз она получила очень дорогой, но весьма\n странный заказ...";
            GirlStory = new Text(Girl_story, font);
            TheChosenOne = 1;
            rect = new RectangleShape(new Vector2f(800, 640));
            rect.Position = new Vector2f(0, 0);
            choose = new Text("ВЫБЕРИТЕ ПЕРСОНАЖА", font);
            Glacius = new Text("Глаций", font);
            GirlsName = new Text("?????", font);
            Glacius.FillColor = Color.Red;
            Glacius.Position = new Vector2f(330, 50);
            GirlsName.Position = Glacius.Position;
            GirlsName.FillColor = Color.Blue;
            GlaciusStory = new Text(Glacius_story, font);
            GlaciusStory.Position = new Vector2f(20, 100);
            choose.Position = new Vector2f(250, 10);
            IceBoy = new Sprite(texture);
            IceBoy.TextureRect = new IntRect(new Vector2i(0, 76), new Vector2i(140, 180));
            IceBoy.Position = new Vector2f(250, 280);
            IceBoy.Scale = new Vector2f(2f, 2f);
            Girl = new Sprite(texture);
            Girl.TextureRect = new IntRect(new Vector2i(145, 76), new Vector2i(140, 180));
            Girl.Position = new Vector2f(285, 280);
            Girl.Scale = new Vector2f(2f, 2f);
            GirlStory.Position = GlaciusStory.Position;
        }
        public void Update()
        {
            ticks++;
            if (TheChosenOne == 1)
            {
                rect.FillColor = new Color(27, 94, 97);
            }
            else
            {
                rect.FillColor = new Color(203, 122, 0);
            }
        }
        public void Selection()
        {
            bool somethingPressed = Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.Left);
            if(ticks-startingTicks>=20)
            {
                if(somethingPressed)
                {
                    if (TheChosenOne == 1)
                        TheChosenOne = 2;
                    else
                        TheChosenOne = 1;
                    Console.WriteLine(TheChosenOne);
                    startingTicks = ticks;
                }
            }
        }
        public static void LoadContent(string path1, string path2)
        {
            texture = new Texture(path1);
            font = new Font(path2);
        }
        public void Draw(GameLoop gameLoop)
        {
            gameLoop.Window.Draw(rect);
            gameLoop.Window.Draw(choose);
            if(TheChosenOne == 1)
            {
                gameLoop.Window.Draw(IceBoy);
                gameLoop.Window.Draw(Glacius);
                gameLoop.Window.Draw(GlaciusStory);
            }
            else
            {
                gameLoop.Window.Draw(Girl);
                gameLoop.Window.Draw(GirlsName);
                gameLoop.Window.Draw(GirlStory);
            }
        }
        public int At()
        {
            return TheChosenOne;
        }
    }
}

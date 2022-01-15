using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace game
{

    class Background : Entity
    {
        public int RectX = 0;
        public int RectY = 0;
        public Background(string Path) : base(Path)
        {
            coordinates = new Vector2f(0f, 0f);
            sprite.Scale = new Vector2f(2f, 2f);
        }
        public void Animate()
        {
            if (ticks % 5 == 0)
            {
                if (RectX == 800 && RectY == 1600)
                {
                    RectX = 0;
                    RectY = 0;
                }
                if (RectX < 2000)
                    RectX += 400;
                else
                {
                    RectX = 0;
                    RectY += 320;
                }
                sprite.TextureRect = new IntRect(new Vector2i(RectX, RectY), new Vector2i(400, 320));
            }
        }
    }
}
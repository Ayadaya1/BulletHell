using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
namespace game
{
    public class Shield : Entity
    {
        public bool isOn = false;
        private static Texture texture;
        public Shield(Player player) : base()
        {
            sprite = new Sprite(texture);
            coordinates = player.coordinates;
        }
        public void Enable()
        {
            isOn = true;
        }
        public void Disable()
        {
            isOn = false;
        }
        public void MoveWithPlayer(Player player)
        {
            coordinates = player.coordinates;
        }
        new public static void LoadContent(string path)
        {
            texture = new Texture(path);
        }

    }
}
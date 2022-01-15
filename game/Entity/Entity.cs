using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;

namespace game
{
    public abstract class Entity
    {
        public Vector2f coordinates;
        public Texture tex;
        public Sprite sprite;
        public int ticks;
        public void LoadContent(string texPath) 
        {
            tex = new Texture(texPath);
        }
        public void Update()
        {
            sprite.Position = coordinates;
            ticks++;
        }
        public Entity(string texPath)
        {
            tex = new Texture(texPath);
            sprite = new Sprite(tex);
            coordinates = new Vector2f(100, 200);
            // sprite.TextureRect = new IntRect(new Vector2i(100,100), new Vector2i(100, 100));
            //sprite.Scale = new Vector2f(0.5f, 0.5f);
            sprite.Position = coordinates;
            ticks = 0;
        }
        public bool Collides(Entity e)
        {
            if (coordinates.X <= e.coordinates.X + e.sprite.GetGlobalBounds().Width && e.coordinates.X <= coordinates.X + sprite.GetGlobalBounds().Width && coordinates.Y <= e.coordinates.Y + e.sprite.GetGlobalBounds().Height && e.coordinates.Y <= coordinates.Y + sprite.GetGlobalBounds().Height)
                return true;
            return false;
        }
    }
}

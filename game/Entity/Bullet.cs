using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace game
{
    public class Bullet : Entity, IDisposable
    {
        public bool isDisposed = false;
        private int RectLoc = 6;
        public float XSpeed;
        public float YSpeed;
        private static Texture EnemyBullets;
        private static Texture PlayerIceBullets;
        private static Texture GirlBullets;
        public Bullet(Entity entity, float xspeed, float yspeed) : base()
        {
            coordinates = new Vector2f(entity.coordinates.X + (entity.sprite.GetGlobalBounds().Width / 2) - 8, entity.coordinates.Y);
            if (entity.GetHashCode() == 1)
            {
                Player player = entity as Player;
                if (player.type == "Ice")
                {
                    sprite = new Sprite(PlayerIceBullets);
                    sprite.TextureRect = new IntRect(new Vector2i(31, 26), new Vector2i(9, 11));
                    sprite.Scale = new Vector2f(2.5f, 2.5f);
                }
                else
                {
                    sprite = new Sprite(PlayerIceBullets);
                    sprite.TextureRect = new IntRect(new Vector2i(155, 24), new Vector2i(9, 11));
                    sprite.Scale = new Vector2f(2.5f, 2.5f);
                    coordinates.X -= 10;
                }
            }
            else
            {
                sprite = new Sprite(EnemyBullets);
                sprite.TextureRect = new IntRect(new Vector2i(6, 6), new Vector2i(4, 4));
                sprite.Scale = new Vector2f(5f, 5f);
            }
            XSpeed = xspeed;
            YSpeed = yspeed;
        }
        public Bullet(float x, float y, float xspeed, float yspeed) : base()
        {
            sprite = new Sprite(EnemyBullets);
            coordinates = new Vector2f(x, y);
            sprite.TextureRect = new IntRect(new Vector2i(6, 6), new Vector2i(4, 4));
            sprite.Scale = new Vector2f(5f, 5f);
            XSpeed = xspeed;
            YSpeed = yspeed;
        }
        public void fly()
        {
            if (!isDisposed)
            {
                if (coordinates.Y + sprite.GetGlobalBounds().Height > 0 && coordinates.Y < 640 && coordinates.X + sprite.GetGlobalBounds().Width > 120 && coordinates.X < 680)
                {
                    coordinates.X += XSpeed;
                    coordinates.Y += YSpeed;
                }
                else
                {
                    this.Dispose();
                    sprite.Dispose();
                    //tex.Dispose();
                    isDisposed = true;
                    //Console.WriteLine("Bullet Disposed");
                }
            }
        }
        public void Dispose()
        {
            isDisposed = true;
        }
        public void Animate()
        {
            if (!isDisposed)
            {
                if (ticks % 5 == 0)
                {
                    sprite.TextureRect = new IntRect(new Vector2i(RectLoc, 6), new Vector2i(4, 4));
                    if (RectLoc < 54)
                        RectLoc += 16;
                    else
                        RectLoc = 6;
                }
            }
        }
       
        public static void LoadContent(string path1, string path2)
        {
            EnemyBullets = new Texture(path1);
            PlayerIceBullets = new Texture(path2);
        }
    }
}
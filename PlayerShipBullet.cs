using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Shmup
{
    class PlayerShipBullet : Bullet
    {

        bool hasCollided = false; //pour savoir si le projectile a touché un vaisseau et le faire disparaitre ensuite

        int damage = 10; //les degâts des projectiles
        Vector2 origin;
        

        public bool HasCollided { get => hasCollided; set => hasCollided = value; }
        public int Damage { get => damage; set => damage = value; }

        /// <summary>
        /// Constructeur par defaut
        /// </summary>

        public PlayerShipBullet()
        {
            width = 9;
            height = 19;
            spriteIndex = Sprites.Red;
            rectangleSource = new Rectangle(0, 0, Width, Height);
            isVisible = true;
            origin = new Vector2(width / 2, height / 2);
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="shipBullet"></param>
        /// <param name="angle"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>

        public PlayerShipBullet(Bullet shipBullet, float angle, Vector2 position, int color)
        {
            width = shipBullet.Width;
            height = shipBullet.Height;
            texture = shipBullet.Texture;
            switch (color)
            {
                case 0:
                    rectangleSource = shipBullet.RectangleSource;
                    SpriteIndex = Sprites.Red;
                    break;
                case 1:
                    rectangleSource = new Rectangle(12, 0, width, height);
                    SpriteIndex = Sprites.Green;
                    break;
                case 2:
                    rectangleSource = new Rectangle(24, 0, width, height);
                    SpriteIndex = Sprites.Yellow;
                    break;
                case 3:
                    rectangleSource = new Rectangle(36, 0, width, height);
                    SpriteIndex = Sprites.Blue;
                    break;
                default:
                    break;
            }
            
            isVisible = shipBullet.IsVisible;
            origin = new Vector2(shipBullet.Width / 2, shipBullet.Height / 2);
            this.angle = angle;
            this.position = position;
            velocity = new Vector2((float)Math.Cos(angle - Math.PI / 2), (float)Math.Sin(angle - Math.PI / 2)) * 10f;

          
            SoundEffectInstance soundEffectInstance = shipBullet.SoundEffect.CreateInstance();
            soundEffectInstance.Volume = 0.1f;
            soundEffectInstance.Play();
        }

        /// <summary>
        /// Fonction Draw principale
        /// </summary>
        /// <param name="spriteBatch"></param>

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangleSource, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
        }


    }
}

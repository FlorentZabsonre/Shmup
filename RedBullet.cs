using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    class RedBullet : Bullet
    {    

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Texture"></param>

        public RedBullet(int Width, int Height) : base(Width, Height)
        {
            spriteIndex = Sprites.Red;            
        }

        /// <summary>
        /// Constructeur avec le projectile
        /// </summary>
        /// <param name="bullet"></param>

        public RedBullet(Bullet bullet) : base(bullet)
        {
            spriteIndex = Sprites.Red;
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>

        public RedBullet()
        {
        }

        /// <summary>
        /// Pour actualiser les projectiles spéciaux
        /// </summary>

        public void UpdateSpecialBullets()
        {
            if (isVisible)
            {
                position += velocity;
                InitialisationHitbox();

                if (position.Y > Level.Height - height || position.Y < 0 || position.X < 0 || position.X > Level.Width - width)
                    isVisible = false;
            }
        }


        /// <summary>
        /// Fonction Draw principale
        /// </summary>
        /// <param name="spriteBatch"></param>

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangleSource, Color.White, angle, Vector2.Zero, 1f, SpriteEffects.None, 0);
        } 
    }
}

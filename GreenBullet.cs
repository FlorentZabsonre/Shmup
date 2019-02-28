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
    class GreenBullet : Bullet
    {
        int nbReflect = 0; //Pour connaître le nombre de rebonds fait par les projectiles classiques du vaisseau vert            

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bullet"></param>

        public GreenBullet(Bullet bullet) : base(bullet)
        {
            spriteIndex = Sprites.Green;
        }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>

        public GreenBullet(int Width, int Height) : base(Width, Height)
        {
            spriteIndex = Sprites.Green;
        }

  
        /// <summary>
        /// Update les projectiles avec rebonds
        /// </summary>

        public new void UpdateBullets()
        {
            //tant que les projectiles n'ont pas fait deux rebonds, on gère leur direction
            if (nbReflect < 2)
            {
                if (position.X < 0 || position.X >= Level.Width - width)
                {
                    velocity.X = -velocity.X;
                    nbReflect++;
                }
                else
                {
                    if (position.Y < 0 || position.Y >= Level.Height - height)
                    {
                        velocity = new Vector2(velocity.X, -velocity.Y);
                        nbReflect++;
                    }
                }
                position += velocity;
                InitialisationHitbox();
            }
            else
            {
                base.UpdateBullets();
            }
            
        }

        /// <summary>
        /// Actualise la position des projectiles spéciaux
        /// </summary>

        public void UpdateSpecialBullets()
        {
            position += velocity;
            InitialisationHitbox();

            if (position.Y > Level.Height - height || position.Y < 0 || position.X < 0 || position.X > Level.Width - width)
                isVisible = false;
        }


        /// <summary>
        /// Fonction Draw principale
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitbox, rectangleSource, Color.White);
        }
    }
}

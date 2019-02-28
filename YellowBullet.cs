using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    class YellowBullet : Bullet
    {
        bool targetEnemy;
        Vector2 maxVelocity = new Vector2(4, 4);
        Vector2 distance = new Vector2(4001,4001);

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>

        public YellowBullet(int Width, int Height) : base(Width, Height)
        {
            spriteIndex = Sprites.Yellow;
        }

        /// <summary>
        /// Constructeur avec un projectile
        /// </summary>
        /// <param name="bullet"></param>

        public YellowBullet(Bullet bullet) : base(bullet)
        {
            spriteIndex = Sprites.Yellow;
        }

        /// <summary>
        /// Constructeur par defaut
        /// </summary>

        public YellowBullet()
        {
        } 
        
        /// <summary>
        /// Rotation des projectiles speciaux
        /// </summary>

        public void Rotation()
        {
            angle += 0.08f;
        }

        /// <summary>
        /// Update des projectiles spéciaux
        /// </summary>
        /// <param name="playerShipPosition"></param>
        /// <param name="enemy"></param>

        public void UpdateSpecialBullets(Vector2 playerShipPosition, YellowEnemy enemy)
        {
            Vector2 previousDistance = distance;
            if (enemy.IsAlive)
            {
                if (!targetEnemy)
                {
                    distance = new Vector2(playerShipPosition.X - position.X, playerShipPosition.Y - position.Y);
                    float angle = (float)Math.Atan2(distance.Y, distance.X);
                    velocity += new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 0.5f;
                    if (velocity.X < -maxVelocity.X)
                    {
                        velocity.X = -maxVelocity.X;
                    }
                    else if (velocity.X > maxVelocity.X)
                    {
                        velocity.X = maxVelocity.X;
                    }

                    if (velocity.Y < -maxVelocity.Y)
                    {
                        velocity.Y = -maxVelocity.Y;
                    }
                    else if (velocity.Y > maxVelocity.Y)
                    {
                        velocity.Y = maxVelocity.Y;
                    }

                    if (Math.Abs(distance.X) > Math.Abs(previousDistance.X) && Math.Abs(distance.Y) > Math.Abs(previousDistance.Y))
                        targetEnemy = true;
                }
                else
                {
                    Vector2 distance = new Vector2(enemy.Position.X - position.X, enemy.Position.Y - position.Y);
                    float angle = (float)Math.Atan2(distance.Y, distance.X);
                    velocity += new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 0.5f;
                    if (velocity.X < -maxVelocity.X)
                    {
                        velocity.X = -maxVelocity.X;
                    }
                    else if (velocity.X > maxVelocity.X)
                    {
                        velocity.X = maxVelocity.X;
                    }

                    if (velocity.Y < -maxVelocity.Y)
                    {
                        velocity.Y = -maxVelocity.Y;
                    }
                    else if (velocity.Y > maxVelocity.Y)
                    {
                        velocity.Y = maxVelocity.Y;
                    }
                    if (hitbox.Intersects(enemy.Hitbox))
                        isVisible = false;
                }
            }
            position += velocity;
            InitialisationHitbox();
        }

        /// <summary>
        /// Update principale des projectiles normaux
        /// </summary>
        /// <param name="playerShip"></param>


        public void Update(PlayerShip playerShip)
        {
            UpdateBullets();
        }

        /// <summary>
        /// Update principale des projectiles speciaux
        /// </summary>
        /// <param name="playerShipPosition"></param>
        /// <param name="enemy"></param>
        /// <param name="playerShip"></param>

        public void UpdateSpecial(Vector2 playerShipPosition, YellowEnemy enemy, PlayerShip playerShip)
        {
            Rotation();
            UpdateSpecialBullets(playerShipPosition, enemy);
        }

        /// <summary>
        /// Fonction Draw principale
        /// </summary>
        /// <param name="spriteBatch"></param>

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(width / 2, height / 2);
            spriteBatch.Draw(texture, position, rectangleSource, Color.White, angle, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}

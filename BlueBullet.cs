using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Shmup
{
    class BlueBullet : Bullet
    {
        SoundEffect soundEffectSpecial; //le son des projectiles spéciaux

        public SoundEffect SoundEffectSpecial { get => soundEffectSpecial; set => soundEffectSpecial = value; }

        /// <summary>
        /// Constructeur du projectile bleu
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Texture"></param>

        public BlueBullet(int Width, int Height) : base(Width, Height)
        {
            spriteIndex = Sprites.Blue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bullet"></param>

        public BlueBullet(Bullet bullet) : base(bullet)
        {
            spriteIndex = Sprites.Blue;
        }

        /// <summary>
        /// Initialise la hitbox du laser
        /// </summary>

        public void InitialisationHitboxLaser()
        {
            switch (angle)
            {
                case 0f:
                    hitbox = new Rectangle((int)Position.X, (int)Position.Y, width, height);
                    break;

                case (float)Math.PI:
                    hitbox = new Rectangle((int)Position.X, 0, width, height);
                    break;

                case (float)Math.PI / 2:
                    hitbox = new Rectangle(0, (int)Position.Y, height, width);
                    break;

                case -(float)Math.PI / 2:
                    hitbox = new Rectangle((int)Position.X, (int)Position.Y, height, width);
                    break;
                default:
                    hitbox = new Rectangle((int)Position.X, (int)Position.Y, width, height);
                    break;
            }
        }

        /// <summary>
        /// Update le laser en fonction du temps et de l'état de l'ennemi
        /// </summary>
        /// <param name="ship"></param>

        public void UpdateLaser(BlueEnemy ship)
        {
            if (!ship.IsAlive)
            {
                hitbox = new Rectangle(-width, -height, 0, 0);
                isVisible = false;
            }

            else
            {
                if (ship.IsCreated)
                {
                    if (ship.TimeAttack >= 7)
                    {
                        position += ship.Velocity;
                        InitialisationHitboxLaser();
                        SoundEffectInstance soundInstance = soundEffect.CreateInstance();
                        soundInstance.Volume = 0.3f;
                        soundInstance.Play();
                        if (ship.TimeAttack >= 9)
                            soundInstance.Stop();
                    }
                }
                else
                {
                    isVisible = false;
                    hitbox = new Rectangle(-width, -height, 0, 0);
                }
                    
            }
        }


        /// <summary>
        /// Pour gérer la collision du laser avec le vaisseau du joueur
        /// </summary>
        /// <param name="ship"></param>

        public void CollisionShipSpecial(PlayerShip ship)
        {
            if (hitbox.Intersects(ship.Hitbox))
            {
                if ((int)ship.ShipColor != (int)Bullet.Sprites.Blue)
                    ship.IsAlive = false;
            }
        }

        /// <summary>
        /// Pour afficher les projectiles du vaisseau bleu
        /// </summary>
        /// <param name="spriteBatch"></param>

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangleSource, Color.White, angle, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }        
    }
}

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
    class BlueEnemy : Enemies
    {
        List<BlueBullet> listBullet = new List<BlueBullet>();
        BlueBullet laser;
        
        /// <summary>
        /// Constructeur du vaisseau bleu
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="SpriteIndex"></param>

        public BlueEnemy(int Width, int Height, Sprites SpriteIndex) : base(Width, Height, SpriteIndex)
        {
            rectangleSource = new Rectangle(0, 0, Width, Height);
            rectangleSourceCollision = new Rectangle(3225, 0, Width, Height);
            pvMax = 600;
            pv = pvMax;
            color = Color.Blue;
            points = 100;
        }

        /// <summary>
        /// Initialise l'angle du vaisseau bleu en fonction de son spawn
        /// </summary>

        public void InitializeAngle()
        {
            switch (spawn)
            {
                case 1:
                    angle = 0f;
                    break;
                case 2:
                    angle = (float)Math.PI / 2;
                    break;
                case 3:
                    angle = (float)Math.PI;
                    break;
                case 4:
                    angle = -(float)Math.PI / 2;
                    break;
            }
        }

        /// <summary>
        /// Pour reset les parametres lorsqu'on relance une partie
        /// </summary>

        public new void Reset()
        {           
            listBullet.Clear();
            laser.IsVisible = false;
            base.Reset();
        }

 

        /// <summary>
        /// Initialisation du laser
        /// </summary>
        /// <param name="bullet"></param>

        public void InitialisationLaser(BlueBullet bullet)
        {
            laser = new BlueBullet(bullet.Width, bullet.Height)
            {
                Texture = bullet.Texture,
                RectangleSource = new Rectangle(0, 0, bullet.Width, bullet.Height),
                SoundEffect = bullet.SoundEffectSpecial,
                Angle = angle
            };
        }

        /// <summary>
        ///  Pour actualiser le vaisseau bleu
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="charge"></param>
        /// <param name="bullet"></param>
        /// <param name="gameTime"></param>
        /// <param name="particle"></param>

        public void Update(PlayerShip ship, Animation charge, BlueBullet bullet, GameTime gameTime, ParticleManager particle)
        {
            if (isAlive)
            {
                Move();
                InitializeAngle();
            }
                
            Update(ship, gameTime, particle);            
            UpdateBullets(ship, bullet, gameTime);

            //on affiche l'animation de charge du laser
            if (isCharging)
            {
                charge.Position = position;
                charge.Update(gameTime);
            }
            else
                charge.CurrentFrame = 0;           
        }

        /// <summary>
        /// Création des projectiles normaux
        /// </summary>
        /// <param name="playerShip"></param>
        /// <param name="bullet"></param>
        /// <param name="gameTime"></param>

        public void Fire(PlayerShip playerShip, Bullet bullet, GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (time >= 1000)
            {
                time = 0.0;
                BlueBullet blueBullet = new BlueBullet(bullet)
                {
                    RectangleSource = new Rectangle(44, 0, bullet.Width, bullet.Height),
                    Angle = angle,
                    Velocity = new Vector2((float)Math.Cos(angle + Math.PI / 2), (float)Math.Sin(angle + Math.PI / 2)) * 4f,
                    Position = new Vector2((float)Math.Cos(angle) + position.X, (float)Math.Sin(angle)  + position.Y)
                };
               
                blueBullet.InitialisationHitbox(this);
                listBullet.Add(blueBullet);
                DistanceSound(playerShip, bullet, position);
            }
        }

        /// <summary>
        /// Creation du laser
        /// </summary>

        public void FireLaser()
        {
            isCreated = true;
            laser.IsVisible = true;
            laser.Angle = angle;
            laser.Position = new Vector2((float)Math.Cos(angle) + position.X, (float)Math.Sin(angle) + position.Y);
            laser.InitialisationHitboxLaser();
        }

        /// <summary>
        /// Update des projectiles
        /// </summary>
        /// <param name="playerShip"></param>
        /// <param name="blueBullet"></param>
        /// <param name="gameTime"></param>

        public void UpdateBullets(PlayerShip playerShip, Bullet blueBullet, GameTime gameTime)
        {
            laser.UpdateLaser(this);
            laser.CollisionShipSpecial(playerShip);

            if (isAlive)
            {
           /* Durant les 5 premières secondes, le vaisseau tire des projectiles classiques.
            * Ensuite, le vaisseau charge son laser durant un peu plus d'une seconde
            * Enfin, le vaisseau envoie son laser
            */
                timeAttack += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeAttack < 5)
                    Fire(playerShip, blueBullet, gameTime);
                else
                {
                    if (timeAttack >= 5.75 && !isCreated)
                        isCharging = true;

                    if (timeAttack >= 7 && !isCreated)
                    {
                        isCharging = false;
                        FireLaser();
                        isCreated = true;
                    }
                }
                if (timeAttack >= 10 && isCreated)
                {
                    isCreated = false;
                    timeAttack = 0;                  

                }
            }

            foreach(BlueBullet bullet in listBullet)
            {
                bullet.UpdateBullets();
                bullet.CollisionShip(playerShip);
            }                     

            for (int i = 0; i < listBullet.Count; i++)
            {
                if (!listBullet[i].IsVisible)
                    listBullet.RemoveAt(i);
            }
        }
        

        /// <summary>
        /// Pour afficher le vaisseau
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="explosion"></param>
        /// <param name="charge"></param>

        public void Draw(SpriteBatch spriteBatch, Animation charge, ParticleManager particle)
        {            

            if (!isAlive)
                particle.Draw(spriteBatch);
            else
            {
                if (!hasCollided)
                    spriteBatch.Draw(texture, position, rectangleSource, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
                else
                {
                    spriteBatch.Draw(texture, position, rectangleSourceCollision, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
                    hasCollided = false;
                }
                if (isCharging)
                {
                    charge.DrawRotate(spriteBatch, angle, origin);
                }
                if (!charge.active)
                    charge.active = true;

                if (laser.IsVisible)
                    laser.Draw(spriteBatch);
            }

            foreach (BlueBullet bullet in listBullet)
                bullet.Draw(spriteBatch);
        }       
    }
}

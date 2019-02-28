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
    class GreenEnemy : Enemies
    {        
        List<GreenBullet> listBullet = new List<GreenBullet>(); //la liste contenant les projectiles classiques
        List<GreenBullet> specialAttack = new List<GreenBullet>(); //la liste contenant les projectiles spéciaux       
        int count;


        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="SpriteIndex"></param>

        public GreenEnemy(int Width, int Height, Sprites SpriteIndex) : base(Width, Height, SpriteIndex)
        {
            rectangleSource = new Rectangle(0, 0, Width, Height);
            rectangleSourceCollision = new Rectangle(1507, 0, Width, Height);
            pvMax = 700;
            pv = pvMax;
            origin = new Vector2(width / 2, height / 2);
            color = Color.Green;
            points = 100;
        }        

        public new void Reset()
        {
            base.Reset();
            listBullet.Clear();
            specialAttack.Clear();
        }
        /// <summary>
        /// Fait tourner sur lui-même le vaisseau
        /// </summary>
        /// <param name="bullet"></param>

        public void Rotate(GameTime gameTime)
        {
            //si le vaisseau charge ses projectiles spéciaux, il tourne plus rapidement
            if (timeAttack >= 9.8 && timeAttack <= 10.1)
                angle += 0.1f;
            else
                angle += 0.01f;
        }

        public void Fire(PlayerShip playerShip, Bullet bullet, GameTime gameTime)
        {
            //Créé des projectiles toutes les secondes, seulement s'il n'y a pas deja 5 projectiles deja affichés sur l'écran
            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (listBullet.Count < 12 && time >= 1000)
            {
                time = 0;
                Random rnd = new Random();
                double angle = rnd.NextDouble() * 2 * Math.PI;
                GreenBullet greenBullet = new GreenBullet(bullet)
                {
                    Texture = bullet.Texture,
                    RectangleSource = new Rectangle(11, 0, bullet.Width, bullet.Height),
                    Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 4f 
                };                              
                
                greenBullet.Position = greenBullet.Velocity + position;
                greenBullet.InitialisationHitbox(this);
                listBullet.Add(greenBullet);
                DistanceSound(playerShip, bullet, position);
            }
        }

        public void FireSpecial(PlayerShip playerShip, Bullet bullet)
        {
            Vector2 distance = Vector2.Zero;
            float angleBullet = 0f;
            while (specialAttack.Count < count + 80)
            {

                GreenBullet shipBullet = new GreenBullet(bullet.Width, bullet.Height)
                {
                    Texture = bullet.Texture,
                    RectangleSource = new Rectangle(11, 0, bullet.Width, bullet.Height),
                    Velocity = new Vector2((float)Math.Cos(angleBullet), (float)Math.Sin(angleBullet)) * 4f
                };
                shipBullet.Position = position + shipBullet.Velocity * 5;
                
                shipBullet.InitialisationHitbox(this);
                specialAttack.Add(shipBullet);
                angleBullet += 0.08f;                
            }
            count = count + 80;
            DistanceSound(playerShip, bullet, position);
        }


        public void UpdateBullets(Bullet bulletg, GameTime gameTime, PlayerShip playerShip)
        {
            timeAttack += gameTime.ElapsedGameTime.TotalSeconds;
            if (isAlive)
            {
                if (timeAttack <= 9)
                    Fire(playerShip, bulletg, gameTime);
                else
                {
                    if (timeAttack >= 9.8 && !isCreated)
                    {
                        isCharging = true;
                    }

                    if (timeAttack >= 10)
                    {
                        isCharging = false;
                        if(!isCreated)
                        {
                            FireSpecial(playerShip, bulletg);
                            isCreated = true;
                        }                        
                    }
                }

                //si tous les projectiles ne sont plus visibles sur l'écran, on remet le timer à 0
                if (timeAttack >= 12)
                {
                    isCreated = false;
                    timeAttack = 0;
                }
            }


            foreach (GreenBullet bullet in listBullet)
            {
                bullet.UpdateBullets();
                bullet.CollisionShip(playerShip);
            }

            foreach (GreenBullet bullet in specialAttack)
            {
                bullet.UpdateSpecialBullets();
                bullet.CollisionShip(playerShip);
            }


            for (int i = 0; i < listBullet.Count; i++)
            {
                if (!listBullet[i].IsVisible)
                    listBullet.RemoveAt(i);
            }

            for (int i = 0; i < specialAttack.Count; i++)
            {
                if (!specialAttack[i].IsVisible)
                {
                    specialAttack.RemoveAt(i);
                    count--;
                }
                   
            }
        }

        /// <summary>
        /// fonction update principale
        /// </summary>
        /// <param name="shipBullet"></param>
        /// <param name="ship"></param>
        /// <param name="explosion"></param>
        /// <param name="charge"></param>
        /// <param name="gameTime"></param>
        /// <param name="bullet"></param>
        /// <param name="score"></param>

        public void Update(PlayerShip ship, Animation charge, GameTime gameTime, Bullet bullet, ParticleManager particle)
        {
            if (isAlive)
                Move();
            Rotate(gameTime);
            Update(ship, gameTime, particle);
            UpdateBullets(bullet, gameTime, ship);

            if (isCharging)
            {
                charge.Position = position;
                charge.Update(gameTime);
            }
            else
            {
                charge.CurrentFrame = 0;
            }
        }

        /// <summary>
        /// Fonction Draw principale
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="explosion"></param>
        /// <param name="charge"></param>

        public void Draw(SpriteBatch spriteBatch, Animation charge, ParticleManager particle)
        {

            if (!isAlive)
            {
                particle.Draw(spriteBatch);
            }
            else
            {
                if (!HasCollided)
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
            }

            foreach (GreenBullet bullet in listBullet)
                bullet.Draw(spriteBatch);

            foreach (GreenBullet bullet in specialAttack)
                bullet.Draw(spriteBatch);
        }     
    }
}


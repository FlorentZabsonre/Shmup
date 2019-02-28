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
    class RedEnemy : Enemies
    {
        List<RedBullet> listBullet = new List<RedBullet>(); //liste contenant les projectiles classiques
        List<RedBullet> specialAttack = new List<RedBullet>(); //liste contenant les projectiles spéciaux

        Vector2 distance; //la distance entre le vaisseau du joueur et du vaisseau
        float originalRotation;
        public Vector2 Distance { get => distance; set => distance = value; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="SpriteIndex"></param>

        public RedEnemy(int Width, int Height, Sprites SpriteIndex) : base(Width, Height, SpriteIndex)
        {
            rectangleSource = new Rectangle(0, 0, Width, Height);
            rectangleSourceCollision = new Rectangle(742, 0, Width, Height);
            pvMax = 550;
            pv = pvMax;
            origin = new Vector2(width / 2, height / 2);
            color = Color.Red;
            points = 100;
        }

        /// <summary>
        /// Pour reinitialiser les vaisseaux
        /// </summary>

        public new void Reset()
        {            
            distance = Vector2.Zero;
            originalRotation = 0f;            
            listBullet.Clear();
            specialAttack.Clear();
            base.Reset();
        }

        /// <summary>
        /// Initialisation de la hitbox du vaisseau
        /// </summary>

       

        /// <summary>
        /// Fonction update principale
        /// </summary>
        /// <param name="playerShip"></param>
        /// <param name="charge"></param>
        /// <param name="gameTime"></param>
        /// <param name="bullet"></param>
        /// <param name="particle"></param>

        public void Update(PlayerShip playerShip, Animation charge, GameTime gameTime, Bullet bullet, ParticleManager particle)
        {
            if (isAlive)
                Move();
            Update(playerShip, gameTime, particle);            
            UpdateBullets(playerShip, bullet, gameTime);

            if(timeAttack<13)
                UpdateRotation(playerShip, gameTime);

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
        /// Pour gérer la rotation du vaisseau en fonction de la position du vaisseau du joueur
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="gameTime"></param>

        public void UpdateRotation(PlayerShip ship, GameTime gameTime)
        {
            distance.X = ship.Position.X - position.X;
            distance.Y = ship.Position.Y - position.Y;
            angle = (float)Math.Atan2(distance.Y, distance.X) - ((1f * (float)Math.PI) / 2); //pour trouver l'angle entre le vaisseau ennemi et le vaisseau principal        
        }

        public void Fire(PlayerShip playerShip, Bullet bullet, GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (listBullet.Count < 10 && time >= 1000)
            {
                time = 0.0;
                RedBullet shipBullet = new RedBullet(bullet)
                {
                    Texture = bullet.Texture,
                    RectangleSource = new Rectangle(0, 0, bullet.Width, bullet.Height),                    
                    Angle = angle,
                    /*new Vector2((float)Math.Cos(angle + (((float)Math.PI) / 2)), (float)Math.Sin(angle + (((float)Math.PI) / 2))) permet d'avoir l'angle du vaisseau pour tirer dans la même direction
                    * 4f est la vitesse initiale
                    * shipBullet.Velocity vitesse qui reste constant en fonction de la vitesse du vaisseau en deplacement*/
                    Velocity = new Vector2((float)Math.Cos(angle + (((float)Math.PI) / 2)), (float)Math.Sin(angle + (((float)Math.PI) / 2))) * 4f
                };              
                shipBullet.Position = position + shipBullet.Velocity * 5;                
                shipBullet.InitialisationHitbox(this);
                listBullet.Add(shipBullet);
                DistanceSound(playerShip, bullet, position);
            }
        }

        /// <summary>
        /// Création des projectiles speciaux
        /// </summary>
        /// <param name="playerShip"></param>
        /// <param name="bullet"></param>
        /// <param name="gameTime"></param>


        public void FireSpecial(PlayerShip playerShip, Bullet bullet, GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            angle += 0.04f;
            if (time > 50)
            {
                time = 0;
                RedBullet shipBullet = new RedBullet(bullet)
                {
                    Texture = bullet.Texture,
                    RectangleSource = new Rectangle(0, 0, bullet.Width, bullet.Height)                    
                };
                shipBullet.Angle = angle;
                shipBullet.Velocity = new Vector2((float)Math.Cos(angle + (((float)Math.PI) / 2)), (float)Math.Sin(angle + (((float)Math.PI) / 2))) * 4f;
                shipBullet.Position = position + shipBullet.Velocity * 5;

                shipBullet.InitialisationHitbox(this);
                specialAttack.Add(shipBullet);
                DistanceSound(playerShip, bullet, position);
            }
        }

        /// <summary>
        /// Update des projectiles
        /// </summary>
        /// <param name="playerShip"></param>
        /// <param name="redBullet"></param>
        /// <param name="gameTime"></param>


        public void UpdateBullets(PlayerShip playerShip, Bullet redBullet, GameTime gameTime)
        {             
            timeAttack += gameTime.ElapsedGameTime.TotalSeconds;

            if(isAlive)
            {
                if (timeAttack < 13)
                {
                    Fire(playerShip, redBullet, gameTime);                    
                    originalRotation = angle;
                }                                          
                else
                {
                    if (timeAttack >= 13 && !isCreated)
                        isCharging = true;

                    if (timeAttack >= 13.2)
                    {
                        isCharging = false;

                        if (specialAttack.Count == 0)                                                       
                            isCreated = true;

                        
                            if (angle >= 6.28319 + originalRotation)
                            {
                                isCreated = false;
                                timeAttack = 0;
                            }                        
                        FireSpecial(playerShip, redBullet, gameTime);
                    }
                }
            }

            foreach (RedBullet bullet in listBullet)
            {
                bullet.UpdateBullets();
                bullet.CollisionShip(playerShip);
            }
               

            foreach (RedBullet bullet in specialAttack)
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
                    specialAttack.RemoveAt(i);
            }
        }

        /// <summary>
        /// Dessin des sprites
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="charge"></param>
        /// <param name="particle"></param>
        

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
                    charge.DrawRotate(spriteBatch, angle, origin);
                if (!charge.active)
                    charge.active = true;
            }

            foreach (Bullet bullet in listBullet)
                bullet.Draw(spriteBatch);
            foreach (Bullet bullet in specialAttack)
                bullet.Draw(spriteBatch);
        }
    }
}

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
    class YellowEnemy : Enemies
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="SpriteIndex"></param>
    
        List<YellowBullet> listBulletLeft = new List<YellowBullet>(); //liste contenant les projectiles classiques situés à gauche du vaisseau
        List<YellowBullet> listBulletRight = new List<YellowBullet>(); //liste contenant les projectiles classiques situés à droite du vaisseau        

        Dictionary<YellowBullet, Vector2> specialAttackLeft = new Dictionary<YellowBullet, Vector2>(); //liste contenant les projectiles classiques situés à gauche du vaisseau
        Dictionary<YellowBullet, Vector2> specialAttackRight = new Dictionary<YellowBullet, Vector2>(); //liste contenant les projectiles classiques situés à droite du vaisseau        
        Vector2 distance; //la distance entre le vaisseau du joueur et du vaisseau
        float originalAngle;

        public float OriginalAngle { get => originalAngle; set => originalAngle = value; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="SpriteIndex"></param>

        public YellowEnemy(int Width, int Height, Sprites SpriteIndex) : base(Width, Height, SpriteIndex)
        {
            rectangleSource = new Rectangle(0, 0, Width, Height);
            rectangleSourceCollision = new Rectangle(657, 0, Width, Height);
            pvMax = 600;
            pv = pvMax;
            color = Color.Yellow;
            origin = new Vector2(width / 2, height / 2);
            points = 100;
        }

        /// <summary>
        /// Reset les parametres lorsque l'on relance une partie
        /// </summary>

        public new void Reset()
        {
            listBulletLeft.Clear();
            listBulletRight.Clear();
            specialAttackLeft.Clear();
            specialAttackRight.Clear();
            distance = Vector2.Zero;
            originalAngle = 0;
            base.Reset();
        }

        /// <summary>
        /// Rotation du vaisseau
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="gameTime"></param>


        public void UpdateRotation(PlayerShip ship, GameTime gameTime)
        {
            distance.X = ship.Position.X - position.X;
            distance.Y = ship.Position.Y - position.Y;
            angle = (float)Math.Atan2(distance.Y, distance.X) - ((1f * (float)Math.PI) / 2); //pour trouver l'angle entre le vaisseau ennemi et le vaisseau principal
        }

        /// <summary>
        /// Creation des projectiles normaux
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
                YellowBullet shipBullet = new YellowBullet(bullet)
                {
                    Texture = bullet.Texture,
                    RectangleSource = new Rectangle(27, 0, bullet.Width, bullet.Height),
                    Angle = angle,
                    Velocity = new Vector2((float)Math.Cos(angle + Math.PI / 2), (float)Math.Sin(angle + Math.PI / 2)) * 4,
                    Position = new Vector2((float)Math.Cos(angle) * (width / 2) + position.X, (float)Math.Sin(angle) * (height / 2) + position.Y)
                };
               

                shipBullet.InitialisationHitbox(this);
                listBulletLeft.Add(shipBullet);


                YellowBullet shipBullet2 = new YellowBullet(bullet.Width, bullet.Height)
                {
                    Texture = bullet.Texture,
                    RectangleSource = new Rectangle(27, 0, bullet.Width, bullet.Height),
                    Velocity = new Vector2((float)Math.Cos(angle + Math.PI / 2), (float)Math.Sin(angle + Math.PI / 2)) * 4f,
                    Position = new Vector2((float)Math.Cos(angle) * (-width / 2) + position.X, (float)Math.Sin(angle) * (-height / 2) + position.Y)
                };


                shipBullet2.InitialisationHitbox(this);
                listBulletRight.Add(shipBullet2);
                DistanceSound(playerShip, bullet, position);
            }
        }

        /// <summary>
        /// Creation des projectiles speciaux
        /// </summary>
        /// <param name="playerShip"></param>
        /// <param name="bullet"></param>
        /// <param name="gameTime"></param>

        public void FireSpecial(PlayerShip playerShip, Bullet bullet, GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (time >= 500)
            {
                time = 0.0;
                YellowBullet shipBullet = new YellowBullet(bullet)
                {
                    Texture = bullet.Texture,
                    RectangleSource = new Rectangle(27, 0, bullet.Width, bullet.Height),
                    Angle = angle,
                    Velocity = new Vector2((float)Math.Cos(angle + Math.PI / 2), (float)Math.Sin(angle + Math.PI / 2)) * 4,
                    Position = new Vector2((float)Math.Cos(angle) * (width / 2) + position.X, (float)Math.Sin(angle) * (height / 2) + position.Y)
                };           
                
                shipBullet.InitialisationHitbox(this);
                specialAttackLeft.Add(shipBullet, playerShip.Position);

                YellowBullet shipBullet2 = new YellowBullet(bullet.Width, bullet.Height)
                {
                    Texture = bullet.Texture,
                    RectangleSource = new Rectangle(27, 0, bullet.Width, bullet.Height),
                    Angle = angle,
                    Velocity = new Vector2((float)Math.Cos(angle + Math.PI / 2), (float)Math.Sin(angle + Math.PI / 2)) * 4f,
                    Position = new Vector2((float)Math.Cos(angle) * (-width / 2) + position.X, (float)Math.Sin(angle) * (-height / 2) + position.Y)
                };

                shipBullet2.InitialisationHitbox(this);
                specialAttackLeft.Add(shipBullet2, playerShip.Position);

                DistanceSound(playerShip, bullet, position);
            }
            isCreated = true;
        }

        /// <summary>
        /// Update des projectiles
        /// </summary>
        /// <param name="playerShip"></param>
        /// <param name="greenBullet"></param>
        /// <param name="gameTime"></param>


        public void UpdateBullets(PlayerShip playerShip, Bullet greenBullet, GameTime gameTime)
        {
            timeAttack += gameTime.ElapsedGameTime.TotalSeconds;
            if (isAlive)
            {
                if (timeAttack < 9)
                {
                    Fire(playerShip, greenBullet, gameTime);
                    originalAngle = angle;
                }
                    
                
                else
                {
                    if (timeAttack >= 9 && !isCreated)
                        isCharging = true;

                    if (timeAttack >= 9.2)
                    {
                        isCharging = false;
                        FireSpecial(playerShip, greenBullet, gameTime);
                    }
                    if (timeAttack >= 13)
                    {
                        timeAttack = 0;
                        angle = originalAngle;
                        isCreated = false;
                    }                        
                }
            }

                
            foreach (YellowBullet bullet in listBulletLeft)
            {
                bullet.Update(playerShip);
                bullet.CollisionShip(playerShip);
            }
               

            foreach (YellowBullet bullet in listBulletRight)
            {
                bullet.Update(playerShip);
                bullet.CollisionShip(playerShip);
            }

            foreach (KeyValuePair<YellowBullet, Vector2> bullet in specialAttackLeft)
            {
                bullet.Key.UpdateSpecial(bullet.Value, this, playerShip);
                bullet.Key.CollisionShip(playerShip);
            }
                

            foreach (KeyValuePair<YellowBullet, Vector2> bullet in specialAttackRight)
            {
                bullet.Key.UpdateSpecial(bullet.Value, this, playerShip);
                bullet.Key.CollisionShip(playerShip);
            }

            for (int i = 0; i < listBulletLeft.Count; i++)
            {
                if (!listBulletLeft[i].IsVisible)
                    listBulletLeft.RemoveAt(i);
            }

            for (int i = 0; i < listBulletRight.Count; i++)
            {
                if (!listBulletRight[i].IsVisible)
                    listBulletRight.RemoveAt(i);
            }

            for (int i = 0; i < specialAttackLeft.Count; i++)
            {
                if (!specialAttackLeft.ElementAt(i).Key.IsVisible)
                    specialAttackLeft.Remove(specialAttackLeft.ElementAt(i).Key);
            }

            for (int i = 0; i < specialAttackRight.Count; i++)
            {
                if (!specialAttackRight.ElementAt(i).Key.IsVisible)
                    specialAttackRight.Remove(specialAttackRight.ElementAt(i).Key);
            }
        }

        /// <summary>
        /// Update du vaisseau en fonction du temps
        /// </summary>
        /// <param name="playerShip"></param>
        /// <param name="charge"></param>
        /// <param name="bullet"></param>
        /// <param name="gameTime"></param>
        /// <param name="particle"></param>

        public void Update(PlayerShip playerShip, Animation charge, Bullet bullet, GameTime gameTime, ParticleManager particle)
        {
            if (isAlive)
                Move();

            if (isCharging)
            {
                charge.Position = position;
                charge.Update(gameTime);
            }
            else
            {
                charge.CurrentFrame = 0;
            }
            if (timeAttack >= 9.2)
                UpdateRotation(playerShip, gameTime);
            Update(playerShip, gameTime, particle);
            UpdateBullets(playerShip, bullet, gameTime);
           
        }



        /// <summary>
        /// Affiche les sprites
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
                               

                if(isCharging)
                {
                    if (isCharging)
                        charge.DrawRotate(spriteBatch, angle, origin);
                    if (!charge.active)
                        charge.active = true;
                }             
            }

            foreach (YellowBullet bullet in listBulletLeft)
                bullet.Draw(spriteBatch);

            foreach (YellowBullet bullet in listBulletRight)
                bullet.Draw(spriteBatch);

            foreach (YellowBullet bullet in specialAttackLeft.Keys)
                bullet.Draw(spriteBatch);

            foreach (YellowBullet bullet in specialAttackRight.Keys)
                bullet.Draw(spriteBatch);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    abstract class Enemies : Ship
    {
        protected Rectangle rectangleSourceCollision; //affiche l'image du sprite utilisé pour afficher une collision avec des projectiles ennemis
        protected int pvMax; //le nombre de pv max qu'a un vaisseau ennemi        
        protected bool hasCollided;        
        protected bool isCharging = false; //pour un savoir si un ennemi charge son attaque spéciale ou non
        protected double time;
        public int start = -1;
        protected Color color;
        protected double timeAttack;
        protected bool isCreated;
        protected int spawn;
        protected int points;
        protected int pv; //les PV actuels des vaisseaux enemies

        protected Sprites spriteIndex;
        public enum Sprites
        {
            Red = 0,
            Green = 1,
            Yellow = 2,
            Blue = 3
        }

        public int PvMax { get => pvMax; set => pvMax = value; }
        public Sprites SpriteIndex { get => spriteIndex; set => spriteIndex = value; }
        public bool HasCollided { get => hasCollided; set => hasCollided = value; }
        public bool IsCharging { get => isCharging; set => isCharging = value; }
        public double TimeAttack { get => timeAttack; set => timeAttack = value; }
        public bool IsCreated { get => isCreated; set => isCreated = value; }
        public int Spawn { get => spawn; set => spawn = value; }
        public int Pv { get => pv; set => pv = value; }
        public int Points { get => points; set => points = value; }

        public Enemies()
        {

        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="SpriteIndex"></param>

        public Enemies(int Width, int Height, Sprites SpriteIndex) : base(Width, Height)
        {
            spriteIndex = SpriteIndex;
        }

        /// <summary>
        /// Le volume du son varie selon la distance entre le vaisseau du joueur et le vaisseau emetteur de projectile
        /// </summary>
        /// <param name="playerShip"></param>
        /// <param name="bullet"></param>
        /// <param name="position"></param>

        public void DistanceSound(PlayerShip playerShip, Bullet bullet, Vector2 position)
        {
            Vector2 distance = new Vector2(Math.Abs(playerShip.Position.X - position.X), Math.Abs(playerShip.Position.Y - position.Y));
            float distanceScale = Math.Max(distance.X, distance.Y);
            try
            {
                bullet.SoundEffect.Play(1f / (distanceScale / 100), 0f, 0f);
            }
            catch
            {
                bullet.SoundEffect.Play(0f, 0f, 0f);
            }
        }

        /// <summary>
        /// Reset les parametres lorsqu'on relance une partie
        /// </summary>

        public new void Reset()
        {
            base.Reset();
            isCharging = false;
            isCreated = false;
            hasCollided = false;
            timeAttack = 0;
            pv = pvMax;
            start = -1;
            time = 0;
            position = Vector2.Zero;
        }
        
        /// <summary>
        /// Deplacement des vaisseaux ennemis
        /// </summary>

        public void Move()
        {            
            switch(spawn)
            {
                case 1:
                    if(velocity==Vector2.Zero)
                        velocity = new Vector2(2, 0);
                    if (position.X >= Level.Width - width)
                        velocity = -velocity;
                    else
                    {
                        if (position.X <= 100)
                            velocity = -velocity;
                    }                      
                    position += velocity;
                    InitialisationHitbox();
                    break;

                case 3:
                    if (velocity == Vector2.Zero)
                        velocity = new Vector2(2, 0);
                    if (position.X >= Level.Width - width - 100)
                        velocity = -velocity;
                    else
                    {
                        if (position.X <= 0)
                            velocity = -velocity;
                    }

                    position += velocity;
                    InitialisationHitbox();
                    break;

                case 2:
                    if (velocity == Vector2.Zero)
                        velocity = new Vector2(0, 2);
                    if (position.Y >= Level.Height - height)
                        velocity = -velocity;
                    else
                    {
                        if (position.Y <= 100)
                            velocity = -velocity;
                    }
                    position += velocity;
                    InitialisationHitbox();
                    break;

                case 4:
                    if (velocity == Vector2.Zero)
                        velocity = new Vector2(0, 2);
                    if (position.Y >= Level.Height - height - 100)
                        velocity = -velocity;
                    else
                    {
                        if (position.Y <= 0)
                            velocity = -velocity;
                    }
                    position += velocity;
                    InitialisationHitbox();
                    break;
            }
        }

       


        /// <summary>
        /// Gère la collision entre un vaisseau ennemi et un projectile du vaisseau du joueur
        /// Si le projectile est de la même couleur que le vaisseau, il le soigne
        /// Si le projectile est de la couleur dominante par rapport à celle du vaisseau, le projectile lui fait deux fois plus de degâts
        /// Bleu > Rouge > Vert > Jaune > Bleu
        /// </summary>
        /// <param name="shipBullet"></param>

        public void CollisionBullet(PlayerShip shipBullet)
        {
            foreach (PlayerShipBullet bullet in shipBullet.ListBulletLeft.ToList())
            {
                if (bullet.Hitbox.Intersects(hitbox))
                {
                    switch ((int)spriteIndex)
                    {
                        //Rouge
                        case 0:

                            //Gère le cas où le projectile est de la même couleur que le vaisseau
                            if (bullet.SpriteIndex == Bullet.Sprites.Red)
                            {
                                if (pv < pvMax)
                                {
                                    if (pv + bullet.Damage > pvMax)
                                        pv = pvMax;
                                    else
                                        pv += bullet.Damage;
                                }
                            }
                            else
                            {
                                //Gère le cas où le projectile a la couleur dominante du vaisseau
                                if (bullet.SpriteIndex == Bullet.Sprites.Blue)
                                    pv -= bullet.Damage * 2;

                                //Gère le dernier cas
                                else
                                    pv -= bullet.Damage;
                            }
                            break;

                        //vert
                        case 1:
                            if (bullet.SpriteIndex == Bullet.Sprites.Green)
                            {
                                if (pv < pvMax)
                                {
                                    if (pv + bullet.Damage > pvMax)
                                        pv = pvMax;
                                    else
                                        pv += bullet.Damage;
                                }
                            }
                            else
                            {
                                if (bullet.SpriteIndex == Bullet.Sprites.Red)
                                    pv -= bullet.Damage * 2;
                                else
                                    pv -= bullet.Damage;
                            }
                            break;

                        //jaune
                        case 2:
                            if (bullet.SpriteIndex == Bullet.Sprites.Yellow)
                            {
                                if (pv < pvMax)
                                {
                                    if (pv + bullet.Damage > pvMax)
                                        pv = pvMax;
                                    else
                                        pv += bullet.Damage;
                                }
                            }
                            else
                            {
                                if (bullet.SpriteIndex == Bullet.Sprites.Green)
                                    pv -= bullet.Damage * 2;
                                else
                                    pv -= bullet.Damage;
                            }
                            break;

                        //bleu
                        case 3:
                            if (bullet.SpriteIndex == Bullet.Sprites.Blue)
                            {
                                if (pv < pvMax)
                                {
                                    if (pv + bullet.Damage > pvMax)
                                        pv = pvMax;
                                    else
                                        pv += bullet.Damage;
                                }
                            }
                            else
                            {
                                if (bullet.SpriteIndex == Bullet.Sprites.Yellow)
                                    pv -= bullet.Damage * 2;
                                else
                                    pv -= bullet.Damage;
                            }
                            break;
                    }

                    //Si le vaisseau a 0 ou moins de PV, il meurt
                    if (pv <= 0)
                        IsAlive = false;
                    hasCollided = true;
                    bullet.IsVisible = false;

                    //Si un projectile a touché le vaisseau, il disparait
                    for (int i = 0; i < shipBullet.ListBulletLeft.Count; i++)
                    {
                        if (!shipBullet.ListBulletLeft[i].IsVisible)
                            shipBullet.ListBulletLeft.RemoveAt(i);

                    }
                }
            }
            foreach (PlayerShipBullet bullet in shipBullet.ListBulletRight.ToList())
            {
                if (bullet.Hitbox.Intersects(hitbox))
                {
                    switch ((int)spriteIndex)
                    {
                        case 0:
                            if (bullet.SpriteIndex == Bullet.Sprites.Red)
                            {
                                if (pv < pvMax)
                                {
                                    if (pv + bullet.Damage > pvMax)
                                        pv = pvMax;
                                    else
                                        pv += bullet.Damage;
                                }
                            }
                            else
                            {
                                if (bullet.SpriteIndex == Bullet.Sprites.Blue)
                                    pv -= bullet.Damage * 2;
                                else
                                    pv -= bullet.Damage;
                            }
                            break;

                        case 1:
                            if (bullet.SpriteIndex == Bullet.Sprites.Green)
                            {
                                if (pv < pvMax)
                                {
                                    if (pv + bullet.Damage > pvMax)
                                        pv = pvMax;
                                    else
                                        pv += bullet.Damage;
                                }
                            }
                            else
                            {
                                if (bullet.SpriteIndex == Bullet.Sprites.Red)
                                    pv -= bullet.Damage * 2;
                                else
                                    pv -= bullet.Damage;
                            }
                            break;

                        case 2:
                            if (bullet.SpriteIndex == Bullet.Sprites.Yellow)
                            {
                                if (pv < pvMax)
                                {
                                    if (pv + bullet.Damage > pvMax)
                                        pv = pvMax;
                                    else
                                        pv += bullet.Damage;
                                }
                            }
                            else
                            {
                                if (bullet.SpriteIndex == Bullet.Sprites.Green)
                                    pv -= bullet.Damage * 2;
                                else
                                    pv -= bullet.Damage;
                            }
                            break;

                        case 3:
                            if (bullet.SpriteIndex == Bullet.Sprites.Blue)
                            {
                                if (pv < pvMax)
                                {
                                    if (pv + bullet.Damage > pvMax)
                                        pv = pvMax;
                                    else
                                        pv += bullet.Damage;
                                }
                            }
                            else
                            {
                                if (bullet.SpriteIndex == Bullet.Sprites.Yellow)
                                    pv -= bullet.Damage * 2;
                                else
                                    pv -= bullet.Damage;
                            }
                            break;
                    }

                    if (pv <= 0)
                        IsAlive = false;
                    hasCollided = true;
                    bullet.IsVisible = false;

                    for (int i = 0; i < shipBullet.ListBulletRight.Count; i++)
                    {
                        if (!shipBullet.ListBulletRight[i].IsVisible)
                            shipBullet.ListBulletRight.RemoveAt(i);

                    }
                }
            }
        }

        /// <summary>
        /// Gère la collision entre le vaisseau du joueur et un vaisseau ennemi. Le vaisseau du joueur meurt
        /// </summary>
        /// <param name="ship"></param>

        public void CollisionMainShip(PlayerShip ship)
        {
            if (ship.Hitbox.Intersects(hitbox))
            {
                ship.IsAlive = false;
            }
               
        }

        /// <summary>
        /// Gere la creation et l'update des particules lorsque le vaisseau explose
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="particle"></param>

        public void Particles(GameTime gameTime, ParticleManager particle)
        {
            if (!isAlive)
            {
                if (!HasExploded)
                {                    
                    start = particle.Initialize(this, color);
                    HasExploded = true;

                    GamePad.SetVibration(PlayerIndex.One, 0.8f, 0.8f);
                    SoundEffectInstance explosionInstance = SoundEffectExplosion.CreateInstance();
                    explosionInstance.Volume = 0.05f;
                    explosionInstance.Play();
                    Shmup.Score += points * Shmup.Multiplicateur;
                    Shmup.Multiplicateur++;
                    Shmup.TimeResetMultiplicateur = 0;
                }
                else
                {                    
                    while (start>=particle.Particles.Count)
                        start -= (particle.Capacity / 15);
                    double framerate = (1 / gameTime.ElapsedGameTime.TotalSeconds);
                    timeParticle += gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeParticle * framerate <= particle.Particle.TimeToLive+1)
                        particle.UpdateParticle(start);
                    if(timeParticle>=0.2 && timeParticle<=0.3)
                        GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                }
                ReinitialisationHitbox();
            }
        }



        /// <summary>
        /// Fonction update principale
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="gameTime"></param>
        /// <param name="particle"></param>

        public void Update(PlayerShip ship, GameTime gameTime, ParticleManager particle)
        {
            CollisionBullet(ship);
            CollisionMainShip(ship);
            Particles(gameTime, particle);
        }
    }
}


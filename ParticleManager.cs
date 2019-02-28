using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    class ParticleManager
    {
        List<Particle> particles = new List<Particle>();
        Particle particle;
        Random rnd;
        int capacity;        
        int start;
        int count;

        public Particle Particle { get => particle; set => particle = value; }
        public bool IsCreated { get; internal set; }
        public int Capacity { get => capacity; set => capacity = value; }
        internal List<Particle> Particles { get => particles; set => particles = value; }
        public int Start { get => start; set => start = value; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="TTL"></param>
        /// <param name="Size"></param>
        /// <param name="Capacity"></param>

        public ParticleManager(float TTL, int Size, int Capacity)
        {
            particle = new Particle(TTL);
            capacity = Capacity;
            rnd = new Random();
            particles = new List<Particle>();
        }

        /// <summary>
        /// Initialisation des particules
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="colorEnemy"></param>
        /// <returns></returns>

        public int Initialize(Ship ship, Color colorEnemy)
        {
            while (count <= ((capacity / 15)-1))
            {
                Particle _particle = new Particle(particle);

                float length;
                double angle;

                if (ship.GetType() == typeof(PlayerShip)) //on vérifie de quel type est ship
                   length = 20f * (1f - 1 / ((float)rnd.NextDouble() * (10f - 1f) + 1f));
                else
                    length = 15f * (1f - 1 / ((float)rnd.NextDouble() * (7f - 1f) + 1f));
                
                angle = rnd.NextDouble() * 2 * Math.PI;

                _particle.Velocity = new Vector2(length * (float)Math.Cos(angle), length * (float)Math.Sin(angle));
                _particle.Position = ship.Position + _particle.Velocity;
                _particle.Angle = (float)Math.Atan2(_particle.Velocity.Y, _particle.Velocity.X);
                _particle.Color = CreateColor(colorEnemy);

                particles.Add(_particle);
                count++;
                start++;
            }
            count = 0;
           return (start-(capacity/15)) ;
        }

        /// <summary>
        /// Les couleurs sont choisis aléatoirement dans la même gamme que la couleur du vaisseau
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>

        public Color CreateColor(Color color)
        {
            int random = -1;
            if (color == Color.White)
                random = rnd.Next(0, 4);

            if (color == Color.Red || random == 0)
                color = new Color(255, rnd.Next(0, 60), rnd.Next(0, 100));
            else
            {
                if (color == Color.Green || random == 1)
                    color = new Color(rnd.Next(0, 150), 255, rnd.Next(0, 150));
                else
                {
                    if (color == Color.Yellow || random == 2)
                        color = new Color(255, rnd.Next(100, 225), rnd.Next(0,100));
                    else
                    {
                        if (color == Color.Blue || random == 3)
                            color = new Color(0, rnd.Next(0, 255), 255);
                    }

                }
            }
            return color;
        }

        /// <summary>
        /// Suppression des particules
        /// </summary>

        public void DeleteParticles()
        {
            for(int i = particles.Count-1; i>=0; i--)
            {
                if (particles[i].TimeToLive == 0)
                    particles[i].IsVisible = false;
                if (!particles[i].IsVisible)
                {
                    particles.RemoveAt(i);
                    start--;
                }                 
            }
        }

        /// <summary>
        /// Update des particules
        /// </summary>
        /// <param name="Start"></param>

        public void UpdateParticle(int Start)
        {            
            for(int i = Start; i <= (Start + (capacity/15))-1; i++)
                particles[i].Update();            
        }

        /// <summary>
        /// Affichage des particules
        /// </summary>
        /// <param name="spriteBatch"></param>

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
                particle.Draw(spriteBatch);
        }
    }
}

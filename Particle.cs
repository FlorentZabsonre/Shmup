using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    class Particle
    {
        Texture2D texture;
        Vector2 position;
        Vector2 velocity;
        Vector2 origin;
        Color color;
        float angle;
        float timeToLive;
        float size;
        float luminosity;
        bool isVisible;
        const float diminution= 0.97f;

        /// <summary>
        /// Constructeur avec Time To Live
        /// </summary>
        /// <param name="TTL"></param>

        public Particle(float TTL)
        {
            position = Vector2.Zero;
            timeToLive = TTL;
            size = 1f;
            velocity = Vector2.Zero;
            luminosity = 1f;
            isVisible = true;
        }

        /// <summary>
        /// Constructeur avec particle
        /// </summary>
        /// <param name="particle"></param>

        public Particle(Particle particle)
        {
            texture = particle.Texture;
            position = Vector2.Zero;
            timeToLive = particle.TimeToLive;
            size = 1f;
            velocity = Vector2.Zero;
            luminosity = 1f;
            isVisible = true;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public Texture2D Texture { get => texture; set => texture = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Color Color { get => color; set => color = value; }
        public float Angle { get => angle; set => angle = value; }
        public float TimeToLive { get => timeToLive; set => timeToLive = value; }
        public float Size { get => size; set => size = value; }
        public bool IsVisible { get => isVisible; set => isVisible = value; }
        public float Luminosity { get => luminosity; set => luminosity = value; }

        /// <summary>
        /// Update des particules jusqu'à ce qu'elles disparaissent
        /// </summary>


        public void Update()
        {
            timeToLive--;
            position += velocity;

            luminosity *= diminution;
            color.A = (byte)(255 * luminosity);
            
            size *= diminution;

            velocity *= diminution;
            if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 0.01f)
                velocity = Vector2.Zero;
        }

        /// <summary>
        /// Affichage des particules
        /// </summary>
        /// <param name="spriteBatch"></param>

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, angle, origin, size, SpriteEffects.None, 1);
        }
    }
}

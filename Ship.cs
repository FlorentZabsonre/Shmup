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
    abstract class Ship : IUpdateHitbox
    {

        protected Vector2 position; //Position X et Y des objets
        protected Texture2D texture; //Charger les sprites des objets
        protected Rectangle rectangleSource; //Avoir les coordonnées un sprite dans un fichier en contenant plusieurs
        protected Rectangle hitbox; //Hitbox des objets
        protected Vector2 velocity; //Vitesse de déplacement des objets
        protected int width; //la largeur de l'objet
        protected int height; //la hauteur de l'objet        
        protected bool isAlive; //pour savoir si le vaisseau est en vie ou non   
        protected SoundEffect soundEffectExplosion; //le son des explosions
        private bool hasExploded = false; //afin de savoir si le vaisseau a explosé ou non
        protected double timeParticle;
        protected float angle;
        protected Vector2 origin;


        public Rectangle Hitbox { get => hitbox; set => hitbox = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Texture2D Texture { get => texture; set => texture = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Rectangle RectangleSource { get => rectangleSource; set => rectangleSource = value; }
        public int Width { get => width; }
        public int Height { get => height; }        
        public bool IsAlive { get => isAlive; set => isAlive = value; }        
        public SoundEffect SoundEffectExplosion { get => soundEffectExplosion; set => soundEffectExplosion = value; }
        public Vector2 Origin { get => origin; set => origin = value; }
        public float Angle { get => angle; set => angle = value; }
        public bool HasExploded { get => hasExploded; set => hasExploded = value; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>

        public Ship(int Width, int Height)
        {
            width = Width;
            height = Height;
            IsAlive = true;
            origin = new Vector2(width / 2, height / 2);
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>

        public Ship()
        {
        }

        /// <summary>
        /// Initialise la hitbox du vaisseau
        /// </summary>

        public void InitialisationHitbox()
        {
            hitbox = new Rectangle((int)(Position.X - origin.X), (int)(Position.Y - origin.Y), width, height);
        }


        /// <summary>
        /// Reinitialisation des parametres lors de la création d'une nouvelle partie
        /// </summary>

        public void Reset()
        {
            velocity = Vector2.Zero;
            isAlive = true;
            hasExploded = false;
            timeParticle = 0f;
            angle = 0f;
        }

        /// <summary>
        /// Supprime les projectiles si elles ne sont plus visibles
        /// </summary>
        /// <param name="list"></param>
      
        public void RemoveBullets(List<Bullet> list)
        {
            for (int i = 0; i < list.Count; i++)
                if (!list[i].IsVisible)
                    list.RemoveAt(i);
        }


        /// <summary>
        /// Décharge la hitbox du vaisseau ennemi s'il meurt
        /// </summary>

        public void ReinitialisationHitbox()
        {
            hitbox.X = -500;
            hitbox.Y = -500;
            hitbox.Width = 0;
            hitbox.Height = 0;
        }


    }
}


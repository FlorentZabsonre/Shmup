using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Shmup
{
    abstract class Bullet : IUpdateHitbox
    {

        protected Vector2 position; //Position X et Y des objets
        protected Texture2D texture; //Charger les sprites des objets
        protected Rectangle rectangleSource; //Avoir les coordonnées un sprite dans un fichier en contenant plusieurs
        protected Rectangle hitbox; //Hitbox des objets
        protected Vector2 velocity; //Vitesse de déplacement des objets
        protected int width; //la largeur de l'objet
        protected int height; //la hauteur de l'objet
        protected SoundEffect soundEffect; //le son des projectiles
        protected Sprites spriteIndex; //l'index qui permet des savoir de quelle couleur est le projectile
        protected bool isVisible; //définit si le projectile est visible dans l'écran ou non afin de le décharger
        protected float angle;


        public Rectangle Hitbox { get => hitbox; set => hitbox = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Texture2D Texture { get => texture; set => texture = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Rectangle RectangleSource { get => rectangleSource; set => rectangleSource = value; }
        public int Width { get => width; set => width = value;  }
        public int Height { get => height; set => height = value;  }
       
        public SoundEffect SoundEffect { get => soundEffect; set => soundEffect = value; }
        public bool IsVisible { get => isVisible; set => isVisible = value; }
        public Sprites SpriteIndex { get => spriteIndex; set => spriteIndex = value; }
        public float Angle { get => angle; set => angle = value; }

        public enum Sprites
        {
            Red = 0,
            Green = 1,
            Yellow = 2,
            Blue = 3
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bullet"></param>

        public Bullet(Bullet bullet)
        {
            width = bullet.Width;
            height = bullet.Height;
            isVisible = true;
            texture = bullet.Texture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// 
        public Bullet(int Width, int Height)
        {
            width = Width;
            height = Height;
            isVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>

        public Bullet()
        {

        }

        /// <summary>
        /// Initialisation de la hitbox
        /// </summary>

        public void InitialisationHitbox()
        {
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }

        

        /// <summary>
        /// Décharge la hitbox du vaisseau ennemi s'il meurt
        /// </summary>


        public void ReinitialisationHitbox()
        {
            hitbox.X = -10;
            hitbox.Y = -10;
            hitbox.Width = 0;
            hitbox.Height = 0;
        }

        /// <summary>
        /// Initialise la hitbox des projectiles en fonction de la position du vaisseau mère
        /// </summary>
        /// <param name="ship"></param>

        public void InitialisationHitbox(Ship ship)
        {
            hitbox = new Rectangle(ship.Hitbox.X, ship.Hitbox.Y, width, height);
        }

        /// <summary>
        /// Update des projectiles
        /// </summary>

        public void UpdateBullets()
        {
            position += velocity;
            InitialisationHitbox();

            if (position.Y > Level.Height - height || position.Y < 0 || position.X < 0 || position.X > Level.Width - width)
                isVisible = false;
        }

        /// <summary>
        ///  le vaisseau du joueur n'est plus en vie s'il touche le vaisseau
        /// </summary>
        /// <param name="ship"></param>

        public void CollisionShip(PlayerShip ship)
        {
            if (hitbox.Intersects(ship.Hitbox))
            {
                if ((int)ship.ShipColor != (int)spriteIndex)
                {
                    ship.IsAlive = false;
                    isVisible = false;
                }
            }
        }

        /// <summary>
        /// Méthode abstraite qui dessine les sprites
        /// </summary>
        /// <param name="spriteBatch"></param>

        public abstract void Draw(SpriteBatch spriteBatch);


    }
}

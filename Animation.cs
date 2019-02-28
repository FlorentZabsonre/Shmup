using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    class Animation
    {
        Texture2D texture;  //La texture du sprite à animer
        Rectangle rectangleSource; //La position du sprite dans l'image
        Rectangle rectangleDestination; //La hitbox du sprite
        Vector2 position; //La position du sprite
        int width; //La largeur du sprite
        int height; //la hauteur du sprite
        float scale; //l'échelle à afficher du sprite
        int nbFrames; //le nombre de sprites differents contenus dans l'image
        int currentFrame; //l'indice de la position actuelle du sprite affiché lors de l'animation
        int frameTime; //le temps qui s'écoule entre l'affichage de deux sprites
        bool restart; //si l'animation se relance ou non
        int time; //pour comparer le temps de jeu avec frameTime
        public bool active; //si l'animation est activée ou non


        public Texture2D Texture { get => texture; set => texture = value; }
        public int Height { get => height; set => height = value; }
        public int NbFrames { get => nbFrames; set => nbFrames = value; }
        public int FrameTime { get => frameTime; set => frameTime = value; }
        public bool Restart { get => restart; set => restart = value; }
        public int Width { get => width; set => width = value; }
        public int CurrentFrame { get => currentFrame; set => currentFrame = value; }
        public Vector2 Position { get => position; set => position = value; }
        public float Scale { get => scale; set => scale = value; }


        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Texture"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="NbFrames"></param>
        /// <param name="FrameRate"></param>
        /// <param name="Scale"></param>
        /// <param name="Restart"></param>
        public Animation(Texture2D Texture, int Width, int Height, int NbFrames, int FrameRate, float Scale, bool Restart)
        {
            texture = Texture;
            scale = Scale;
            width = Width;
            height = Height;
            nbFrames = NbFrames;
            frameTime = FrameRate;
            restart = Restart;
            active = true;
        }

        /// <summary>
        /// Pour actualiser l'animation des sprites
        /// </summary>
        /// <param name="gameTime"></param>

        public void Update(GameTime gameTime)
        {
            if(!active)
            return;

            time += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            //si le temps écoulé est plus grand que le temps choisi entre l'affichage de deux sprites, on passe au sprite suivant
            if(time>frameTime)
            {
                currentFrame++;
                //si on atteint le dernier sprite de l'image, on revient au premier sprite
                if(currentFrame==nbFrames)
                {
                    currentFrame = 0;
                    if (!restart)
                        active = false;
                }
                time = 0;
            }
            rectangleSource = new Rectangle(currentFrame * width, 0, width, height);
            rectangleDestination=new Rectangle((int)position.X -(int)(width*Scale/2), (int)position.Y - (int)(height * Scale) / 2, (int)(width * Scale), (int)(height * Scale)); 
        }

        /// <summary>
        /// pour dessiner les sprites animées
        /// </summary>
        /// <param name="spriteBatch"></param>

        public void Draw(SpriteBatch spriteBatch)
        {
            if(active)
            {
                spriteBatch.Draw(texture, rectangleDestination, rectangleSource, Color.White);
            }
        }

        /// <summary>
        /// Pour dessiner les sprites animées dans le cas où le sprite fait un mouvement de rotation
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="angle"></param>
        /// <param name="origin"></param>
        public void DrawRotate(SpriteBatch spriteBatch, float angle, Vector2 origin)
        {
            spriteBatch.Draw(texture, position, rectangleSource, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
        }
    }


}

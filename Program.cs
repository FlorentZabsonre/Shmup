using System;

namespace Shmup
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.d
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Shmup())
                game.Run();
        }
    }
#endif
}

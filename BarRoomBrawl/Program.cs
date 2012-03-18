using System;
using System.Diagnostics;

namespace BarRoomBrawl
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                Debug.WriteLine("args: {0}", args.Length);
                if (args.Length == 1)
                {
                    game.IsClient(true);
                }
                else
                {
                    game.IsClient(false);
                }
                game.Run();
            }
        }
    }
#endif
}


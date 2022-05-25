using System;

namespace TGC.MonoGame.Vigilantes9
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TGCGame())
                game.Run();

/*                 
            using (var game = new TGCDebug())
                game.Run();
 */
        }
    }
}

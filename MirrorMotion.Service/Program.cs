using System.ServiceProcess;

namespace MirrorMotion.Service
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var servicesToRun = new ServiceBase[]
                {
                    new MirrorMotionService()
                };
            ServiceBase.Run(servicesToRun);
        }
    }
}
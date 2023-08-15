using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;

namespace RogueStarIdle.CoreBusiness
{
    public class Methods
    {
        public static int GetGifDuration(string localPath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            basePath = Regex.Replace(basePath, @"bin\\Debug\\net6.0", "wwwroot");
            localPath.TrimStart('/');
            localPath = Regex.Replace(localPath, "/", "\\");
            string path = String.Concat(basePath, localPath);
            Image gif = System.Drawing.Image.FromFile(path);
            int duration = 0;
            FrameDimension dimension = new FrameDimension(gif.FrameDimensionsList[0]);
            int frameCount = gif.GetFrameCount(dimension);
            for (int i = 0; i < frameCount; i++)
            {
                gif.SelectActiveFrame(dimension, i);
                PropertyItem frameDelay = gif.GetPropertyItem(0x5100);
                int delay = BitConverter.ToInt32(frameDelay.Value, 0) * 10;
                duration += delay;
            }
            Console.WriteLine(duration.ToString());
            return duration;
        }
    }
}

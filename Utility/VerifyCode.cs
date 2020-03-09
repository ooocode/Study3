using System;
using System.Collections.Generic;
using System.Drawing;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Text;
using Color = System.DrawingCore.Color;

namespace Utility
{
    public class VerifyCodeResult
    {
        /// <summary>
        /// 验证码的Id 随机的，没什么用
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 验证码生成的字符串
        /// </summary>
        public string Str { get; set; }


        /// <summary>
        /// 验证码生成的图片数据
        /// </summary>
        public byte[] ImageData { get; set; }
    }

    /// <summary>
    /// 验证码
    /// </summary>
    public class VerifyCode
    {
        /// <summary>
        /// 字符串里面随机选 
        /// </summary>
        public const string AllChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";


        /// <summary>
        /// 创建一个验证码
        /// </summary>
        /// <returns></returns>
        public static VerifyCodeResult New()
        {
            Random random = new Random();

            string code = null;
            for (int i = 0; i < 4; i++)
            {
                var index = random.Next(AllChars.Length - 1);
                code += AllChars[index];
            }


            int codeW = 80;
            int codeH = 30;
            int fontSize = 16;

            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码 
            string[] font = { "Times New Roman" };
            //验证码的字符集，去掉了一些容易混淆的字符 

            using (Bitmap bmp = new Bitmap(codeW, codeH))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    //画噪线 
                    for (int i = 0; i < 1; i++)
                    {
                        int x1 = random.Next(codeW);
                        int y1 = random.Next(codeH);
                        int x2 = random.Next(codeW);
                        int y2 = random.Next(codeH);
                        Color clr = color[random.Next(color.Length)];
                        g.DrawLine(new Pen(clr), x1, y1, x2, y2);
                    }
                    //画验证码字符串 
                    for (int i = 0; i < code.Length; i++)
                    {
                        string fnt = font[random.Next(font.Length)];
                        Font ft = new Font(fnt, fontSize);
                        Color clr = color[random.Next(color.Length)];
                        g.DrawString(code[i].ToString(), ft, new SolidBrush(clr), (float)i * 18, (float)0);
                    }
                    //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 

                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Png);

                        string id = Guid.NewGuid().ToString("N") + DateTimeOffset.Now.UtcTicks.ToString() + Guid.NewGuid().ToString("N");

                        return new VerifyCodeResult { ImageData = ms.ToArray(), Str = code.ToLower(), Id = id };
                    }
                }
            }
        }
    }
}

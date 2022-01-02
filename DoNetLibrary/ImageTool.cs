using System.Drawing;
using System.Drawing.Drawing2D;
namespace Tools
{
    public class ImageTool
    {
        /// <summary>
        /// 生成缩略图的方式
        /// </summary>
        public enum Model
        {
            /// <summary>
            /// 指定宽高缩放（可能变形）
            /// </summary>
            WH = 0,
            /// <summary>
            /// //指定宽，高按比例
            /// </summary>
            W = 1,
            /// <summary>
            /// //指定高，宽按比例
            /// </summary>
            H = 2,
            /// <summary>
            /// //指定高宽裁减（不变形）
            /// </summary>
            Cut = 3
        }

        /// <summary>    
        /// 生成缩略图    
        /// </summary>    
        /// <param name="originalImagePath">源图路径（物理路径）</param>    
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>    
        /// <param name="width">缩略图宽度</param>    
        /// <param name="height">缩略图高度</param>    
        /// <param name="mode">生成缩略图的方式</param>        
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ImageTool.Model mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case Model.WH:
                    break;
                case Model.W:
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case Model.H:
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case Model.Cut:
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {

                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    //if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    //{

                    //    oh = originalImage.Height;
                    //    ow = originalImage.Height * towidth / toheight;
                    //    y = 0;
                    //    x = (originalImage.Width - oh) / 2;
                    //}
                    //else
                    //{
                    //    ow = originalImage.Width;
                    //    oh = originalImage.Width * height / towidth;
                    //    x = 0;
                    //    y = (originalImage.Height - ow) / 2;
                    //} 
                    break;
                default:
                    break;
            }

            //新建一个bmp图片    
            Image bitmap = new Bitmap(towidth, toheight);

            //新建一个画板    
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法    
            g.InterpolationMode = InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度    
            g.SmoothingMode = SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充    
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分    
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图    
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /**/
        /// <summary>    
        /// 在图片上增加文字水印    
        /// </summary>    
        /// <param name="Path">原服务器图片路径</param>    
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>    
        protected void AddWater(string Path, string Path_sy)
        {
            string addText = "www.tzwhx.com";
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 10);    //字体位置为左空10    
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            g.DrawString(addText, f, b, 14, 14);    //字体大小为14X14    
            g.Dispose();

            image.Save(Path_sy);
            image.Dispose();
        }

        /**/
        /// <summary>    
        /// 在图片上生成图片水印    
        /// </summary>    
        /// <param name="Path">原服务器图片路径</param>    
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>    
        /// <param name="Path_sypf">水印图片路径</param>    
        protected void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }

    }
}



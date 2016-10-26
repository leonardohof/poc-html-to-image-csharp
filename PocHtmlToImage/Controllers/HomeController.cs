using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace PocHtmlToImage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetImageFromHtml(string html)
        {
            var stream = new MemoryStream();

            var jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            var myEncoder = System.Drawing.Imaging.Encoder.Quality;

            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;            

            var image = ConvertHtmlToImage(html);
            image.Save(stream, jgpEncoder, myEncoderParameters);

            stream.Position = 0;

            return File(stream, "image/jpeg");
        }

        #region Functions

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public Image ConvertHtmlToImage(string html)
        {
            return HtmlRender.RenderToImage(html, 755, 0, Color.White);
        }

        #endregion
    }
}
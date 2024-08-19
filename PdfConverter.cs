

using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SkiaSharp;

namespace ImgToPdfConverter;

public class ConvertOptions
{
    public string OutputPath { get; set; } = "output/new-file.pdf";
    public ICollection<string> InputPagesPathes { get; set; } = [];
}

public static class PdfConverter
{
    public static void ConvertRotate(ConvertOptions opts)
    {
        const double IMG_RESIZE_KOEF = 0.90;
        const int IMG_ENCODE_QUALITY = 90;

        var document = new PdfDocument();

        foreach (var inputPath in opts.InputPagesPathes)
        {
            var page = document.AddPage();
            page.Orientation = PdfSharp.PageOrientation.Landscape;

            using (var originalImage = SKBitmap.Decode(inputPath))
            {
                int newWidth = (int)(originalImage.Width * IMG_RESIZE_KOEF);
                int newHeight = (int)(originalImage.Height * IMG_RESIZE_KOEF);

                using (var resizedImage = originalImage.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High))
                {
                    using (var imageStream = new MemoryStream())
                    {
                        resizedImage.Encode(imageStream, SKEncodedImageFormat.Jpeg, IMG_ENCODE_QUALITY);
                        imageStream.Seek(0, SeekOrigin.Begin);

                        using (var image = XImage.FromStream(imageStream))
                        {
                            var graphics = XGraphics.FromPdfPage(page);

                            page.Width = XUnit.FromPoint(image.PixelHeight);
                            page.Height = XUnit.FromPoint(image.PixelWidth);

                            double pageWidth = page.Width.Point;
                            double pageHeight = page.Height.Point;

                            graphics.RotateTransform(-90);

                            graphics.DrawImage(image, -pageHeight, 0, pageHeight, pageWidth);
                        }
                    }
                }
            }
        }

        document.Save(opts.OutputPath);
    }
}

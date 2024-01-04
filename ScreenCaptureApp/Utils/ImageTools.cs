﻿using static ScreenCaptureApp.Utils.Contains;

namespace ScreenCaptureApp.Utils;

public static class ImageTools
{
  public static int BoxCount = -1;
  public static bool IsElementPresent(Bitmap sourceImage, Bitmap elementImage, Point position)
  {
    if (position.X < 0 || position.Y < 0 || position.X + elementImage.Width > sourceImage.Width || position.Y + elementImage.Height > sourceImage.Height)
    {
      return false;
    }

    Rectangle elementRect = new Rectangle(position, elementImage.Size);
    Bitmap sourceRegion = sourceImage.Clone(elementRect, sourceImage.PixelFormat);
    //sourceRegion.Save("test.png", System.Drawing.Imaging.ImageFormat.Png);
    if (AreBitmapsEqual(sourceRegion, elementImage))
    {
      return true;
    }
    return false;
  }

  private static bool AreBitmapsEqual(Bitmap bmp1, Bitmap bmp2)
  {
    if (bmp1.Size != bmp2.Size)
    {
      return false;
    }

    for (int i = 0; i < bmp1.Width; i++)
    {
      for (int j = 0; j < bmp1.Height; j++)
      {
        if (!AreColorsWithinThreshold(bmp1.GetPixel(i, j), bmp2.GetPixel(i, j), 0.34))
        {
          return false;
        }
      }
    }

    return true;
  }
  private static bool AreColorsWithinThreshold(Color color1, Color color2, double threshold)
  {
    // 计算每个颜色分量的差异
    int deltaR = Math.Abs(color1.R - color2.R);
    int deltaG = Math.Abs(color1.G - color2.G);
    int deltaB = Math.Abs(color1.B - color2.B);

    // 计算总体颜色差异
    double totalDifference = (deltaR + deltaG + deltaB) / 3.0 / 255.0;

    // 判断是否在阈值范围内
    return totalDifference <= threshold;
  }

  public static Image? ReMoveImage(this Image? image)
  {
    if (image != null)
    {
      image.Dispose();
    }
    return null;
  }

  public static bool RestImages(this Bitmap sourceImage, string? restType, string? restModel, bool isTeam)
  {
    try
    {
      if (!EMPTY.Equals(restType) && !EMPTY.Equals(restModel))
      {
        var size = ImagesConfig.START.Equals(restModel) ?
            isTeam ?
            new Size((int)(sourceImage.Width * ImagesConfig.StartXSizeRateTeam), (int)(sourceImage.Height * ImagesConfig.StartYSizeRate)) :
            new Size((int)(sourceImage.Width * ImagesConfig.StartXSizeRate), (int)(sourceImage.Height * ImagesConfig.StartYSizeRate))
          : new Size((int)(sourceImage.Width * ImagesConfig.EndXSizeRate), (int)(sourceImage.Height * ImagesConfig.EndYSizeRate));
        Point position = ImagesConfig.START.Equals(restModel) ?
            isTeam ?
            new Point((int)(sourceImage.Width * ImagesConfig.StartXRateTeam), (int)(sourceImage.Height * ImagesConfig.StartYRateTeam)) :
            new Point((int)(sourceImage.Width * ImagesConfig.StartXRate), (int)(sourceImage.Height * ImagesConfig.StartYRate))
          : new Point((int)(sourceImage.Width * ImagesConfig.EndXRate), (int)(sourceImage.Height * ImagesConfig.EndYRate));
        Rectangle elementRect = new Rectangle(position, size);
        Bitmap sourceRegion = sourceImage.Clone(elementRect, sourceImage.PixelFormat);
        var type =
          ImagesConfig.YUHUN.Equals(restType) ? EnergyValue.YUHUN :
          ImagesConfig.JUEXIN.Equals(restType) ? EnergyValue.JUEXING :
          ImagesConfig.YULIN.Equals(restType) ? EnergyValue.YULIN :
          EMPTY;
        var team = isTeam ? "_team" : "";
        var path =
          ImagesConfig.END.Equals(restModel) ? $@"./Resource/End/end.png" :
          $@"./Resource/Start/start_{type}{team}.png";
        sourceRegion.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        return true;
      }
      else
      {
        return false;
      }
    }
    catch
    {
      return false;
    }
  }

  public static PictureBox Next(this List<PictureBox> pictureBoxes)
  {
    var pictureBoxesCount = pictureBoxes.Count;
    BoxCount += 1;
    if (BoxCount + 1 > pictureBoxesCount)
    {
      throw new ArgumentOutOfRangeException();
    }
    return pictureBoxes[BoxCount];
  }
}

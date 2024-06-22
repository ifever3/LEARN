using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEARN.WF;

public class opencv
{
    public static void ocv()
    {
        Mat img1 = new Mat("C:\\Users\\JAYCE.O\\source\\repos\\Visual Studio\\LEARN\\LEARN.WF\\resource\\learn.png", ImreadModes.Color);

        Cv2.Rotate(img1, img1, RotateFlags.Rotate90Counterclockwise);//逆时针90度
        Cv2.Rotate(img1, img1, RotateFlags.Rotate90Clockwise);//顺时针90度
        Cv2.Flip(img1, img1, FlipMode.Y);//水平翻转
        Cv2.Flip(img1, img1, FlipMode.X);//垂直翻转

        Point center = new Point(500, 525);//声明坐标
        Point txt = new Point(450, 610);
        Cv2.Circle(img1, center, 35, 130, 1);
        Cv2.PutText(img1, "Check", txt, HersheyFonts.HersheyComplex, 2.0, 15, 2);

        Point text = new Point(300, 300);
        Vec3b btr = img1.At<Vec3b>(500, 450);
        int a = btr[0];
        int b = btr[1];
        int c = btr[2];

        Cv2.PutText(img1, Convert.ToString(a) + "-" + Convert.ToString(b) + "-" + Convert.ToString(c), text, HersheyFonts.HersheyComplex, 2.0, 15, 2);

        for (var i = 0; i < 500; i++)
            for (var j = 0; j < 500; j++)
            {
                img1.At<Vec3b>(i, j)[2] = 255;
            }

        Salt(img1, 10000);

        colorReduce(img1, 100);

        Cv2.ImShow("l", img1);

        reverse(img1);

        Cv2.WaitKey(0); // 等待用户按下任意键后关闭窗口

    }

    private static void Salt(Mat img, int n)
    {
        Random rd = new Random();
        for (int i = 0; i < n; i++)
        {
            int r = rd.Next(1, 600);
            int c = rd.Next(1, 600);
            img.At<Vec3b>(r, c)[0] = 255;
            img.At<Vec3b>(r, c)[1] = 255;
            img.At<Vec3b>(r, c)[2] = 255;
        }
    }

    private static void colorReduce(Mat img, int div)
    {
        int r = img.Rows;
        int c = img.Cols;
        for (int i = 0; i < r; i++)
            for (int j = 0; j < c; j++)
            {
                img.At<Vec3b>(i, j)[0] = (byte)(img.At<Vec3b>(i, j)[0] / div * div + div / 2);
                img.At<Vec3b>(i, j)[1] = (byte)(img.At<Vec3b>(i, j)[1] / div * div + div / 2);
                img.At<Vec3b>(i, j)[2] = (byte)(img.At<Vec3b>(i, j)[2] / div * div + div / 2);
            }
    }
    private static void reverse(Mat img)
    {
        int r = img.Rows;
        int c = img.Cols;
        for (int i = 0; i < r; i++)
            for (int j = 0; j < c; j++)
            {
                img.At<Vec3b>(i, j)[0] = (byte)(255 - img.At<Vec3b>(i, j)[0]);
                img.At<Vec3b>(i, j)[1] = (byte)(255 - img.At<Vec3b>(i, j)[1]);
                img.At<Vec3b>(i, j)[2] = (byte)(255 - img.At<Vec3b>(i, j)[2]);
            }
        Cv2.ImShow("win1", img);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.Geometry;

namespace ImageEditor.DrawingObjects
{
    public class TagUI : IDrawingUI
    {
        public double X
        {
            get; set;
        }
        public double Y
        {
            get; set;
        }
        public string TagText  //显示Text
        {
            get; set;
        }
        public int TagType  //Tag类型  0表示At好友  1表示热门话题  2表示插入位置
        {
            get; set;
        }
        public Size Bound  //画布边界
        {
            get; set;
        }
        private Rect _region;
        public Rect Region
        {
            get
            {
                return _region;
            }
        }
        private Rect _close_region;
        public Rect CloseRegion
        {
            get
            {
                return _close_region;
            }
        }
        public bool ShowCloseBtn
        {
            get;set;
        }
        private bool _translated = false;
        public void Draw(CanvasDrawingSession graphics, float scale)
        {
            var x = X * scale;
            var y = Y * scale;

            var radius = 4;
            var color = Color.FromArgb(200, 0xFF, 0xFF, 0xFF);
            var color2 = Color.FromArgb(200, 0x00, 0x00, 0x00);

            graphics.FillCircle((float)x, (float)y, radius, color);
            graphics.DrawCircle((float)x, (float)y, radius, color2);
            if (!_translated)
            {
                _translated = true;
                if (TagType == 0)
                {
                    TagText = "@" + TagText;
                }
                else if (TagType == 1)
                {
                    TagText = "#" + TagText + "#";
                }
                else
                {
                    //TagText = "↓" + TagText;
                }
            }
            var ctFormat = new CanvasTextFormat { FontSize = 11.0f, WordWrapping = CanvasWordWrapping.NoWrap, FontFamily="微软雅黑" };
            var ctLayout = new CanvasTextLayout(graphics, TagText, ctFormat, 0.0f, 0.0f);
            //字体占用高度、宽度
            var width = ctLayout.DrawBounds.Width + 10;
            var height = ctLayout.DrawBounds.Height + 12;

            var w = x + width + 10 + height;
            var pathBuilder = new CanvasPathBuilder(graphics);


            if (w > Bound.Width)  //超出画布边界
            {
                pathBuilder.BeginFigure((float)x - 5, (float)y);
                pathBuilder.AddLine((float)x - 5 - 6, (float)y - (float)height / 2);
                pathBuilder.AddLine((float)x - (float)width - 5 - 6, (float)y - (float)height / 2);
                pathBuilder.AddLine((float)x - (float)width - 5 - 6, (float)y + (float)height / 2);
                pathBuilder.AddLine((float)x - 5 - 6, (float)y + (float)height / 2);

                pathBuilder.EndFigure(CanvasFigureLoop.Closed);

                var geometry = CanvasGeometry.CreatePath(pathBuilder);

                graphics.FillGeometry(geometry, color2);
                graphics.DrawText(TagText, (float)x - (float)width - 5 - 6 + 5, (float)y - (float)height / 2 + 4, Colors.White, ctFormat);


                _region = new Rect((float)x - (float)width - 5 - 6, (float)y - (float)height / 2, width, height);
                _close_region = new Rect((float)x - (float)width - 5 - 6 - height, (float)y - (float)height / 2, height, height);

                if (ShowCloseBtn && scale == 1) //显示关闭按钮 且没有缩放
                {
                    graphics.FillRectangle(_close_region, color2);
                    graphics.DrawLine((float)_close_region.Left, (float)_close_region.Top, (float)_close_region.Left, (float)_close_region.Top + (float)height, Colors.White, 0.5f);
                    graphics.DrawText("×", (float)_close_region.Left + 6, (float)_close_region.Top, Colors.White,new CanvasTextFormat() { FontSize=15, FontFamily="微软雅黑" });
                }
            }
            else
            {
                pathBuilder.BeginFigure((float)x + 5, (float)y);
                pathBuilder.AddLine((float)x + 5 + 6, (float)y - (float)height / 2);
                pathBuilder.AddLine((float)x + (float)width + 5 + 6, (float)y - (float)height / 2);
                pathBuilder.AddLine((float)x + (float)width + 5 + 6, (float)y + (float)height / 2);
                pathBuilder.AddLine((float)x + 5 + 6, (float)y + (float)height / 2);

                pathBuilder.EndFigure(CanvasFigureLoop.Closed);

                var geo = CanvasGeometry.CreatePath(pathBuilder);

                graphics.FillGeometry(geo, color2);
                graphics.DrawText(TagText, (float)x + 5 + 4 + 5, (float)y - (float)height / 2 + 4, Colors.White, ctFormat);

                _region = new Rect((float)x + 5 + 6, (float)y - (float)height / 2, width, height);
                _close_region = new Rect((float)x + (float)width + 5 + 6, (float)y - (float)height / 2, height, height);

                if (ShowCloseBtn && scale == 1) //显示关闭按钮  且 没有缩放
                {
                    graphics.FillRectangle(_close_region, color2);
                    graphics.DrawLine((float)_close_region.Left, (float)_close_region.Top, (float)_close_region.Left, (float)_close_region.Top + (float)height, Colors.White, 0.5f);
                    graphics.DrawText("×", (float)_close_region.Left + 6, (float)_close_region.Top, Colors.White, new CanvasTextFormat() { FontSize = 15, FontFamily = "微软雅黑" });
                }
            }

        }
    }
}

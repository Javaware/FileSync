using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProgressBarSample
{
    

    public enum ProgressBarDisplayText
    {
        Percentage,
        CustomText
    }

    class CustomProgressBar : ProgressBar
    {
        private String content;
        public CustomProgressBar(String content)
        {
            this.content = content;
        }

        public void UpdateText()
        {
            using (Graphics gr = this.CreateGraphics())
            {
                gr.DrawString(this.content, Font, new SolidBrush(Color.Black),
                    new PointF(0,
                    Height / 2 - (gr.MeasureString(content, Font).Height / 2.0F)));
            }
        }
    }
}
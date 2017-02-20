using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UIBlocks.MaterialDesignTheme
{
    public class ColorZone:ContentControl
    {
        static ColorZone()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorZone),new FrameworkPropertyMetadata(typeof(ColorZone)));
        }

        #region CornerRadius，方块圆角
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
    "CornerRadius", typeof(CornerRadius), typeof(ColorZone), new PropertyMetadata(default(CornerRadius)));
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

    }
}

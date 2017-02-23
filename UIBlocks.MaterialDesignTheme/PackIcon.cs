using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UIBlocks.MaterialDesignTheme
{
    public class PackIcon : PackIconBase<PackIconKind>
    {
        static PackIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIcon), new FrameworkPropertyMetadata(typeof(PackIcon)));
        }

        public PackIcon() : base(PackIconDataFactory.Create) { }
    }
}

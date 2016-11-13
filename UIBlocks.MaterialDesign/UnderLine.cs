using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UIBlocks.MaterialDesign
{
    [TemplateVisualState(GroupName = "ActivationStates", Name = "active")]
    [TemplateVisualState(GroupName = "ActivationStates", Name = "inactive")]
    public  class UnderLine:Control
    {
        public static bool GetIsActive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsActiveProperty);
        }
        public static void SetIsActive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsActiveProperty, value);
        }
        /// <summary>
        /// 依赖属性，判断下划线是否处于被使用状态
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(UnderLine), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, IsActivePropertyChange));
        
        //当被激活和取消激活时的动画
        private static void IsActivePropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((UnderLine)obj).GotoVisualState((bool)e.NewValue);
        }
        private void GotoVisualState(bool val)
        {
            if(val)
            {
                bool ans=VisualStateManager.GoToState(this, "active", true);
                
                Console.WriteLine(ans.ToString());
            }
            else
            {
                bool ans=VisualStateManager.GoToState(this, "inactive", true);
                Console.WriteLine(ans.ToString()+"fffff");
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            GotoVisualState(false);
        }
        static UnderLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UnderLine), new FrameworkPropertyMetadata(typeof(UnderLine)));
        }
    }
}

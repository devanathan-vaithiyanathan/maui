using System;
using System.Collections.Generic;
using System.Text;

namespace Maui.Controls.Sample.Controls.Input;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ExtendedRefreshView : RefreshView
{
    public double Threshold
    {
        get => (double)GetValue(ThresholdProperty);
        set => SetValue(ThresholdProperty, value);
    }

    public static readonly BindableProperty ThresholdProperty = BindableProperty.Create(
        nameof(Threshold),
        typeof(double),
        typeof(ExtendedRefreshView)
    );
}
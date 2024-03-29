﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Microsoft.CodeAnalysis.CodeActions;
using RoslynPad.Roslyn.CodeActions;

namespace WrapperApiSampleApp;

/// <summary>
///     From
///     <see
///         href="https://github.com/roslynpad/roslynpad/blob/73372a15821287161dbaaf4415b81daef44f17eb/src/RoslynPad/Formatting/CodeActionsConverter.cs" />
///     .
/// </summary>
internal sealed class CodeActionsConverter : MarkupExtension, IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((CodeAction)value).GetCodeActions();
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
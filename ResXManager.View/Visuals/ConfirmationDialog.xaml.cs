﻿namespace tomenglertde.ResXManager.View.Visuals
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    using JetBrains.Annotations;

    using TomsToolbox.Composition;
    using TomsToolbox.Wpf;
    using TomsToolbox.Wpf.Composition;

    /// <summary>
    /// Interaction logic for MoveToResourceDialog.xaml
    /// </summary>
    public partial class ConfirmationDialog
    {
        private ConfirmationDialog()
        {
            InitializeComponent();
        }

        public static bool? Show([NotNull] IExportProvider exportProvider, [CanBeNull] object content, [CanBeNull] string title, [CanBeNull] Window owner)
        {
            var window = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = owner ?? Application.Current?.MainWindow,
                Title = title,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.SingleBorderWindow,
                SizeToContent = SizeToContent.WidthAndHeight,
                Icon = new BitmapImage(new Uri("pack://application:,,,/ResXManager.View;component/16x16.png"))
            };

            window.SetExportProvider(exportProvider);
            window.Resources.MergedDictionaries.Add(DataTemplateManager.CreateDynamicDataTemplates(exportProvider));
            window.SetResourceReference(StyleProperty, TomsToolbox.Wpf.Composition.Styles.ResourceKeys.WindowStyle);
            window.Content = new ConfirmationDialog { Content = content };

            return window.ShowDialog();
        }

        [NotNull]
        public ICommand CommitCommand => new DelegateCommand(CanCommit, Commit);

        private void Commit()
        {
            var window = Window.GetWindow(this);
            if (window == null)
                return;

            window.DialogResult = true;
        }

        private bool CanCommit()
        {
            return !this.VisualDescendants().Any(Validation.GetHasError);
        }
    }
}

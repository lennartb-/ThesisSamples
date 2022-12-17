using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AugmentationFrameworkSampleApp.Controls;

/// <summary>
///     https://stackoverflow.com/a/9802346/368354
/// </summary>
[TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
public class CachingTabControl : TabControl
{
    private Panel? itemsHolderPanel;

    public CachingTabControl()
    {
        // This is necessary so that we get the initial data-bound selected item
        ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
    }

    /// <summary>
    ///     If containers are done, generate the selected item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ItemContainerGenerator_StatusChanged(object? sender, EventArgs e)
    {
        if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
        {
            ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
            UpdateSelectedItem();
        }
    }

    /// <summary>
    ///     Get the ItemsHolder and generate any children
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        itemsHolderPanel = GetTemplateChild("PART_ItemsHolder") as Panel;
        UpdateSelectedItem();
    }

    /// <summary>
    ///     When the items change we remove any generated panel children and add any new ones as necessary
    /// </summary>
    /// <param name="e"></param>
    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);

        if (itemsHolderPanel == null)
        {
            return;
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Reset:
                itemsHolderPanel.Children.Clear();
                break;

            case NotifyCollectionChangedAction.Add:
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        var cp = FindChildContentPresenter(item);
                        if (cp != null)
                        {
                            itemsHolderPanel.Children.Remove(cp);
                        }
                    }
                }

                // Don't do anything with new items because we don't want to
                // create visuals that aren't being shown
                break;

            case NotifyCollectionChangedAction.Replace:
                break;
        }

        UpdateSelectedItem();
    }

    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        base.OnSelectionChanged(e);
        UpdateSelectedItem();
    }

    private void UpdateSelectedItem()
    {
        if (itemsHolderPanel == null)
        {
            return;
        }

        // Generate a ContentPresenter if necessary
        var item = GetSelectedTabItem();
        if (item != null)
        {
            CreateChildContentPresenter(item);
        }

        // show the right child
        foreach (ContentPresenter child in itemsHolderPanel.Children)
        {
            if (child.Tag is TabItem tabItem)
            {
                child.Visibility = tabItem.IsSelected ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }

    private void CreateChildContentPresenter(object? item)
    {
        if (item == null)
        {
            return;
        }

        var contentPresenter = FindChildContentPresenter(item);

        if (contentPresenter != null)
        {
            return;
        }

        // the actual child to be added.  cp.Tag is a reference to the TabItem
        contentPresenter = new ContentPresenter
        {
            Content = item is TabItem tabItem ? tabItem.Content : item,
            ContentTemplate = SelectedContentTemplate,
            ContentTemplateSelector = SelectedContentTemplateSelector,
            ContentStringFormat = SelectedContentStringFormat,
            Visibility = Visibility.Collapsed,
            Tag = item is TabItem ? item : ItemContainerGenerator.ContainerFromItem(item)
        };
        itemsHolderPanel?.Children.Add(contentPresenter);
    }

    private ContentPresenter? FindChildContentPresenter(object data)
    {
        if (data is TabItem item)
        {
            data = item.Content;
        }

        if (data == null)
        {
            return null;
        }

        if (itemsHolderPanel == null)
        {
            return null;
        }

        foreach (ContentPresenter cp in itemsHolderPanel.Children)
        {
            if (cp.Content == data)
            {
                return cp;
            }
        }

        return null;
    }

    protected TabItem? GetSelectedTabItem()
    {
        var selectedItem = SelectedItem;
        if (selectedItem == null)
        {
            return null;
        }

        if (selectedItem is not TabItem item)
        {
            return ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;
        }

        return item;
    }
}


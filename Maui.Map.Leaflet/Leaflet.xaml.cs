using Maui.Map.Leaflet.Exceptions;
using Maui.Map.Leaflet.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Maui.Map.Leaflet;

public partial class Leaflet : WebView
{
	

    #region Events
    public event EventHandler<Pin[]> PinAdded;
    #endregion
    #region Collection Properties

    public static BindableProperty PinsProperty = BindableProperty.Create(
        propertyName: nameof(Pins),
        returnType: typeof(IEnumerable<Pin>),
        declaringType: typeof(Leaflet),
        defaultValue: new ObservableCollection<Pin>(), 
        propertyChanged: (bindable, oldValue, newValue) => OnItemsSourceChanged(bindable, oldValue, newValue));

    #region Handler
    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var wraplayout = bindable as WebView;

            if (e.OldItems != null)
                foreach (var item in e.OldItems)
                {
                    //! Remove form WebView with js
                }

            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                {
                    //! Qdd item WebView with js
                }

        }
        var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

        if (null != oldValueINotifyCollectionChanged)
        {
            oldValueINotifyCollectionChanged.CollectionChanged -= newValueINotifyCollectionChanged_CollectionChanged;
        }

        var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;

        if (null != newValueINotifyCollectionChanged)
        {
            newValueINotifyCollectionChanged.CollectionChanged += newValueINotifyCollectionChanged_CollectionChanged;
        }
    }
    #endregion
    public IList<Pin> Pins
    {
        get => (IList<Pin>)GetValue(PinsProperty);
        set => SetValue(PinsProperty, value);
    }
    #endregion
    #region Commands Properties


    public Command PinAddedCommand
    {
        get { return (Command)GetValue(PinAddedCommandProperty); }
        set { SetValue(PinAddedCommandProperty, value); }
    }
    public static BindableProperty PinAddedCommandProperty = BindableProperty.Create(
        propertyName: nameof(PinAddedCommand),
        returnType: typeof(Command),
        declaringType: typeof(Leaflet),
        defaultValue: null);

    #endregion

    #region Constructor
    public Leaflet()
    {
        InitializeComponent();
    }
    #endregion
    #region Overrides
    public virtual void OnLoad()
    {

    }
    public virtual void OnPinsAdded(Pin[] pins)
    {

    }
    #endregion

    #region Actions
    public void AddPin(params Pin[] pins)
    {

        pins.ToList().ForEach(pin =>
        {
            if (string.IsNullOrEmpty(pin.Key))
                throw new PinMustHaveKeyException();
            if (Pins.Select(p => p.Key).Contains(pin.Key))
                throw new PinAlreadyExistException();
            Pins.Add(pin);
        });
        PinAdded?.Invoke(this, pins);
        OnPinsAdded(pins);
    }

    public Pin UpdatePin(Pin pin)
    {
        if(string.IsNullOrEmpty(pin.Key))
            throw new PinMustHaveKeyException();
        
        if(!Pins.Select(p => p.Key).Contains(pin.Key))
            throw new PinMustExistException();

        Pins.Remove(pin);
        Pins.Add(pin);

        return pin;
    }

    public void DeletePin(Pin pin)
    {
        if (string.IsNullOrEmpty(pin.Key))
            throw new PinMustHaveKeyException();

        if (!Pins.Select(p => p.Key).Contains(pin.Key))
            throw new PinMustExistException();

        Pins.Remove(pin);
    }
    #endregion

}
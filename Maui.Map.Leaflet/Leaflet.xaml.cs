using Maui.Map.Leaflet.EventArgs;
using Maui.Map.Leaflet.Exceptions;
using Maui.Map.Leaflet.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace Maui.Map.Leaflet;

public partial class Leaflet : WebView
{

#if ANDROID
    partial void ChangedHandler(object sender);
    partial void ChangingHandler(object sender, HandlerChangingEventArgs e);
#endif
    partial void ExecuteJavascript(object sender, string jsScript);
#region Events
    public event EventHandler<Pin[]> PinAdded;
    public event EventHandler<MapClickedEventArg> MapCliqued;
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
            var map = bindable as Leaflet;

            if (e.OldItems != null)
                foreach (var pin in e.OldItems)
                {
                    //! Remove form WebView with js
                }

            if (e.NewItems != null)
                foreach (var pin in e.NewItems)
                {
#if WINDOWS
                    map.EvaluateJavaScriptAsync($"AddPin({((Pin)pin).Latitude},{((Pin)pin).Longitude},`{((Pin)pin).Key}`)");
#endif
                    //! Add item WebView with js
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
#if WINDOWS
    public string Port 
    {
        get { return (string)GetValue(PortProperty); }
        set { SetValue(PortProperty, value); }
    }

    public static BindableProperty PortProperty = BindableProperty.Create(
        propertyName: nameof(Port),
        returnType: typeof(string),
        declaringType: typeof(Leaflet),
        defaultValue: "9696"
    );
#endif
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

    public Command MapTappedCommand
    {
        get { return (Command)GetValue(MapTappedCommandProperty); }
        set { SetValue(MapTappedCommandProperty, value); }
    }
    public static BindableProperty MapTappedCommandProperty = BindableProperty.Create(
        propertyName: nameof(MapTappedCommand),
        returnType: typeof(Command),
        declaringType: typeof(Leaflet),
        defaultValue: null);

#endregion

#region Constructor
    public Leaflet()
    {
        InitializeComponent();
        Navigated += Leaflet_Navigated;
#if WINDOWS
    LeafletController.LeafletServerRequest += LeafletController_LeafletServerRequest;
#endif
    }



    private void Leaflet_Navigated(object sender, WebNavigatedEventArgs e) =>
        ExecuteJavascriptFunctionAsync("InitializeMap()");
#endregion
#region Overrides
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
#if ANDROID
        ChangedHandler(this);
#endif
    }

    protected override void OnHandlerChanging(HandlerChangingEventArgs args)
    {
        base.OnHandlerChanging(args);
#if ANDROID
        ChangingHandler(this,args);
#endif
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
#if WINDOWS || ANDROID
        Source = new HtmlWebViewSource()
        {
            Html = _htmlCode
        };
#endif
        
#if WINDOWS

        if(_server == null)
        {
            _server = new ËmbeddedServer();
        }
        _server.InitializeServer(Port);
#endif
    }
#endregion
#region Virtuals
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

            if(!(Pins?.FirstOrDefault(p => p.Key == pin.Key) is null))
                throw new PinAlreadyExistException();
#if ANDROID
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-EN");
            ExecuteJavascript(this,$"AddPin({pin.Latitude},{pin.Longitude},`{pin.Key}`)");
#else
            ExecuteJavascriptFunctionAsync($"AddPin({pin.Latitude},{pin.Longitude},`{pin.Key}`)");
#endif
            Pins.Add(pin);
        });
        PinAdded?.Invoke(this, pins);
        OnPinsAdded(pins);
    }

    private async void ExecuteJavascriptFunctionAsync(string script) => await EvaluateJavaScriptAsync(script);

    public Pin UpdatePin(Pin pin)
    {
        if(string.IsNullOrEmpty(pin.Key))
            throw new PinMustHaveKeyException();
        
        if(Pins.FirstOrDefault(p => p.Key == pin.Key) is null)
            throw new PinMustExistException();

        Pins.Remove(pin);
        Pins.Add(pin);

        return pin;
    }

    public void DeletePin(Pin pin)
    {
        if (string.IsNullOrEmpty(pin.Key))
            throw new PinMustHaveKeyException();

        if (Pins.FirstOrDefault(p => p.Key == pin.Key) is null)
            throw new PinMustExistException();

        Pins.Remove(pin);
    }
   
#endregion
}
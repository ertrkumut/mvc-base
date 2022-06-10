# MVC-BASE

#### This is a mvc base framework for Unity Engine.

## Creating Root & Context

>Navigate **Tools/MVC/Create Root**.
#### In the editor window, there is a field for Root name. Editor automatically create a test scene for you. If you don't want to create a new scene you need to make Create Scene toggle to false. If you want, you can override root scene path.
#### In context, there are some of methods automatically overrode for you.

#### ==> You can bind signals in this method.
```csharp
public override void SignalBindings()
{
    base.SignalBindings();
}
```

#### ==> You can bind models or other classes that you want in this method.
```csharp
public override void InjectionBindings()
{
    base.InjectionBindings();
}
```

#### ==> You can bind view and mediators in this method.
```csharp
public override void MediationBindings()
{
    base.MediationBindings();
}
```

#### ==> You can bind commands in this method.
```csharp
public override void CommandBindings()
{
    base.CommandBindings();
}
```

#### In inspector, there are some toggles and fields that shows some information about status of **Context**. At the beginning of game, Bindings methods executed one by one. If you want you can disable these methods in inspector.
#### Initialize Order is a order number which is very helpful in multi context scenes. You can choose context's initialize order with this parameter. 
[![GameContext](https://i.postimg.cc/R02CzDJn/Screen-Shot-2022-05-14-at-4-55-54-PM.png)](https://postimg.cc/xkyVvgR0)










## Creating View-Mediator
#### In **mvc-base** there is a editor window that helps to create views inside of Unity-Editor. 

>Navigate **Tools/MVC/Create View**.

#### There are some field you need to fill up. The editor shows all Context classes in the project. You need to choose current context related to your view or you can choose Global. The editor window organized folder structure for you. It automatically create view and mediator.
#### You need to bind your views and mediators in your context. If you don't bind, the system will not register your mediator.
```csharp
public override void MediationBindings()
{
    base.MediationBindings();
    
    MediationBinder.Bind<PlayerView>().To<PlayerMediator>();
}
```

#### In mediator there are 2 methods called `OnRegister` and `OnRemove`. `OnRegister` methods execute automatically when your mediator registered. `OnRemove` method execute when your view removed from system.
```csharp
public class PlayerMediator : IMediator
{
    [Inject] private PlayerView _view { get; set; }
    
    public void OnRegister()
    {
    }

    public void OnRemove()
    {
    }
}
```

#### In mvc-base, mediators are automatically created as a lite-mediator. You can not see them in inspector. If you want to see mediator in inspector, you can inherit from MonoBehaviour. But there is one component called `ViewInjector`. You can track view status in inspector.
#### You can test `OnRegister` and `OnRemove` methods by clicking buttons from inspector. Also you can disable auto register toggle.
#### If auto-register is disabled, you can register view like below and you can remove registration by calling RemoveRegistration method. 
```csharp
_playerView.InjectView();
_playerView.RemoveRegistration();
```

[![ViewInjector-Component](https://i.postimg.cc/wTb296Bb/Screen-Shot-2022-05-15-at-11-35-39-AM.png)](https://postimg.cc/sv77cC7p)

















## Commands

#### Commands work with signals. You need to bind command or commands to signal.
#### To execute command, you need to `Dispatch` signal.
```csharp
public class PlayerSignals
{
    public Signal InitializePlayer;
}
```

```csharp
public override void CommandBindings()
{
    base.CommandBindings();

    CommandBinder.Bind(_playerSignals.InitializePlayer)
        .To<InitializePlayerDataCommand>();
}
```

```csharp
_playerSignals.InitializePlayer.Dispatch();
```

#### There are 5 type signals that 4 of them can take parameters. You can send parameter in `Dispatch` method. You can get signal parameters by using `[SignalParam]` attribute.
```csharp
public Signal<double> AddCurrency;
```
```csharp
public class AddCurrencyCommand : Command
{
    [Inject] private IPlayerModel _playerModel;
    
    [SignalParam] private double _currencyAmount;

    public override void Execute()
    {
        // Add currency here!
        _playerModel.AddCurrency(_currencyAmount);
    }
}
```

```csharp
_playerSignals.AddCurrency.Dispatch(100d);
```
#### You can use commands in sequence. And you can pass parameters in sequence commands, using `Release` method. if you run `Release` method, system going to execute next command in the sequence. But if you want to use Release method, you need to call `Retain` method before. Retain stops sequence in current command and wait for Release method.
```csharp
CommandBinder.Bind(_playerSignals.AddCurrency)
    .To<AddCurrencyCommand>()
    .To<SavePlayerDataCommand>()
    .InSequence();
```

```csharp
public class AddCurrencyCommand : Command
{
    [Inject] private IPlayerModel _playerModel;
    
    [SignalParam] private double _currencyAmount;

    public override void Execute()
    {
        Retain();
        
        _playerModel.AddCurrency(_currencyAmount);
        
        Release(_playerModel);
    }
}
    
public class SavePlayerDataCommand : Command<IPlayerModel>
{
    public override void Execute(IPlayerModel playerModel)
    {
        playerModel.SaveCurrencyData();
    }
}
```

## Functions

#### Functions are the best feature in this framework. They work like commands but as you know, commands can not return values. Also if you want to execute command, you need to dispatch signal. But functions work by `FunctionProvider`. Functions created for some functions that developer need to execute wherever he/she wants. It's kinda injectable methods. There are two kind of functions, first one can return value, the other one is void function.

#### If you want to use Functions, you need to Inject `IFunctionProvider` property.

```csharp
[Inject] private IFunctionProvider _functionProvider;
```

#### In this example, as you can see function have 2 generic types. The first type specify the return type of Function. And other types specify Execute method parameter types.

```csharp
public class CalculateDamageFunction : FunctionReturn<double, string>
{
    [Inject] private IPlayerModel _playerModel;
    [Inject] private IWeaponsModel _weaponsModel;
    
    public override double Execute(string weaponId)
    {
        var weaponConfigVO = _weaponsModel.GetConfigVO(weaponId);

        var damage = weaponConfigVO.baseDamage * _playerModel.GetPlayerDamageMultiplier();
        return damage;
    }
}
```

#### Simple execution for this function like this:

```csharp
_functionProvider
    .Execute<CalculateDamageFunction>()
    .AddParams(_weaponId)
    .SetReturn<double>();
```



















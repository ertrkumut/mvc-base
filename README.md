# MVC-BASE

#### This is a mvc base framework for Unity Engine.

## Creating Root & Context

>Navigate **Tools/MVC/Create Root**.
#### In the editor window, there is a field for Root name. Editor automatically create a test scene for you. If you don't want to create a new scene you need to make Create Scene toggle to false. If you want, you can override root scene path.
#### In context, there are some of methods automatically overrode for you.

#### ==> You can bind signals in this method.
```
public override void SignalBindings()
{
    base.SignalBindings();
}
```

#### ==> You can bind models or other classes that you want in this method.
```
public override void InjectionBindings()
{
    base.InjectionBindings();
}
```

#### ==> You can bind view and mediators in this method.
```
public override void MediationBindings()
{
    base.MediationBindings();
}
```

#### ==> You can bind commands in this method.
```
public override void CommandBindings()
{
    base.CommandBindings();
}
```

![GameContext](https://ibb.co/BsjNsrm)










## Creating View-Mediator
#### In **mvc-base** there is a editor window that helps to create views inside of Unity-Editor. 

>Navigate **Tools/MVC/Create View**.

#### There are some field you need to fill up. The editor shows all Context classes in the project. You need to choose current context related to your view or you can choose Global. The editor window organized folder structure for you. It automatically create view and mediator.
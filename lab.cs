using System;
using System.Collections.Generic;

// Интерфейс команды
public interface ICommand
{
    void Execute();
    void Undo();
}

// =================== Устройства (Receivers) ===================
public class Light
{
    public void On() => Console.WriteLine("💡 Свет включен.");
    public void Off() => Console.WriteLine("💡 Свет выключен.");
}

public class Television
{
    public void On() => Console.WriteLine("📺 Телевизор включен.");
    public void Off() => Console.WriteLine("📺 Телевизор выключен.");
}

// =================== Команды ===================
public class LightOnCommand : ICommand
{
    private Light _light;
    public LightOnCommand(Light light) => _light = light;
    public void Execute() => _light.On();
    public void Undo() => _light.Off();
}

public class LightOffCommand : ICommand
{
    private Light _light;
    public LightOffCommand(Light light) => _light = light;
    public void Execute() => _light.Off();
    public void Undo() => _light.On();
}

public class TelevisionOnCommand : ICommand
{
    private Television _tv;
    public TelevisionOnCommand(Television tv) => _tv = tv;
    public void Execute() => _tv.On();
    public void Undo() => _tv.Off();
}

public class TelevisionOffCommand : ICommand
{
    private Television _tv;
    public TelevisionOffCommand(Television tv) => _tv = tv;
    public void Execute() => _tv.Off();
    public void Undo() => _tv.On();
}

// =================== Пульт (Invoker) ===================
public class RemoteControl
{
    private ICommand _onCommand;
    private ICommand _offCommand;
    private Stack<ICommand> _commandHistory = new Stack<ICommand>();

    public void SetCommands(ICommand onCommand, ICommand offCommand)
    {
        _onCommand = onCommand;
        _offCommand = offCommand;
    }

    public void PressOnButton()
    {
        _onCommand?.Execute();
        _commandHistory.Push(_onCommand);
    }

    public void PressOffButton()
    {
        _offCommand?.Execute();
        _commandHistory.Push(_offCommand);
    }

    public void PressUndoButton()
    {
        if (_commandHistory.Count > 0)
        {
            var lastCommand = _commandHistory.Pop();
            lastCommand.Undo();
        }
        else
        {
            Console.WriteLine("⛔ Нет команд для отмены.");
        }
    }
}

// =================== Клиентский код ===================
class Program
{
    static void Main(string[] args)
    {
        Light light = new Light();
        Television tv = new Television();

        ICommand lightOn = new LightOnCommand(light);
        ICommand lightOff = new LightOffCommand(light);

        ICommand tvOn = new TelevisionOnCommand(tv);
        ICommand tvOff = new TelevisionOffCommand(tv);

        RemoteControl remote = new RemoteControl();

        // Управление светом
        Console.WriteLine("=== Управление светом ===");
        remote.SetCommands(lightOn, lightOff);
        remote.PressOnButton();
        remote.PressOffButton();
        remote.PressUndoButton();

        Console.WriteLine();

        // Управление телевизором
        Console.WriteLine("=== Управление телевизором ===");
        remote.SetCommands(tvOn, tvOff);
        remote.PressOnButton();
        remote.PressOffButton();
        remote.PressUndoButton();
    }
}

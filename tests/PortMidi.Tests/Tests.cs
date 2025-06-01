using System;
using System.Linq;
using System.Threading;
using PortMidi;
using NUnit.Framework;

public class EqualExamples
{
	[Test]
	public void EqualStringIgnoreCase()
	{
		var devices = MidiDeviceManager.AllDevices;
		foreach (var device in devices)
		{
			Console.WriteLine(device.ToString());
		}
		var toDelugeDevice = devices.First(d => d.Name.Contains("Deluge") && d.IsOutput);

		using var toDeluge = MidiDeviceManager.OpenOutput(toDelugeDevice.ID, 10);

		for (var i = 0; i < 24; i++)
		{
			var start = i * 20;
			toDeluge.Write(new MidiEvent
			{
				Message = new MidiMessage(0x90, 60 + i, 127),
				Timestamp = start,
			});

			toDeluge.Write(new MidiEvent
			{
				Message = new MidiMessage(0x80, 60 + i, 127),
				Timestamp = start + 15,
			});
		}
		Thread.Sleep(1000);
	}
}
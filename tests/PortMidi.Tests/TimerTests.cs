#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PortMidi.Tests;

public class TimerTests
{
	[Test, Explicit]
	public async Task Start()
	{
		MidiTimer.Stop();
		MidiTimer.Start(1);
		await Task.Delay(10);
		// this is flaky => explicit test
		var time = MidiTimer.Time();
		Assert.That(time, Is.Positive.And.LessThan(100));
	}

	[Test]
	public async Task StartWithCallback()
	{
		MidiTimer.Stop();
		var timestamps = new List<int>();
		MidiTimer.Ticked += (_, time) => timestamps.Add(time);
		MidiTimer.Start(1);
		await Task.Delay(10);
		Assert.That(timestamps.Count, Is.Positive);
	}

	[Test]
	public void StartWithCallbackThreadSleep()
	{
		MidiTimer.Stop();
		var timestamps = new List<int>();
		MidiTimer.Ticked += (_, time) => timestamps.Add(time);
		MidiTimer.Start(1);
		Thread.Sleep(10);
		Assert.That(timestamps.Count, Is.Positive);
	}

	[Test]
	public void CallbackIsFiredOnDifferentThread()
	{
		MidiTimer.Stop();
		var callingThread = Thread.CurrentThread;
		Thread? callbackThread = null;
		MidiTimer.Ticked += (_, time) => callbackThread = Thread.CurrentThread;
		MidiTimer.Start(1);
		Thread.Sleep(10);
		Assert.That(callbackThread.ManagedThreadId, Is.Not.EqualTo(Thread.CurrentThread.ManagedThreadId));
	}
}
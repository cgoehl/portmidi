using System;
using System.Collections.Generic;
using static PortMidi.PortMidiMarshal;
using PmError = PortMidi.MidiErrorType;

namespace PortMidi
{
	public class MidiTimer
	{
		public static void Sleep(int milliseconds) => Pt_Sleep(milliseconds);
		
		public static void Start(int resolution)
		{
			var e = Pt_Start(resolution, TimerProcCallback, IntPtr.Zero);
			if (e != PmError.NoError)
			{
				throw new MidiException(e, $"Failed to open MIDI input device {e}");
			}
		}

		private static void TimerProcCallback(int milliseconds, IntPtr userdata)
		{
			OnTimerFired(milliseconds);
		}

		public static event EventHandler<int> Ticked;

		public static bool Started => Pt_Stared() != 0;

		public static void Stop() => Pt_Stop();

		public static int Time() => Pt_Time();

		private static void OnTimerFired(int e)
		{
			try
			{
				Ticked?.Invoke(null, e);
			}
			catch (Exception exception)
			{
				OnCallbackError(exception);
			}
		}

		public static Action<Exception> OnCallbackError { get; set; } = _ => { };
	}
}
using System;
using Gtk;
namespace Notepad
{
	public class Notepad
	{
		public Notepad ()
		{
		}
		public static void Main(string[] args)
		{
			Application.Init ();
			new NotepadView ();
			Application.Run ();

		}

	}
}
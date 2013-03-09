using System;
using System.IO;
using Gtk;
namespace Notepad
{
	public partial class NotepadView : Gtk.Window // Holy shit thats cool
	{
		private string programName = "MyNotePad";
		private string[] authors = {"MalcolmLc"};
		private const string version = "0.1"; // Now the version makes a little more sense version.

		private string savePath;

		private string SavePath{
			set{savePath = value;this.Title = savePath +" - "+programName;}
			get{return savePath;}
		}

		public NotepadView () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			bindEvents ();
			bindMenuActions ();
			initUi ();
		}
		protected void initUi()
		{
			Title = programName;
			
		}
		
		protected void bindEvents()
		{
			DeleteEvent += OnDeleteEvent;
			textBody.Buffer.Changed += ontextBodyChange;
		}
		protected void bindMenuActions()
		{

			quitAction.Activated +=  onQuitAction;

			filesaveAction.Activated += onSaveAction;
			filesaveAsAction.Activated += onSaveAsAction;

			openAction1.Activated += onOpenAction;

			aboutAction.Activated += onAboutAction;
			newAction.Activated += onNewAction;
			statusBarAction.Toggled += ontoggleStatusbarAction;
		}
		//---------------------------------
		// Menu Actions
		protected void onAboutAction(object sender , EventArgs e )
		{
			AboutDialog dialog = new AboutDialog ();
			dialog.Version = version;
			dialog.Authors = authors;
			dialog.ProgramName = programName;
			dialog.Comments = "A basic basic text editor";

			// The following makes the dialog hide when needed
			dialog.DeleteEvent += (object o, DeleteEventArgs args) =>{
				dialog.Hide();
			};
			dialog.Response+= (object o, ResponseArgs args) =>{
				ResponseType id = args.ResponseId;
				if ( id == ResponseType.Close || id == ResponseType.Cancel){
					dialog.Hide();
				}
			};

			// and now run the dialog
			dialog.Run ();
		


		}
		protected void ontoggleStatusbarAction (object sender, EventArgs e)
		{
			/*
			Check the state of the toggle status bar button and do actions accordingly
			 */
			bool state = statusBarAction.Active;
			if (state) {			
				statusbar.Show ();
			} else {
				statusbar.Hide();
			}
		}
		protected void onQuitAction (object sender, EventArgs e){Application.Quit ();}
		protected void onSaveAction (object sender ,EventArgs e )
		{
			// if the path is empty , run the saveAs action
			if (SavePath == null) {
				this.onSaveAsAction (sender, e);
			} else {
				System.IO.File.WriteAllText(SavePath,textBody.Buffer.Text);
				statusbar.Push (1, SavePath+ " Saved");
			}




		}
		protected void onSaveAsAction (object sender , EventArgs e)
		{
			FileChooserDialog fc = new FileChooserDialog("Save As",
			                                             this,
			                                             FileChooserAction.Save,
			                                             "Cancel",ResponseType.Cancel,
			                                             "Save",ResponseType.Accept);
			fc.Run ();
			SavePath=fc.Filename;
			fc.Destroy ();

			//onSaveAction (sender, e);
		}
		protected void onOpenAction( object sender, EventArgs e)
		{
			FileChooserDialog fc = new FileChooserDialog("Choose the file to open",
				                          this,
				                          FileChooserAction.Open,
				                          "Cancel",ResponseType.Cancel,
				                          "Open",ResponseType.Accept);
			fc.Run ();
			SavePath = fc.Filename;
			string data = System.IO.File.ReadAllText (SavePath);
			textBody.Buffer.Text = data;
			statusbar.Push (1, SavePath+ " Opened");

			//textBody.Buffer.Text;
			fc.Destroy ();




		}
		protected void onNewAction (object sender, EventArgs e)
		{

			textBody.Buffer.Text = "";
			SavePath = null;
			Title = "New Document - " + programName;
			statusbar.Push (1, "New Document");
		}

		//---------------------------------
		// Events
		protected void OnDeleteEvent(object obj, DeleteEventArgs args){Application.Quit ();}
		protected void ontextBodyChange (object sender, EventArgs e)
		{
			statusbar.Push (1, SavePath+ " Edited");
		}
	}
}


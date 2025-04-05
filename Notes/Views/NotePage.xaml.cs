using System.Threading.Tasks;

namespace Notes.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class NotePage : ContentPage
{
	string _fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");
	public NotePage()
	{
		InitializeComponent();

		string appDataPath = FileSystem.AppDataDirectory;
		string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";

		LoadNote(Path.Combine(appDataPath, randomFileName));
	}

	public string ItemId
	{
		set { LoadNote(value); }
	}

	private void LoadNote(string fileName)
	{
		Model.Note noteModel = new Model.Note();
		noteModel.FileName = fileName;

		if (File.Exists(fileName))
		{
			noteModel.Date = File.GetCreationTime(fileName);
			noteModel.Text = File.ReadAllText(fileName);
		}
		BindingContext = noteModel;
	}

	private async void SaveButton_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Model.Note note)
			File.WriteAllText(note.FileName, TextEditor.Text);

		await Shell.Current.GoToAsync("..");
	}

	private async void DeleteButton_Clicked(object sender, EventArgs e)
	{
		if (BindingContext is Model.Note note)
		{
			//Delete the file.
			if (File.Exists(note.FileName))
				File.Delete(note.FileName);
		}

		await Shell.Current.GoToAsync("..");
	}
}
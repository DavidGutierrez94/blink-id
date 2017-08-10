using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using System.Diagnostics;

namespace BlinkIDApp
{
	public partial class BlinkIDPage : ContentPage
	{
		public BlinkIDPage ()
		{
			InitializeComponent ();

			// Display results in Editor
			MessagingCenter.Subscribe<Messages.BlinkIDResults> (this, Messages.BlinkIDResultsMessage, (sender) => {
				Debug.WriteLine (sender.Results);

				string asString = "";
				string[] res2;
				string[] res3;
				string[] divider;
				//Regex prueba = new Regex("^[A-Z]$"); 
				foreach (var result in sender.Results) {
					asString += string.Join (";", result);
				}
				/*var res = prueba.Match(asString);
				if (res.Success) {
					 res2 = res.ToString();
				}*/
				res2 = asString.Split(' ');
				var temp = res2[1].Replace('\0','@');
				var temp2 = temp.Trim('@');
				res3 = Regex.Split(temp2,@"[@]");
				List<string> list = new List<string>(res3);
				for (int index = 0; index<list.Count; index++)
				{
				    bool nullOrEmpty = string.IsNullOrEmpty(list[index]);
				    if (nullOrEmpty)
				    {
				        list.RemoveAt(index);
				        --index;
				    }
				}
				var temporal = list[3];
				divider = Regex.Split(temporal, @"[A-Z]");
				list[8] = divider[0];
				divider = Regex.Split(temporal, @"[0-9]");
				list[9] = divider[10];
				Device.BeginInvokeOnMainThread (() => {
					resultsEditor0.Text = list[0];
					resultsEditor1.Text = list[2];
					resultsEditor2.Text = list[8];
					resultsEditor3.Text = list[9]+" "+list[4];
					resultsEditor4.Text = list[5]+" "+list[6];
					resultsEditor.Text = list[7];

				});

			});

			// Display metadata image in Image
			MessagingCenter.Subscribe<Messages.BlinkIDImage> (this, Messages.BlinkIDImageMessage, (sender) => {
				Device.BeginInvokeOnMainThread (() => {
					metadataImage.Source = sender.Image;
				});
			});
		}

		void StartScan (object sender, EventArgs e)
		{
			var blinkId = DependencyService.Get<IBlinkID> ();
			blinkId.Scan ();
		}
	}
}


#region License
/*
Copyright © 2014-2019 European Support Limited

Licensed under the Apache License, Version 2.0 (the "License")
you may not use this file except in compliance with the License.
You may obtain a copy of the License at 

http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and 
limitations under the License. 
*/
#endregion

using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace Ginger.UserControlsLib.TextEditor
{
	/// <summary>
	/// Implements AvalonEdit ICompletionData interface to provide the entries in the completion drop down.
	/// </summary>
	public class TextCompletionData : ICompletionData
	{
	    public TextCompletionData(string text)
		{
			this.Text = text;
		}

        public TextCompletionData(string Text, string Description) : this(Text)
        {
        }

        public ImageSource Image { get; set; }
		
		public string Text { get; private set; }
		
		// Use this property if you want to show a fancy UIElement in the drop down list.
		public object Content {
			get { return this.Text; }
		}
		
		public object Description { get; set; }
		
		public double Priority { get { return 0; } }

        ImageSource ICompletionData.Image { get { throw new NotImplementedException(); } }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
		{        
            textArea.Document.Replace(completionSegment.Offset -1, 1 , this.Text);
		}
	}
}

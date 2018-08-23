#region License
/*
Copyright © 2014-2018 European Support Limited

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
using System.ComponentModel;

namespace GingerCoreNET.RunLib
{
    public class GingerNodeInfo : INotifyPropertyChanged
    {
        public Guid SessionID { get; set; }
        public string Name { get; set; }
        // public string PluginId { get; set; }
        // DO we want version?

        public string ServiceId { get; set; }
        public string IP { get; set; }
        public string Host { get; set; }
        public string OS { get; set; }
        string mPing;
        public string Ping
        {
            get { return mPing; }
            set { if (mPing != value) { mPing = value; OnPropertyChanged(nameof(Ping)); } } }

        // TOdo change to enum
        private string mStatus;        
        public string Status { get { return mStatus; } set { if (mStatus != value) { mStatus = value; OnPropertyChanged(nameof(Status)); } } }  
        
        public event PropertyChangedEventHandler PropertyChanged;

        int mActionCount = 0;
        public int ActionCount { get { return mActionCount;  } }
        public void IncreaseActionCount()
        {
            mActionCount++;
            OnPropertyChanged(nameof(ActionCount));
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        // TOOD: add drivers info
    }
}

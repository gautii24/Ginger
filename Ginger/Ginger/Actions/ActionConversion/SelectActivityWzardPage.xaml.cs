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

using amdocs.ginger.GingerCoreNET;
using Amdocs.Ginger.Common;
using Ginger.UserControls;
using GingerCore;
using GingerCore.Environments;
using GingerWPF.WizardLib;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using static Ginger.ExtensionMethods;

namespace Ginger.Actions.ActionConversion
{
    /// <summary>
    /// Interaction logic for SelectActivityWzardPage.xaml
    /// </summary>
    public partial class SelectActivityWzardPage : Page, IWizardPage
    {
        ActionsConversionWizard mWizard;
        public SelectActivityWzardPage()
        {
            InitializeComponent();
        }

        public void WizardEvent(WizardEventArgs WizardEventArgs)
        {
            switch (WizardEventArgs.EventType)
            {
                case EventType.Init:
                    mWizard = (ActionsConversionWizard)WizardEventArgs.Wizard;
                    SetGridsView();
                    break;
                case EventType.Active:
                    break;
                case EventType.Finish:
                    break;
            }
        }

        private void SetGridsView()
        {
            GridViewDef defView = new GridViewDef(GridViewDef.DefaultViewName);
            defView.GridColsView = new ObservableList<GridColView>();
            defView.GridColsView.Add(new GridColView() { Field = Activity.Fields.SelectedForConversion, WidthWeight = 2.5, MaxWidth = 50, StyleType = GridColView.eGridColStyleType.CheckBox, Header = "Select" });
            defView.GridColsView.Add(new GridColView() { Field = Activity.Fields.ActivityName, WidthWeight = 15, Header = "Name of " + GingerDicser.GetTermResValue(eTermResKey.Activity) });
            grdGroups.SetAllColumnsDefaultView(defView);
            grdGroups.InitViewItems();
            grdGroups.SetTitleLightStyle = true;

            if (mWizard.BusinessFlow.Activities.Where(x => x.SelectedForConversion == true).Count() != 0)
            {
                mWizard.BusinessFlow.Activities.Where(x => x.SelectedForConversion == true).ToList().ForEach(x => { x.SelectedForConversion = false; });
            }
            grdGroups.DataSourceList = GingerCore.General.ConvertListToObservableList(mWizard.BusinessFlow.Activities.Where(x => x.Active == true).ToList());
            grdGroups.RowChangedEvent += grdGroups_RowChangedEvent;
            grdGroups.Title = "Name of " + GingerDicser.GetTermResValue(eTermResKey.Activities) + " in '" + mWizard.BusinessFlow.Name + "'";
            grdGroups.MarkUnMarkAllActive += MarkUnMarkAllActivities;
        }

        private void grdGroups_RowChangedEvent(object sender, EventArgs e)
        {
            if (mWizard.BusinessFlow != null)
            {
                mWizard.BusinessFlow.CurrentActivity = (Activity)grdGroups.CurrentItem;
                if (mWizard.BusinessFlow.CurrentActivity != null)
                    ((Activity)mWizard.BusinessFlow.CurrentActivity).PropertyChanged += CurrentActivity_PropertyChanged;
            }
        }

        private void CurrentActivity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HandlerType")
                grdGroups.setDefaultView();
        }

        private void MarkUnMarkAllActivities(bool ActiveStatus)
        {
            if (grdGroups.DataSourceList.Count <= 0) return;
            if (grdGroups.DataSourceList.Count > 0)
            {
                ObservableList<Activity> lstMarkUnMarkActivities = (ObservableList<Activity>)grdGroups.DataSourceList;
                foreach (Activity act in lstMarkUnMarkActivities)
                {
                    act.SelectedForConversion = ActiveStatus;
                }
                grdGroups.DataSourceList = lstMarkUnMarkActivities;
            }
        }
    }
}
